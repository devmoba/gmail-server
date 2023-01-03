using GmailServer.Enums;
using GmailServer.GmailResources;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.Default)]
    public class IndexModel : GmailServerPageModel
    {
        private readonly IGmailResourceAppService _gmailResourceAppService;

        public bool IsRoleNameAppleIdMember { get; private set; }

        public IndexModel(IGmailResourceAppService gmailResourceAppService)
        {
            _gmailResourceAppService = gmailResourceAppService;
        }

        public async void OnGet()
        {
            var gmailResourceStatusSelections = Enum.GetValues(typeof(GmailResourceStatus)).Cast<GmailResourceStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();

            var usernames = await _gmailResourceAppService.GetUsernameSelectionAsync();
            var usernameSelections = usernames.Select(item => new SelectListItem()
            {
                Text = item,
                Value = item
            }).ToList();

            ViewData.Add("gmailResourceStatusSelections", SerializeObject(gmailResourceStatusSelections));
            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
            var isRoleNameAppleIdMember = CurrentUser.IsInRole(RoleName.RoleNameAppleIdMember);
            IsRoleNameAppleIdMember = isRoleNameAppleIdMember;
            ViewData.Add("isRoleNameAppleIdMember", SerializeObject(isRoleNameAppleIdMember));
        }
    }
}
