using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.CheckMails
{
    [Authorize(GmailServerPermissions.CheckMails.Default)]
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
        }
    }
}
