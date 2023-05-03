using GmailServer.MomoAccounts;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.MomoAccounts
{
    [Authorize(GmailServerPermissions.MomoAccounts.Statistic)]
    public class StatisticModel : GmailServerPageModel
    {
        private readonly IMomoAccountAppService _appService;

        public StatisticModel(IMomoAccountAppService appService)
        {
            _appService = appService;
        }
        public async void OnGet()
        {
            var uploadGroupSelections = await _appService.GetUploadGroupSelectionAsync();

            ViewData.Add("uploadGroupSelections", SerializeObject(uploadGroupSelections));
        }
    }
}
