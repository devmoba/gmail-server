using GmailServer.ControllerInterfaces;
using GmailServer.RecoveryEmails;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/recoveryEmails")]
    public class RecoveryEmailController : AbpController, IRecoveryEmailController
    {
        private readonly IRecoveryEmailAppService _recoveryEmailAppService;

        public RecoveryEmailController(IRecoveryEmailAppService recoveryEmailAppService)
        {
            _recoveryEmailAppService = recoveryEmailAppService;
        }

        [HttpPost]
        [Route("create")]
        public Task<RecoveryEmailDto> CreateAsync(CreateUpdateRecoveryEmailDto input)
        {
            return _recoveryEmailAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("deleteAll")]
        public Task DeleteAllAsync()
        {
            return _recoveryEmailAppService.DeleteAllAsync();
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return _recoveryEmailAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("getFirst")]
        public Task<RecoveryEmailDto> GetFirstRecoveryEmailAsync()
        {
            return _recoveryEmailAppService.GetFirstRecoveryEmailAsync();
        }

        [HttpGet]
        public Task<PagedResultDto<RecoveryEmailDto>> GetListAsync(RecoveryEmailFilterDto input)
        {
            return _recoveryEmailAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("getRandom")]
        public Task<RecoveryEmailDto> GetRecoveryEmailRandomAsync()
        {
            return _recoveryEmailAppService.GetRecoveryEmailRandomAsync();
        }
    }
}
