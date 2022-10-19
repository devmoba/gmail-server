using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GmailServer.Web.Pages.Checkers
{
    [Authorize(GmailServerPermissions.Checkers.Default)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
