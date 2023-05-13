using GmailServer.AppleIds;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.AppleIds
{
    [Authorize(GmailServerPermissions.AppleIds.Statistic)]
    public class StatisticModel : GmailServerPageModel
    {
         private readonly IAppleIdAppService _appleIdAppService;

        public StatisticModel(IAppleIdAppService appleIdAppService)
        {
            _appleIdAppService = appleIdAppService;
        }

        public async void OnGet()
        {
            var usernameSelections = await _appleIdAppService.GetUsernameSelectionAsync();
            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        }
    }
}
