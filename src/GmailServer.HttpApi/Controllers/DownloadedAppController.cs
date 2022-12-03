using GmailServer.ControllerInterfaces;
using GmailServer.DownloadedApps;
using Microsoft.AspNetCore.Mvc;
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
        [IgnoreAntiforgeryToken]
        public Task<DownloadedAppGetOutputDto> CreateAsync(CreateDownloadedAppDto input)
        {
            return this.downloadedAppAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return this.downloadedAppAppService.DeleteAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<DownloadedAppGetListOutputDto>> GetListAsync(DownloadAppFilterDto input)
        {
            return this.downloadedAppAppService.GetListAsync(input);
        }
    }
}
