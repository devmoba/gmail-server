using GmailServer.ControllerInterfaces;
using GmailServer.Enums;
using GmailServer.GmailResources;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/gmailResources")]
    public class GmailResourceController : AbpController, IGmailResourceController
    {
        private readonly IGmailResourceAppService _gmailResourceAppService;

        public GmailResourceController(IGmailResourceAppService gmailResourceAppService)
        {
            _gmailResourceAppService = gmailResourceAppService;
        }

        [HttpDelete]
        [Route("deleteAll")]
        public Task DeleteAllAsync()
        {
            return _gmailResourceAppService.DeleteAllAsync();
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return _gmailResourceAppService.DeleteAsync(id);
        }


        [HttpGet]
        [Route("getFirst")]
        public Task<GmailResourceDto> GetFirstGmailPremiumAsync()
        {
            return _gmailResourceAppService.GetFirstGmailResourceAsync();
        }

        [HttpGet]
        public Task<PagedResultDto<GmailResourceDto>> GetListAsync(GmailResourceFilterDto input)
        {
            return _gmailResourceAppService.GetListAsync(input);
        }

        [HttpPut]
        [Route("updateStatus")]
        public Task<GmailResourceDto> UpdateStatusAsync([Required] string email, [Required] GmailResourceStatus status)
        {
            return _gmailResourceAppService.UpdateStatusAsync(email, status);
        }
    }
}
