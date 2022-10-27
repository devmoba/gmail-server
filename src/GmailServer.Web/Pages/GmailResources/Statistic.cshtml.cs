using GmailServer.GmailResources;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.Statistic)]
    public class StatisticModel : GmailServerPageModel
    {
        private readonly IGmailResourceAppService _gmailResourceAppService;

        public StatisticModel(IGmailResourceAppService gmailResourceAppService)
        {
            _gmailResourceAppService = gmailResourceAppService;
        }

        public async void OnGet()
        {
            var usernames = await _gmailResourceAppService.GetUsernameSelectionAsync();
            var usernameSelections = usernames.Select(item => new SelectListItem()
            {
                Text = item,
                Value = item
            });

            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        }
    }
}
