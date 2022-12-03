using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.DownloadedApps
{
    [Authorize(GmailServerPermissions.DownloadedApps.Default)]
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
        }
    }
}
