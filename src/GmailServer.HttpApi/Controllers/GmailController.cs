using GmailServer.ControllerInterfaces;
using GmailServer.Gmails;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/gmails")]
    public class GmailController : AbpController, IGmailController
    {
        private readonly IGmailAppService _appService;
        public GmailController(IGmailAppService gmailAppService)
        {
            _appService = gmailAppService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<GmailDto> CreateAsync(CreateGmailDto input)
        {
            return await _appService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(long id)
        {
            await _appService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("reports")]
        public async Task<PagedResultDto<GmailReportDto>> GetGmailReportsAsync(GmailReportFilterDto input)
        {
            return await _appService.GetGmailReportsAsync(input);
        }

        [HttpGet]
        [Route("getGmailStatusSelection")]
        public Task<List<GmailStatusSelectionDto>> GetGmailStatusSelectionAsync(DateTime? createdFrom, DateTime? createdTo)
        {
            return _appService.GetGmailStatusSelectionAsync(createdFrom, createdTo);
        }

        [HttpGet]
        public async Task<PagedResultDto<GmailDto>> GetListAsync(GmailFilterDto input)
        {
            return await _appService.GetListAsync(input);
        }

        [HttpGet]
        [Route("reportByStatus")]
        public async Task<ReportbyStatusDto> GetReportbyStatusAsync()
        {
            return await _appService.GetReportbyStatusAsync();
        }
    }
}
