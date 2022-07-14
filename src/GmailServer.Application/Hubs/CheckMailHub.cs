using GmailServer.CheckMails;
using GmailServer.EmailChecks;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.Repositories;
using GmailServer.TaskPools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.SignalR;

namespace GmailServer.Hubs
{
    [Authorize]
    [HubRoute("/signalr-hubs/check-mail")]
    public class CheckMailHub : AbpHub<ICheckMailHub>
    {
        private const string ConnectionName = "CheckMailTool";
        private readonly IGmailRepository _gmailRepository;
        private readonly ICheckerRepository _checkerRepository;
        private readonly ITaskCheckRepository _taskCheckRepository;
        private readonly IConfiguration _cfg;

        public CheckMailHub(IGmailRepository gmailRepository,
            ICheckerRepository checkerRepository,
            ITaskCheckRepository taskCheckRepository,
            IConfiguration configuration)
        {
            _gmailRepository = gmailRepository;
            _checkerRepository = checkerRepository;
            _taskCheckRepository = taskCheckRepository;
            _cfg = configuration;
        }

        public override Task OnConnectedAsync()
        {
            var currentUser = CurrentUser;
            if (currentUser.IsInRole("check-mail-tool"))
            {
                ConnectionMapping<string>.GetInstance().Add(ConnectionName, Context.ConnectionId);
            } 
            else
            {
                ConnectionMapping<string>.GetInstance().Add(currentUser.UserName, Context.ConnectionId);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var currentUser = CurrentUser;

            if (currentUser.IsInRole("check-mail-tool"))
            {
                ConnectionMapping<string>.GetInstance().Remove(ConnectionName, Context.ConnectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task InputEmailCheckAsync(List<EmailCheck> input)
        {
            var limit = _cfg.GetValue<int>("CheckMail:MailPerTaskCheck");
            var emailCheckSplit = EnumerableExtension.Split<EmailCheck>(
                        input,
                        (int)Math.Ceiling((decimal)input.Count / limit)).ToList();
            foreach (var emailChecks in emailCheckSplit)
            {
                var checker = await _checkerRepository.GetCheckerOnlineFirst();
                if (checker != null)
                {
                    await _taskCheckRepository.InsertAsync(new TaskCheck()
                    {
                        CheckerId = checker.Id,
                        Username = CurrentUser.UserName,
                        EmailChecks = string.Join('|', emailChecks),
                        Status = TaskCheckStatus.NA,
                        TypeCheck = TypeCheck.Browser,
                        Created = DateTime.Now
                    }, autoSave: true);
                }
            }
        }

        public async Task ReportEmailResultAsync(List<EmailResult> emailResults)
        {
            if (emailResults.Count > 0)
            {
                emailResults = emailResults.OrderBy(x => x.Id).ToList();
                var ids = emailResults.Select(x => x.Id).ToList();

                var entities = await _gmailRepository.GetByListIdAsync(ids);
                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].Status = emailResults[i].Status;
                    entities[i].Updated = DateTime.Now;
                }

                await _gmailRepository.BulkUpdateAsync(
                    entities,
                    new List<string>()
                    {
                        nameof(Gmail.Status),
                        nameof(Gmail.Updated)
                    });
            }
        }
    }
}
