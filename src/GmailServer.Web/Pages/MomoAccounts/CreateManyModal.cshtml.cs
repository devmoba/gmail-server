using GmailServer.MomoAccounts;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace GmailServer.Web.Pages.MomoAccounts
{
    [Authorize(GmailServerPermissions.MomoAccounts.CreateMany)]
    public class CreateManyModalModel : GmailServerPageModel
    {
        [BindProperty]
        public CreateManyMomoAccoutModel MomoAccount { get; set; }

        private readonly IMomoAccountAppService _appService;
        public CreateManyModalModel(IMomoAccountAppService appService)
        {
            _appService = appService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<CreateManyMomoAccoutModel, CreateManyMomoAccountInputDto>(MomoAccount);
            await _appService.CreateManyAsync(input);
            return NoContent();
        }
    }

    public class CreateManyMomoAccoutModel 
    {
        [Required]
        [DisplayName("Upload Group")]
        public string UploadGroup { get; set; }

        [Required]
        [TextArea(Rows = 35)]
        [Placeholder("username|password|email(optional)")]
        public string Accounts { get; set; }
    }
}
