using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages
{
    [Authorize]
    public class IndexModel : GmailServerPageModel
    {
        
    }
}