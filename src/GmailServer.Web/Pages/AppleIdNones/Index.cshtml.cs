using GmailServer.AppleIdNones;
using GmailServer.Enums;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.AppleIdNones
{
    [Authorize(GmailServerPermissions.AppleIdNones.Default)]
    public class IndexModel : GmailServerPageModel
    {
        private readonly IAppleIdNoneAppService _appService;

        public IndexModel(IAppleIdNoneAppService appService)
        {
            _appService = appService;
        }

        public async void OnGet()
        {
            var usernameSelections = await _appService.GetUsernameSelectionAsync();
            var appleIdNoneStatusSelections = Enum.GetValues(typeof(AppleIdNoneStatus)).Cast<AppleIdNoneStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();
            var removePaymentStatusSelections = Enum.GetValues(typeof(RemovePaymentStatus)).Cast<RemovePaymentStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();
            var isRoleNameAppleIdMember = CurrentUser.IsInRole(RoleName.RoleNameAppleIdMember);
            ViewData.Add("appleIdNoneStatusSelections", SerializeObject(appleIdNoneStatusSelections));
            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
            ViewData.Add("removePaymentStatusSelections", SerializeObject(removePaymentStatusSelections));
            ViewData.Add("isRoleNameAppleIdMember", SerializeObject(isRoleNameAppleIdMember));
        }
    }
}
