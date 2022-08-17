using GmailServer.RecoveryEmails;
using GmailServer.Web.Pages.RecoveryEmails.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.RecoveryEmails
{
    public class EditModalModel : GmailServerPageModel
    {

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty]
        public RecoveryEmailViewModel RecoveryEmail { get; set; }

        private readonly IRecoveryEmailAppService recoveryEmailAppService;

        public EditModalModel(IRecoveryEmailAppService recoveryEmailAppService)
        {
            this.recoveryEmailAppService = recoveryEmailAppService;
        }

        public async void OnGet()
        {
            var recoveryEmailDto = await this.recoveryEmailAppService.GetAsync(Id);
            RecoveryEmail = ObjectMapper.Map<RecoveryEmailDto, RecoveryEmailViewModel>(recoveryEmailDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var recoveryEmail = ObjectMapper.Map<RecoveryEmailViewModel, CreateUpdateRecoveryEmailDto>(RecoveryEmail);
            await this.recoveryEmailAppService.UpdateAsync(Id, recoveryEmail);
            return NoContent(); 
        }
    }
}
