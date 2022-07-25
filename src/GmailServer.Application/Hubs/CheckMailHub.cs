using GmailServer.CheckMails;
using GmailServer.EmailChecks;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.Repositories;
using GmailServer.TaskPools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    }
}
