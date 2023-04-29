using GmailServer.ControllerInterfaces;
using GmailServer.MomoAccounts;
using GmailServer.MomoAccounts.Statistics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/momoAccounts")]
    public class MomoAccountController : AbpController, IMomoAccountController
    {
        private readonly IMomoAccountAppService _appService;
        public MomoAccountController(IMomoAccountAppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        [Route("create")]
        [IgnoreAntiforgeryToken]
        public Task<MomoAccountDto> CreateAsync(CreateMomoAccountInputDto input)
        {
            return _appService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("deleteAll")]
        public Task DeleteAllAsync()
        {
            return _appService.DeleteAllAsync();
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return _appService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<MomoAccountDto> GetAsync(long id)
        {
            return _appService.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<MomoAccountDto>> GetListAsync(MomoAccountFilterDto input)
        {
            return _appService.GetListAsync(input);
        }

        [HttpGet]
        [Route("getMomoAccount")]
        [IgnoreAntiforgeryToken]
        public Task<MomoAccountDto> GetMomoAcountAsync()
        {
            return _appService.GetMomoAcountAsync();
        }

        [HttpGet]
        [Route("getMomoAccountStatusSelection")]
        public Task<List<MomoAccountStatusSelectionDto>> GetMomoAcountStatusSelectionsAsync(string uploadGroup, DateTime? createdFrom, DateTime? createdTo)
        {
            return _appService.GetMomoAcountStatusSelectionsAsync(uploadGroup, createdFrom, createdTo);
        }

        [HttpGet]
        [Route("getStatistic")]
        public Task<PagedResultDto<MomoAccountStatisticDto>> GetStatisticAsync(MomoAccountStatisticFilterDto input)
        {
            return _appService.GetStatisticAsync(input);    
        }

        [HttpGet]
        [Route("increaseLinkCount")]
        public Task<MomoAccountDto> IncreaseLinkCountAsync(string username)
        {
            return _appService.IncreaseLinkCountAsync(username);
        }

        [HttpPut]
        [Route("update/{username}")]
        [IgnoreAntiforgeryToken]
        public Task<MomoAccountDto> UpdateMomoAcountAsync(string username, UpdateMomoAccountInputDto input)
        {
            return _appService.UpdateMomoAcountAsync(username, input);
        }
    }
}
