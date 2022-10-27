using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.StatisticDaily)]
    public class StatisticDailyModel : GmailServerPageModel
    {
        [BindProperty(SupportsGet = true)]
        [Required]
        public string Username { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(Username))
            {
                Response.Redirect("/GmailResources/Statistic");
            }
            ViewData.Add("usernameParam", SerializeObject(Username));
        }
    }
}
