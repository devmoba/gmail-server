using GmailServer.EmailChecks;
using GmailServer.Entities;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace GmailServer.Hubs
{
    [Authorize]
    [HubRoute("/signalr-hubs/check-mail")]
    public class CheckMailHub : AbpHub<ICheckMailHub>
    {
        private const string ConnectionName = "CheckMailTool";
        private readonly IGmailRepository _gmailRepository;

        public CheckMailHub(IGmailRepository gmailRepository)
        {
            _gmailRepository = gmailRepository;
        }

        public override Task OnConnectedAsync()
        {
            var currentUser = CurrentUser;
            if (currentUser.IsInRole("check-mail-tool"))
            {
                ConnectionMapping<string>.GetInstance().Add(ConnectionName, Context.ConnectionId);
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
