using GmailServer.ControllerInterfaces;
using GmailServer.DownloadedApps;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/downloadedApps")]
    public class DownloadedAppController : AbpController, IDownloadedAppController
    {
        private readonly IDownloadedAppAppService downloadedAppAppService;

        public DownloadedAppController(IDownloadedAppAppService downloadedAppAppService)
        {
            this.downloadedAppAppService = downloadedAppAppService;
        }

        [HttpPost]
        [Route("create")]
        public Task<DownloadedAppGetOutputDto> CreateAsync(CreateDownloadedAppDto input)
        {
            return this.downloadedAppAppService.CreateAsync(input);
        }

        [HttpGet]
        public Task<PagedResultDto<DownloadedAppGetListOutputDto>> GetListAsync(DownloadAppFilterDto input)
        {
            return this.downloadedAppAppService.GetListAsync(input);
        }
    }
}
