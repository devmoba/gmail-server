using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.AppleOrders
{
    [Authorize(GmailServerPermissions.AppleOrders.Statistic)]
    public class StatisticByLinkStatusModel : GmailServerPageModel
    {
        public void OnGet()
        {
        }
    }
}
