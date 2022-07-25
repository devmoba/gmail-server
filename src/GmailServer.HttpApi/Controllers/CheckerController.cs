using GmailServer.Checkers;
using GmailServer.ControllerInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/checkers")]
    public class CheckerController : AbpController, ICheckerController
    {
        private readonly ICheckerAppService checkerAppService;

        public CheckerController(ICheckerAppService checkerAppService)
        {
            this.checkerAppService = checkerAppService;
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return this.checkerAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<CheckerDto> GetAsync(long id)
        {
            return await this.checkerAppService.GetAsync(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<CheckerDto>> GetListAsync(CheckerFilterDto input)
        {
            return await this.checkerAppService.GetListAsync(input);
        }
    }
}
