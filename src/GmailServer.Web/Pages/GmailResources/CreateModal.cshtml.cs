using GmailServer.GmailResources;
using GmailServer.Permissions;
using GmailServer.Web.Pages.GmailResources.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.Create)]
    public class CreateModalModel : GmailServerPageModel
    {
        [BindProperty]
        public GmailResourceViewModel GmailResource { get; set; }

        private readonly IGmailResourceAppService gmailPremiumAppService;
        public CreateModalModel(IGmailResourceAppService gmailPremiumAppService)
        {
            this.gmailPremiumAppService = gmailPremiumAppService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<GmailResourceViewModel, CreateManyGmailResourceInputDto>(GmailResource);
            await this.gmailPremiumAppService.CreateManyAsync(input);
            return NoContent();
        }
    }
}
