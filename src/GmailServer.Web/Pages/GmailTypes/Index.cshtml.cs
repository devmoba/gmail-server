using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GmailServer.Web.Pages.GmailTypes
{
    [Authorize(GmailServerPermissions.GmailTypes.Default)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
