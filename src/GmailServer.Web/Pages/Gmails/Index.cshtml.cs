using GmailServer.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.Gmails
{
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
            var gmailStatusSelections = Enum.GetValues(typeof(Status)).Cast<Status>()
                .Select(item => new SelectListItem()
                {
                    Text = item.ToString(),
                    Value = $"{(int)item}"
                });

            var genderSelections = Enum.GetValues(typeof(Gender)).Cast<Gender>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               });

            ViewData.Add("gmailStatusSelections", SerializeObject(gmailStatusSelections));
            ViewData.Add("genderSelections", SerializeObject(genderSelections));
        }
    }
}
