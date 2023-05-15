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
        public bool IsRoleNameAppleIdMember { get; private set; }

        public void OnGet()
        {
            var appleIdStatusSelections = Enum.GetValues(typeof(AppleIdStatus)).Cast<AppleIdStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();

            var isRoleNameAppleIdMember = CurrentUser.IsInRole(RoleName.RoleNameAppleIdMember);
            IsRoleNameAppleIdMember = isRoleNameAppleIdMember;
            ViewData.Add("appleIdStatusSelections", SerializeObject(appleIdStatusSelections));
            ViewData.Add("isRoleNameAppleIdMember", SerializeObject(isRoleNameAppleIdMember));
        }
    }
}
