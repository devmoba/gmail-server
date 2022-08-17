using GmailServer.RecoveryEmails;
using GmailServer.Web.Pages.RecoveryEmails.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.RecoveryEmails
{
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
            var recoveryEmail = ObjectMapper.Map<RecoveryEmailViewModel, CreateUpdateRecoveryEmailDto>(RecoveryEmail);
            await this.recoveryEmailAppService.CreateAsync(recoveryEmail);
            return NoContent();
        }
    }
}
