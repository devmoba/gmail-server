using GmailServer.ControllerInterfaces;
using GmailServer.GmailPremiums;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/gmailPremiums")]
    public class GmailPremiumController : AbpController, IGmailPremiumController
    {
        private readonly IGmailPremiumAppService _gmailPremiumAppService;
        public GmailPremiumController(IGmailPremiumAppService gmailPremiumAppService)
        {
            _gmailPremiumAppService = gmailPremiumAppService;
        }

        [HttpPost]
        [Route("create")]
        public Task<GmailPremiumDto> CreateAsync(CreateUpdateGmailPremiumDto input)
        {
           return _gmailPremiumAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("deleteAll")]
        public Task DeleteAllAsync()
        {
            return _gmailPremiumAppService.DeleteAllAsync();
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return _gmailPremiumAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("getFirst")]
        public Task<GmailPremiumDto> GetFirstGmailPremiumAsync()
        {
            return _gmailPremiumAppService.GetFirstGmailPremiumAsync();
        }

        [HttpGet]
        public Task<PagedResultDto<GmailPremiumDto>> GetListAsync(GmailPremiumFilterDto input)
        {
            return _gmailPremiumAppService.GetListAsync(input);
        }
    }
}
