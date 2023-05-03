using GmailServer.AppleIdNones;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.AppleIdNones
{
    [Authorize(GmailServerPermissions.AppleIdNones.Statistic)]
    public class StatisticModel : GmailServerPageModel
    {
        private readonly IAppleIdNoneAppService _appService;

        public StatisticModel(IAppleIdNoneAppService appService)
        {
            _appService = appService;
        }

        public async void OnGet()
        {
            var usernameSelections = await _appService.GetUsernameSelectionAsync();

            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        }
    }
}
