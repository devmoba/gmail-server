using GmailServer.AppleIds;
using GmailServer.Web.Pages.AppleIds.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.AppleIds
{
    public class CreateModalModel : GmailServerPageModel
    {
        [BindProperty]
        public AppleIdViewModel AppleId { get; set; }

        private readonly IAppleIdAppService appleIdAppService;

        public CreateModalModel(IAppleIdAppService appleIdAppService)
        {
            this.appleIdAppService = appleIdAppService;
        }
        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<AppleIdViewModel, CreateManyAppleIdInputDto>(AppleId);
            await this.appleIdAppService.CreateManyAsync(input);
            return NoContent();
        }
    }
}
