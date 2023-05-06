using GmailServer.OwnerConfigs;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.OwnerConfigs
{
    [Authorize(GmailServerPermissions.OwnerConfigs.Create)]
    public class CreateModalModel : GmailServerPageModel
    {
        [BindProperty]
        public CreateUpdateOwnerConfigDto OwnerConfig { get; set; }

        private readonly IOwnerConfigAppService _appService;

        public CreateModalModel(IOwnerConfigAppService appService)
        {
            _appService = appService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _appService.CreateAsync(OwnerConfig);
            return NoContent();
        }
    }
}
