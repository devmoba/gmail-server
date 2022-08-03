using GmailServer.CheckerReports;
using GmailServer.CheckMails;
using GmailServer.EmailChecks;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.Hubs;
using GmailServer.Repositories;
using GmailServer.TaskChecks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    [Authorize]
    public class CheckerReportAppService : ApplicationService, ICheckerReportAppService
    {
        private readonly ICheckerRepository _checkerRepository;
        private readonly IGmailRepository _gmailRepository;
        private readonly ITaskCheckRepository _taskCheckRepository;
        private readonly IConfiguration _cfg;
        private readonly IHubContext<CheckMailHub, ICheckMailHub> _hubContext;
        private readonly ILogger<CheckerReportAppService> _logger;
        private static ConcurrentDictionary<long, SemaphoreSlim> CheckerOnlineSyncLocks = new ConcurrentDictionary<long, SemaphoreSlim>();

        public CheckerReportAppService(ICheckerRepository checkerRepository,
            IGmailRepository gmailRepository,
            ITaskCheckRepository taskCheckRepository,
            IConfiguration configuration,
            IHubContext<CheckMailHub, ICheckMailHub> hubContext,
            ILogger<CheckerReportAppService> logger)
        {
            _gmailRepository = gmailRepository;
            _checkerRepository = checkerRepository;
            _taskCheckRepository = taskCheckRepository;
            _hubContext = hubContext;
            _cfg = configuration;
            _logger = logger;
        }

        public async Task InputEmailChecksAsync(List<EmailCheck> input)
        {
            var limit = _cfg.GetValue<int>("CheckMail:MailPerTaskCheck"); // 50 email / TaskCheck
            var emailCheckSplit = EnumerableExtension.Split<EmailCheck>(
                        input,
                        (int)Math.Ceiling((decimal)input.Count / limit)).ToList();
            foreach (var emailChecks in emailCheckSplit)
            {
                var checker = await TryGetCheckerOnlineAsync();
                if (checker != null)
                {
                    try
                    {
                        await _taskCheckRepository.InsertAsync(new TaskCheck()
                        {
                            CheckerId = checker.Id,
                            Username = CurrentUser.UserName,
                            EmailChecks = JsonConvert.SerializeObject(emailChecks),
                            Status = TaskCheckStatus.NA,
                            TypeCheck = TypeCheck.Browser,
                            Created = DateTime.Now
                        }, autoSave: true);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    var connections = ConnectionMapping<string>
                     .GetInstance()
                     .GetConnections(CurrentUser.UserName)
                     .ToList();
                    await _hubContext.Clients
                        .Clients(connections)
                        .ReceiveNotiAsync($"Non Checker Client!", "danger");
                    return;
                }
            }
        }

        public async Task<ReportResponseDto> ReportAsync(ReportRequestDto input)
        {

            _logger.LogInformation($"{input.CheckerId} - {CurrentUser.UserName} reporting...");
            var checker = await AsyncExecuter.FirstOrDefaultAsync(
                _checkerRepository.Where(x => x.CheckerId == input.CheckerId));

            if (checker == null)
            {
                checker = await _checkerRepository.InsertAsync(new Checker()
                {
                    CheckerId = input.CheckerId,
                    CheckerIP = input.CheckerIP,
                    Status = CheckerStatus.Online,
                    FreeRam = input.FreeRam,
                    TotalRam = input.TotalRam,
                    UsingThread = input.UsingThread,
                    MaxThread = input.MaxThread,
                    LastCheck = DateTime.Now,
                    Created = DateTime.Now
                }, autoSave: true);
                _logger.LogInformation($"{input.CheckerId} - {CurrentUser.UserName} Creating new checker!");
            }
            else
            {
                checker.CheckerIP = input.CheckerIP;
                checker.Status = CheckerStatus.Online;
                checker.FreeRam = input.FreeRam;
                checker.TotalRam = input.TotalRam;
                checker.UsingThread = input.UsingThread;
                checker.MaxThread = input.MaxThread;
                checker.LastCheck = DateTime.Now;
                await _checkerRepository.UpdateAsync(checker, autoSave: true);
                _logger.LogInformation($"{input.CheckerId} - {CurrentUser.UserName} Update checker done!");
            }
            var syncLock = CheckerOnlineSyncLocks.GetOrAdd(checker.Id, new SemaphoreSlim(1, 1));

            if (input.TaskCheckResults.Count > 0)
            {
                _logger.LogInformation($"{input.CheckerId} - {CurrentUser.UserName} TaskCheckResult count: {input.TaskCheckResults.Count}");
                foreach (var taskCheckResult in input.TaskCheckResults)
                {
                    if (taskCheckResult.TypeCheck == TypeCheck.OwnerDB)
                    {
                        var emailResults = taskCheckResult.EmailResults.OrderBy(x => x.Id).ToList();
                        var ids = taskCheckResult.EmailResults.Select(x => x.Id).ToList();

                        var gmails = await _gmailRepository.GetByListIdAsync(ids);

                        for (int i = 0; i < gmails.Count; i++)
                        {
                            gmails[i].Status = emailResults[i].Status;
                            gmails[i].Updated = DateTime.Now;
                        }
                        await _gmailRepository.BulkUpdateAsync(gmails, new List<string>()
                        {
                            nameof(Gmail.Status),
                            nameof(Gmail.Updated)
                        });
                    }

                    if (taskCheckResult.TypeCheck == TypeCheck.Browser)
                    {
                        _logger.LogInformation($"Get connection by {taskCheckResult.Username}");
                        var connections = ConnectionMapping<string>
                          .GetInstance()
                          .GetConnections(taskCheckResult.Username)
                          .ToList();
                        await _hubContext
                            .Clients
                            .Clients(connections)
                            .ReceiveEmailResultAsync(taskCheckResult.EmailResults);
                        _logger.LogInformation($"ReceiveEmailResult done!");
                    }
                    //await _taskCheckRepository.DeleteAsync(taskCheckResult.Id);
                }
                await _taskCheckRepository.BulkDeleteAsync(input.TaskCheckResults.Select(x => x.Id).ToList());
            }
            var taskChecks = await AsyncExecuter.ToListAsync(
                   _taskCheckRepository.Where(
                       x => x.Status == TaskCheckStatus.NA &&
                       x.CheckerId == checker.Id)
                   );
            var taskCheckDtos = ObjectMapper.Map<List<TaskCheck>, List<TaskCheckDto>>(taskChecks);

            //await syncLock.WaitAsync();
            try
            {
                taskChecks.ForEach(x => x.Status = TaskCheckStatus.Checking);
                await _taskCheckRepository.BulkUpdateAsync(
                    taskChecks,
                    new List<string>() {
                    nameof(TaskCheck.Status)
                    });
            }
            catch (Exception ex)
            {

            }
            //finally
            //{
            //    syncLock.Release();
            //}
            return new ReportResponseDto() { TaskChecks = taskCheckDtos };
        }

        private async Task<Checker> TryGetCheckerOnlineAsync()
        {
            var count = 0;
            while (count < 5)
            {
                var checker = await _checkerRepository.GetCheckerOnlineFirstAsync();
                if (checker != null)
                    return checker;
                count++;
                Thread.Sleep(3000);
            }
            return null;
        }
    }
}
