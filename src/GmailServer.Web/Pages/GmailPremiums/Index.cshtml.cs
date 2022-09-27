using GmailServer.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.GmailPremiums
{
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
            var gmailPremiumStatusSelections = Enum.GetValues(typeof(GmailPremiumStatus)).Cast<GmailPremiumStatus>()
              .Select(item => new SelectListItem()
              {
                  Text = item.ToString(),
                  Value = $"{(int)item}"
              });

            ViewData.Add("gmailPremiumStatusSelections", SerializeObject(gmailPremiumStatusSelections));
        }
    }
}
