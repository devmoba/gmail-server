using GmailServer.Enums;
using GmailServer.MomoAccounts;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.MomoAccounts
{
    [Authorize(GmailServerPermissions.MomoAccounts.Default)]
    public class IndexModel : GmailServerPageModel
    {
        private readonly IMomoAccountAppService _appService;

        public IndexModel(IMomoAccountAppService appService)
        {
            _appService = appService;
        }

        public async void OnGet()
        {
            var uploadGroupSelections = await _appService.GetUploadGroupSelectionAsync();
            var momoAccountStatusSelections = Enum.GetValues(typeof(MomoAccountStatus)).Cast<MomoAccountStatus>()
              .Select(item => new SelectListItem()
              {
                  Text = item.ToString(),
                  Value = $"{(int)item}"
              }).ToList();
            ViewData.Add("momoAccountStatusSelections", SerializeObject(momoAccountStatusSelections));
            ViewData.Add("uploadGroupSelections", SerializeObject(uploadGroupSelections));
        }
    }
}
