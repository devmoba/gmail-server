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
        public void OnGet()
        {
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
            ViewData.Add("removePaymentStatusSelections", SerializeObject(removePaymentStatusSelections));
            ViewData.Add("isRoleNameAppleIdMember", SerializeObject(isRoleNameAppleIdMember));
        }
    }
}
