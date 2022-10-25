using GmailServer.AppleIds;
using GmailServer.AppleIds.Statistics;
using GmailServer.ControllerInterfaces;
using GmailServer.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [HttpGet]
        [Route("upload")]
        public Task<AppleIdDto> CreateAsync([Required] string email, [Required] string password, [Required] string username)
        {
            return _appleIdAppService.CreateAsync(new CreateUpdateAppleIdDto()
            {
                Email = email,
                Password = password,
                Username = username
            });
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
        [Route("getAppleIdStatusSelection")]
        public Task<List<AppleIdStatusSelectionDto>> GetAppleIdStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo)
        {
            return _appleIdAppService.GetAppleIdStatusSelectionAsync(username, createdFrom, createdTo);
        }

        [HttpGet]
        [Route("getByStatus")]
        public Task<AppleIdDto> GetByStatusAsync(AppleIdStatus status)
        {
            return _appleIdAppService.GetByStatusAsync(status);
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

        [HttpGet]
        [Route("getStatistic")]
        public Task<PagedResultDto<AppleIdStatisticDto>> GetStatisticAsync(AppleStatisticFilterDto input)
        {
            return _appleIdAppService.GetStatisticAsync(input);
        }

        [HttpGet]
        [Route("statisticByUsername")]
        public Task<StatisticByUsernameDto> GetStatisticByUsernameAsync()
        {
            return _appleIdAppService.GetStatisticByUsernameAsync();
        }

        [HttpGet]
        [Route("getStatisticDaily")]
        public Task<PagedResultDto<AppleStatisticDailyDto>> GetStatisticDailyAsync(AppleIdStatisticDailyFilterDto input)
        {
            return _appleIdAppService.GetStatisticDailyAsync(input);
        }

        [HttpPut]
        [Route("updateStatus")]
        public Task<AppleIdDto> UpdateStatusAsync([Required] string email, [Required] AppleIdStatus status)
        {
            return _appleIdAppService.UpdateStatusAsync(email, status);
        }
    }
}
