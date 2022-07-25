using GmailServer.CheckerReports;
using GmailServer.EmailChecks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GmailServer.ControllerInterfaces
{
    public interface ICheckerReportController
    {
        Task<ReportResponseDto> ReportAsync(ReportRequestDto input);

        Task InputEmailCheckAsync(List<EmailCheck> input);
    }
}
