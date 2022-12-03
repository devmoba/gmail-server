using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.AppleIds
{
    [Authorize(GmailServerPermissions.DownloadedApps.Default)]
    public class DownloadedAppModalModel : GmailServerPageModel
    {
        public void OnGet()
        {
        }
    }
}
