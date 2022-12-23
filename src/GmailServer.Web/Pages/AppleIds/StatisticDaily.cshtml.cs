using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GmailServer.Web.Pages.AppleIds
{
    [Authorize(GmailServerPermissions.AppleIds.StatisticDaily)]
    public class StatisticDailyModel : GmailServerPageModel
    {
        [BindProperty(SupportsGet = true)]
        [Required]
        [HiddenInput]
        public string Username { get; set; }

        public void OnGet()
        {
            if (CurrentUser.IsInRole(RoleName.RoleNameAppleIdMember))
            {
                ViewData.Add("usernameParam", SerializeObject(CurrentUser.UserName));
                Username = CurrentUser.UserName;
            }
            else
            {
                if (string.IsNullOrEmpty(Username))
                {
                    Response.Redirect("/AppleIds/Statistic");
                }
                ViewData.Add("usernameParam", SerializeObject(Username));
            }
        }
    }
}
