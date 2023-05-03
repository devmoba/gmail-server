using GmailServer.AppleIdNones;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace GmailServer.Web.Pages.AppleIdNones
{
    [Authorize(GmailServerPermissions.AppleIdNones.Create)]
    public class CreateModalModel : GmailServerPageModel
    {
        [BindProperty]
        public AppleIdNoneViewModel AppleIdNone { get; set; }

        private readonly IAppleIdNoneAppService _appService;

        public CreateModalModel(IAppleIdNoneAppService appService)
        {
            _appService = appService;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<AppleIdNoneViewModel, CreateManyAppleIdNoneInputDto>(AppleIdNone);
            await _appService.CreateManyAsync(input);
            return NoContent();
        }
    }

    public class AppleIdNoneViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [TextArea(Rows = 35)]
        [Placeholder("email|password|ccv(optional)")]
        public string Emails { get; set; }
    }
}
