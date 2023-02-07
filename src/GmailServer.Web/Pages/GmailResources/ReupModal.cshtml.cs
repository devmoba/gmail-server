using GmailServer.GmailResources;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.ReupEmail)]
    public class ReupModalModel : GmailServerPageModel
    {
        [BindProperty]
        public ReupFormModel ReupForm { get; set; }

        private readonly IGmailResourceAppService gmailResourceAppService;

        public ReupModalModel(IGmailResourceAppService gmailResourceAppService)
        {
            this.gmailResourceAppService = gmailResourceAppService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<ReupFormModel, ReupGmailResourceInputDto>(ReupForm);
            await this.gmailResourceAppService.ReupAsync(input);
            return NoContent(); 
        }
    }


    public class ReupFormModel
    {
        [Required]
        [TextArea(Rows = 35)]
        [Placeholder("email|password")]
        public string Emails { get; set; }
    }
}
