using GmailServer.Permissions;
using GmailServer.RecoveryEmails;
using GmailServer.Web.Pages.RecoveryEmails.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.RecoveryEmails
{
    [Authorize(GmailServerPermissions.RecoveryEmails.Create)]
    public class CreateModalModel : GmailServerPageModel
    {
        [BindProperty]
        public RecoveryEmailViewModel RecoveryEmail { get; set; }

        private readonly IRecoveryEmailAppService recoveryEmailAppService;

        public CreateModalModel(IRecoveryEmailAppService recoveryEmailAppService)
        {
            this.recoveryEmailAppService = recoveryEmailAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<RecoveryEmailViewModel, CreateManyRecoveryEmailInputDto>(RecoveryEmail);
            await this.recoveryEmailAppService.CreateManyAsync(input);
            return NoContent();
        }
    }
}
