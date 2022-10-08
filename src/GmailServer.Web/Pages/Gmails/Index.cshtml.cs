using GmailServer.Enums;
using GmailServer.GmailTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.Gmails
{
    public class IndexModel : GmailServerPageModel
    {
        private readonly IGmailTypeAppService gmailTypeAppService;

        public IndexModel(IGmailTypeAppService gmailTypeAppService)
        {
            this.gmailTypeAppService = gmailTypeAppService;
        }

        public async void OnGet()
        {
            var gmailTypeSelections = await this.gmailTypeAppService.GetAllSelectionAsync();

            var gmailStatusSelections = Enum.GetValues(typeof(Status)).Cast<Status>()
                .Select(item => new SelectListItem()
                {
                    Text = item.ToString(),
                    Value = $"{(int)item}"
                });

            ViewData.Add("gmailStatusSelections", SerializeObject(gmailStatusSelections));
            ViewData.Add("gmailTypeSelections", SerializeObject(gmailTypeSelections.Select(item => new SelectListItem()
            {
                Text = item.Name,
                Value = $"{item.Id}"
            })));
        }
    }
}
