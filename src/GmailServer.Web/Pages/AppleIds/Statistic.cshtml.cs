using GmailServer.AppleIds;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

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
            //var usernameSelections = usernames.Select(item => new SelectListItem()
            //{
            //    Text = item,
            //    Value = item
            //}).ToList();

            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        }
    }
}
