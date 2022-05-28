using GmailServer.ControllerInterfaces;
using GmailServer.Gmails;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<PagedResultDto<GmailDto>> GetListAsync(GmailFilterDto input)
        {
            return await _appService.GetListAsync(input);
        }
    }
}
