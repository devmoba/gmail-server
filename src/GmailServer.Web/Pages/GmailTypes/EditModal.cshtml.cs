using GmailServer.GmailTypes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.GmailTypes
{
    public class EditModalModel : GmailServerPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty]
        public CreateUpdateGmailTypeDto GmailType { get; set; }

        private readonly IGmailTypeAppService gmailTypeAppService;

        public EditModalModel(IGmailTypeAppService gmailTypeAppService)
        {
            this.gmailTypeAppService = gmailTypeAppService;
        }

        public async void OnGet()
        {
            var gmailTypeDto = await this.gmailTypeAppService.GetAsync(Id);
            GmailType = ObjectMapper.Map<GmailTypeDto, CreateUpdateGmailTypeDto>(gmailTypeDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await this.gmailTypeAppService.UpdateAsync(Id, GmailType);
            return NoContent();
        }
    }
}
