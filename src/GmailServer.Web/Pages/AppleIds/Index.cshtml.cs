using GmailServer.AppleIds;
using GmailServer.Enums;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.AppleIds
{
    [Authorize(GmailServerPermissions.AppleIds.Default)]
    public class IndexModel : GmailServerPageModel
    {
        private readonly IAppleIdAppService _appleIdAppService;

        public bool IsRoleNameAppleIdMember { get; private set; }

        public IndexModel(IAppleIdAppService appleIdAppService)
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

            var appleIdStatusSelections = Enum.GetValues(typeof(AppleIdStatus)).Cast<AppleIdStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();

            var isRoleNameAppleIdMember = CurrentUser.IsInRole(RoleName.RoleNameAppleIdMember);
            IsRoleNameAppleIdMember = isRoleNameAppleIdMember;
            ViewData.Add("appleIdStatusSelections", SerializeObject(appleIdStatusSelections));
            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
            ViewData.Add("isRoleNameAppleIdMember", SerializeObject(isRoleNameAppleIdMember));
        }
    }
}
