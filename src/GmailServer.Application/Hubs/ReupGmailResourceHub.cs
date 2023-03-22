using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace GmailServer.Hubs
{
    [Authorize(GmailServerPermissions.GmailResources.ReupEmail)]
    [HubRoute("/signalr-hubs/reup-gmailresource")]
    public class ReupGmailResourceHub : AbpHub<IReupGmailResourceHub>
    {
        public override Task OnConnectedAsync()
        {
            var currentUser = CurrentUser;
            if (!currentUser.IsInRole("check-mail-tool"))
            {
                ConnectionMapping<string>.GetInstance().Add(currentUser.UserName, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var currentUser = CurrentUser;
            ConnectionMapping<string>.GetInstance().Remove(currentUser.UserName, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
