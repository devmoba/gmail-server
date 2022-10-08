using GmailServer.GmailTypes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.GmailTypes
{
    public class CreateModalModel : GmailServerPageModel
    {
        private readonly IGmailTypeAppService gmailTypeAppService;
        
        [BindProperty]
        public CreateUpdateGmailTypeDto GmailType { get; set; }

        public CreateModalModel(IGmailTypeAppService gmailTypeAppService)
        {
            this.gmailTypeAppService = gmailTypeAppService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await this.gmailTypeAppService.CreateAsync(GmailType);
            return NoContent();
        }
    }
}
