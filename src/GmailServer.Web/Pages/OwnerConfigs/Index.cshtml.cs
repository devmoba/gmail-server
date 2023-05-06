using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.OwnerConfigs
{
    [Authorize(GmailServerPermissions.OwnerConfigs.Default)]
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
        }
    }
}
