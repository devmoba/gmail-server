using GmailServer.Enums;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.GmailPremiums
{
    [Authorize(GmailServerPermissions.GmailPremiums.Default)]
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
            var gmailPremiumStatusSelections = Enum.GetValues(typeof(GmailPremiumStatus)).Cast<GmailPremiumStatus>()
              .Select(item => new SelectListItem()
              {
                  Text = item.ToString(),
                  Value = $"{(int)item}"
              }).ToList();

            ViewData.Add("gmailPremiumStatusSelections", SerializeObject(gmailPremiumStatusSelections));
        }
    }
}
