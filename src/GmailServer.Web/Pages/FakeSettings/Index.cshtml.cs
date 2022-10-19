using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.FakeSettings
{
    [Authorize(GmailServerPermissions.FakeSettings.Default)]
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
        }
    }
}
