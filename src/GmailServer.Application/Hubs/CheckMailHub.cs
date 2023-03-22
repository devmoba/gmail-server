using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace GmailServer.Hubs
{
    [Authorize]
    [HubRoute("/signalr-hubs/check-mail")]
    public class CheckMailHub : AbpHub<ICheckMailHub>
    {
        private const string ConnectionName = "CheckMailTool";

        public CheckMailHub()
        {

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
            else
            {
                ConnectionMapping<string>.GetInstance().Remove(currentUser.UserName, Context.ConnectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
