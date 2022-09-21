using GmailServer.GmailPremiums;
using GmailServer.Web.Pages.GmailPremiums.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.GmailPremiums
{
    public class CreateModalModel : GmailServerPageModel
    {
        [BindProperty]
        public GmailPremiumViewModel GmailPremium { get; set; }

        private readonly IGmailPremiumAppService gmailPremiumAppService;
        public CreateModalModel(IGmailPremiumAppService gmailPremiumAppService)
        {
            this.gmailPremiumAppService = gmailPremiumAppService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<GmailPremiumViewModel, CreateManyGmailPremiumInputDto>(GmailPremium);
            await this.gmailPremiumAppService.CreateManyAsync(input);
            return NoContent();
        }
    }
}
