using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.AppleIdRaws
{
    [Authorize(GmailServerPermissions.AppleIdRaws.Statistic)]
    public class StatisticModel : GmailServerPageModel
    {
        public void OnGet()
        {
        }
    }
}
