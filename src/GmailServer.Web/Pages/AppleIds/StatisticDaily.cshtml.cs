using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GmailServer.Web.Pages.AppleIds
{
    [Authorize(GmailServerPermissions.AppleIds.StatisticDaily)]
    public class StatisticDailyModel : GmailServerPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Username { get; set; }

        public void OnGet()
        {
            ViewData.Add("usernameParam", SerializeObject(Username));
        }
    }
}
