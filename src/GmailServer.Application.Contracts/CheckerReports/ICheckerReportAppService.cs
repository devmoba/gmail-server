using GmailServer.EmailChecks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.CheckerReports
{
    public interface ICheckerReportAppService : IApplicationService
    {
        Task<ReportResponseDto> ReportAsync(ReportRequestDto input);

        Task InputEmailChecksAsync(List<EmailCheck> input);
    }
}
