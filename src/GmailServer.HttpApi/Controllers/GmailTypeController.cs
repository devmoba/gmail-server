using GmailServer.ControllerInterfaces;
using GmailServer.GmailTypes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/gmailTypes")]
    public class GmailTypeController : AbpController, IGmailTypeController
    {
        private readonly IGmailTypeAppService _gmailTypeAppService;

        public GmailTypeController(IGmailTypeAppService gmailTypeAppService)
        {
            _gmailTypeAppService = gmailTypeAppService;
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return _gmailTypeAppService.DeleteAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<GmailTypeDto>> GetListAsync(GmailTypeFilterDto input)
        {
            return _gmailTypeAppService.GetListAsync(input);
        }
    }
}
