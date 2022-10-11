using GmailServer.RecoveryEmails;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace GmailServer.Web.Pages
{
    [Authorize]
    public class IndexModel : GmailServerPageModel
    {
        private readonly IRecoveryEmailAppService recoveryEmailAppService;

        public int ReadyCount { get; set; } = 0;

        public int CompletedCount { get; set; } = 0;

        public IndexModel(IRecoveryEmailAppService recoveryEmailAppService)
        {
            this.recoveryEmailAppService = recoveryEmailAppService;
        }

        public async void OnGet()
        {
            var recoveryEmailReports = await this.recoveryEmailAppService.GetRecoveryEmailReportAsync();
            if (recoveryEmailReports.Count > 0)
            {
                var ready = recoveryEmailReports.Where(x => x.Status == Enums.RecoveryEmailStatus.Ready).FirstOrDefault();
                ReadyCount = ready != null ? ready.Count : 0;
                var completed = recoveryEmailReports.Where(x => x.Status == Enums.RecoveryEmailStatus.Completed).FirstOrDefault();
                CompletedCount = completed != null ? completed.Count : 0;
            }
        }
    }
}