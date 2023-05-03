using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GmailServer.Web.Pages.AppleOrders
{
    [Authorize(GmailServerPermissions.AppleOrders.Statistic)]
    public class StatisticByAddPaymentStatusModel : GmailServerPageModel
    {
        public void OnGet()
        {
        }
    }
}
