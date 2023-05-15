using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.AppleIds
{
    [Authorize(GmailServerPermissions.AppleIds.Statistic)]
    public class StatisticModel : GmailServerPageModel
    {
        public async void OnGet()
        {
        }
    }
}
