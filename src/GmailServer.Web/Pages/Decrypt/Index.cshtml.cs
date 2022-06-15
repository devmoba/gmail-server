using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.Decrypt
{
    [Authorize(GmailServerPermissions.Decrypts.Default)]
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
        }
    }
}
