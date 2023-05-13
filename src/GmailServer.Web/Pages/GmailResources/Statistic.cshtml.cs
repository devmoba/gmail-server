using GmailServer.GmailResources;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.Statistic)]
    public class StatisticModel : GmailServerPageModel
    {
        //private readonly IGmailResourceAppService _gmailResourceAppService;

        //public StatisticModel(IGmailResourceAppService gmailResourceAppService)
        //{
        //    _gmailResourceAppService = gmailResourceAppService;
        //}

        //public async void OnGet()
        //{
        //    var usernameSelections = await _gmailResourceAppService.GetUsernameSelectionAsync();
        //    ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        //}
    }
}
