using GmailServer.RecoveryEmails;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages
{
    [Authorize]
    public class IndexModel : GmailServerPageModel
    {
        //private readonly IRecoveryEmailAppService recoveryEmailAppService;

        //public RecoveryEmailReportStatusDto RecoveryEmailReportStatus { get; set; }

        //public IndexModel(IRecoveryEmailAppService recoveryEmailAppService)
        //{
        //    this.recoveryEmailAppService = recoveryEmailAppService;
        //}

        //public async Task OnGetAsync()
        //{
        //    RecoveryEmailReportStatus = await recoveryEmailAppService.GetRecoveryEmailReportAsync();
        //}
    }
}