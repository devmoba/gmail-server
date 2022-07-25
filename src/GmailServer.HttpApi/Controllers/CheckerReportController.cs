using GmailServer.CheckerReports;
using GmailServer.ControllerInterfaces;
using GmailServer.EmailChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        private readonly IHttpContextAccessor httpContextAccessor;

        public CheckerReportController(ICheckerReportAppService checkerReportAppService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.checkerReportAppService = checkerReportAppService; 
            this.httpContextAccessor = httpContextAccessor;
        }

        [Route("inputEmailChecks")]
        [HttpPost]
        public Task InputEmailCheckAsync(List<EmailCheck> input)
        {
            return this.checkerReportAppService.InputEmailChecksAsync(input);
        }

        [Route("report")]
        [HttpPost]
        public Task<ReportResponseDto> ReportAsync(ReportRequestDto input)
        {
            input.CheckerIP = this.httpContextAccessor
                .HttpContext
                .Connection
                .RemoteIpAddress
                .MapToIPv4()
                .ToString();
            return this.checkerReportAppService.ReportAsync(input);
        }
    }
}
