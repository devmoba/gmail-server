using GmailServer.ControllerInterfaces;
using GmailServer.Enums;
using GmailServer.GmailResources;
using GmailServer.GmailResources.Statistics;
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
    [Route("/api/gmailResources")]
    public class GmailResourceController : AbpController, IGmailResourceController
    {
        private readonly IGmailResourceAppService _gmailResourceAppService;

        public GmailResourceController(IGmailResourceAppService gmailResourceAppService)
        {
            _gmailResourceAppService = gmailResourceAppService;
        }

        [HttpPost]
        [Route("create")]
        public Task<GmailResourceDto> CreateAsync(CreateUpdateGmailResourceDto input)
        {
            return _gmailResourceAppService.CreateAsync(input);
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
        [Route("getByStatus")]
        public Task<GmailResourceDto> GetByStatusAsync(GmailResourceStatus status)
        {
            return _gmailResourceAppService.GetByStatusAsync(status);
        }

        [HttpGet]
        [Route("getFirst")]
        public Task<GmailResourceDto> GetFirstGmailPremiumAsync()
        {
            return _gmailResourceAppService.GetFirstGmailResourceAsync();
        }

        [HttpGet]
        [Route("getGmailResourceStatusSelection")]
        public Task<List<GmailResourceStatusSelectionDto>> GetGmailResourceStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo, int? updatedHours = null)
        {
            return _gmailResourceAppService.GetGmailResourceStatusSelectionAsync(username, createdFrom, createdTo, updatedHours);
        }

        [HttpGet]
        public Task<PagedResultDto<GmailResourceDto>> GetListAsync(GmailResourceFilterDto input)
        {
            return _gmailResourceAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("getStatistic")]
        public Task<PagedResultDto<GmailResourceStatisticDto>> GetStatisticAsync(GmailResourceStatisticFilterDto input)
        {
            return _gmailResourceAppService.GetStatisticAsync(input);
        }

        [HttpGet]
        [Route("statisticByUsername")]
        public Task<StatisticByUsernameDto> GetStatisticByUsernameAsync()
        {
            return _gmailResourceAppService.GetStatisticByUsernameAsync();
        }

        [HttpGet]
        [Route("getStatisticDaily")]
        public Task<PagedResultDto<GmailResourceStatisticDailyDto>> GetStatisticDailyAsync(GmailResourceStatisticDailyFilterDto input)
        {
            return _gmailResourceAppService.GetStatisticDailyAsync(input);
        }

        [HttpPut]
        [Route("updateStatus")]
        public Task<GmailResourceDto> UpdateStatusAsync([Required] string email, [Required] GmailResourceStatus status)
        {
            return _gmailResourceAppService.UpdateStatusAsync(email, status);
        }
    }
}
