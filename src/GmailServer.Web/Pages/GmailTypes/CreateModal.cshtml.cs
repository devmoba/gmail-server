using GmailServer.GmailTypes;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.GmailTypes
{
    [Authorize(GmailServerPermissions.GmailTypes.Create)]
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
