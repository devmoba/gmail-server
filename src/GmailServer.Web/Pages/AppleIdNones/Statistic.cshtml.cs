using GmailServer.AppleIdNones;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.AppleIdNones
{
    [Authorize(GmailServerPermissions.AppleIdNones.Statistic)]
    public class StatisticModel : GmailServerPageModel
    {
        public void OnGet()
        {
           
        }
    }
}
