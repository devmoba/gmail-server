using GmailServer.CheckerReports;
using GmailServer.ControllerInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/checkerReports")]
    public class CheckerReportController : AbpController, ICheckerReportController
    {
        private readonly ICheckerReportAppService checkerReportAppService;

        public CheckerReportController(ICheckerReportAppService checkerReportAppService)
        {
            this.checkerReportAppService = checkerReportAppService; 
        }

        [Route("report")]
        [HttpPost]
        public Task<ReportResponseDto> ReportAsync(ReportRequestDto input)
        {
            return this.checkerReportAppService.ReportAsync(input);
        }
    }
}
