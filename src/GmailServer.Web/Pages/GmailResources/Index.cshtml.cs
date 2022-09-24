using GmailServer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.GmailResources
{
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
            var gmailResourceStatusSelections = Enum.GetValues(typeof(GmailResourceStatus)).Cast<GmailResourceStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               });

            ViewData.Add("gmailResourceStatusSelections", SerializeObject(gmailResourceStatusSelections));
        }
    }
}
