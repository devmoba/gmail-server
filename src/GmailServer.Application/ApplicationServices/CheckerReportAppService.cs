using GmailServer.CheckerReports;
using GmailServer.CheckMails;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Hubs;
using GmailServer.Repositories;
using GmailServer.TaskChecks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IHubContext<CheckMailHub, ICheckMailHub> _hubContext;

        public CheckerReportAppService(ICheckerRepository checkerRepository,
            IGmailRepository gmailRepository,
            ITaskCheckRepository taskCheckRepository,
            IHubContext<CheckMailHub, ICheckMailHub> hubContext)
        {
            _gmailRepository = gmailRepository;
            _checkerRepository = checkerRepository;
            _taskCheckRepository = taskCheckRepository;
            _hubContext = hubContext;
        }

        public async Task<ReportResponseDto> ReportAsync(ReportRequestDto input)
        {
            var checker = await AsyncExecuter.FirstOrDefaultAsync(
                _checkerRepository.Where(x => x.CheckerId == input.CheckerId));

            if (checker == null)
            {
                await _checkerRepository.InsertAsync(new Checker()
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
            }

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

                    await _gmailRepository.BulkUpdateAsync(
                        gmails,
                        new List<string>()
                        {
                            nameof(Gmail.Status),
                            nameof(Gmail.Updated)
                        });
                }

                if (taskCheckResult.TypeCheck == TypeCheck.Browser)
                {
                    var connections = ConnectionMapping<string>
                      .GetInstance()
                      .GetConnections(taskCheckResult.Username)
                      .ToList();
                    await _hubContext
                        .Clients
                        .Clients(connections)
                        .ReceiveEmailResultAsync(taskCheckResult.EmailResults);
                    var emailResultGroups = taskCheckResult.EmailResults
                                    .GroupBy(x => x.Status)
                        .Select(group => new EmailResultGroup()
                        {
                            Status = group.Key,
                            EmailResultOuput = string.Join('\n', group.Select(x => $"{x.Email}|{Enum.GetName(typeof(Status), x.Status)}").ToList()),
                            Count = group.Count()
                        }).ToList();
                    foreach (var item in emailResultGroups)
                    {
                        await _hubContext.Clients
                            .Clients(connections)
                            .ReceiveEmailResultGroupAsync(
                                item.EmailResultOuput,
                                item.Status,
                                item.Count
                            );
                    }
                }
                await _taskCheckRepository.DeleteAsync(taskCheckResult.Id);
            }

            var taskChecks = await AsyncExecuter.ToListAsync(
                _taskCheckRepository.Where(x => x.Status == TaskCheckStatus.NA));
            var taskCheckDtos = ObjectMapper.Map<List<TaskCheck>, List<TaskCheckDto>>(taskChecks);
            taskChecks.ForEach(x => x.Status = TaskCheckStatus.Checking);

            await _taskCheckRepository.BulkUpdateAsync(
                taskChecks, 
                new List<string>() { 
                    nameof(TaskCheck.Status) 
                });

            return new ReportResponseDto() { TaskChecks = taskCheckDtos };
        }
    }
}
