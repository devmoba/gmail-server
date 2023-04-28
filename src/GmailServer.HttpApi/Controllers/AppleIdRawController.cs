using GmailServer.AppleIdRaws;
using GmailServer.ControllerInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/appleIdRaws")]
    public class AppleIdRawController : AbpController, IAppleIdRawController
    {
        private readonly IAppleIdRawAppService _appService;

        public AppleIdRawController(IAppleIdRawAppService appService) 
        {
            _appService = appService;   
        }

        [HttpPost]
        [Route("upload")]
        [IgnoreAntiforgeryToken]
        public Task<AppleIdRawDto> CreateAsync(CreateAppleIdRawInputDto input)
        {
            return _appService.CreateAsync(input);
        }

        [HttpGet]
        [Route("getAppleIdRawStatistic")]
        public Task<PagedResultDto<AppleIdRawStatisticDailyDto>> GetAppleIdRawStatisticDailyAsync(AppleIdRawStatisticFilterDto input)
        {
            return _appService.GetAppleIdRawStatisticDailyAsync(input);
        }
    }
}
