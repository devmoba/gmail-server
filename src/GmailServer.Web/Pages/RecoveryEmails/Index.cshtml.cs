using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GmailServer.Web.Pages.RecoveryEmails
{
    [Authorize(GmailServerPermissions.RecoveryEmails.Default)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
