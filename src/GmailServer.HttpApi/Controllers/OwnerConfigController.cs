using GmailServer.ControllerInterfaces;
using GmailServer.OwnerConfigs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/ownerConfigs")]
    public class OwnerConfigController : AbpController, IOwnerConfigController
    {
        private readonly IOwnerConfigAppService _appService;

        public OwnerConfigController(IOwnerConfigAppService appService)
        {
            _appService = appService;
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return _appService.DeleteAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<OwnerConfigDto>> GetListAsync(OwnerConfigFilterDto input)
        {
            return _appService.GetListAsync(input);
        }
    }
}
