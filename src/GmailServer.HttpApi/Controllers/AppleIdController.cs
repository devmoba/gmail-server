using GmailServer.AppleIds;
using GmailServer.ControllerInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/appleIds")]
    public class AppleIdController : AbpController, IAppleIdController
    {
        private readonly IAppleIdAppService _appleIdAppService;
        public AppleIdController(IAppleIdAppService appleIdAppService)
        {
            _appleIdAppService = appleIdAppService;
        }

        [HttpDelete]
        [Route("deleteAll")]
        public Task DeleteAllAsync()
        {
            return _appleIdAppService.DeleteAllAsync();
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return _appleIdAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("getFirst")]
        public Task<AppleIdDto> GetFirstAppleIdAsync()
        {
            return _appleIdAppService.GetFirstAppleIdAsync();
        }

        [HttpGet]
        public Task<PagedResultDto<AppleIdDto>> GetListAsync(AppleIdFilterDto input)
        {
            return _appleIdAppService.GetListAsync(input);
        }
    }
}
