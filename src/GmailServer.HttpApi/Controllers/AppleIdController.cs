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
        public Task<AppleIdGetOutputDto> CreateAsync([Required] string email, [Required] string password, [Required] string username, 
            string ccv = default, string secretAnswer1 = default, string secretAnswer2 = default, string secretAnswer3 = default, string dob = default)
        {
            return _appleIdAppService.CreateAsync(new CreateUpdateAppleIdDto()
            {
                Email = email,
                Password = password,
                Username = username,
                Ccv = ccv,
                SecretAnswer1 = secretAnswer1,
                SecretAnswer2 = secretAnswer2,
                SecretAnswer3 = secretAnswer3,
                DateOfBirth = dob
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
        public Task<List<AppleIdStatusSelectionDto>> GetAppleIdStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo, int? updatedHours = null)
        {
            return _appleIdAppService.GetAppleIdStatusSelectionAsync(username, createdFrom, createdTo, updatedHours);
        }

        [HttpGet]
        [Route("getByStatus")]
        public Task<AppleIdGetOutputDto> GetByStatusAsync(AppleIdStatus status)
        {
            return _appleIdAppService.GetByStatusAsync(status);
        }

        [HttpGet]
        [Route("getFirst")]
        public Task<AppleIdGetOutputDto> GetFirstAppleIdAsync()
        {
            return _appleIdAppService.GetFirstAppleIdAsync();
        }

        [HttpPut]
        [Route("setTakenOutNumber")]
        public Task<AppleIdGetOutputDto> SetTakenOutNumberAsync([Required] string email, [Required] int value)
        {
            return _appleIdAppService.SetTakenOutNumberAsync(email, value);
        }

        [HttpPut]
        [Route("increasePurchase")]
        public Task<AppleIdGetOutputDto> IncreasePurchaseAsync(string email)
        {
            return _appleIdAppService.IncreasePurchaseAsync(email);
        }

        [HttpPut]
        [Route("updateStatus")]
        public Task<AppleIdGetOutputDto> UpdateStatusAsync([Required] string email, [Required] AppleIdStatus status)
        {
            return _appleIdAppService.UpdateStatusAsync(email, status);
        }

        [HttpGet]
        public Task<PagedResultDto<AppleIdGetListOutputDto>> GetListAsync(AppleIdFilterDto input)
        {
            return _appleIdAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("getStatistic")]
        public Task<PagedResultDto<AppleIdStatisticDto>> GetStatisticAsync(AppleIdStatisticFilterDto input)
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
        public Task<PagedResultDto<AppleIdStatisticDailyDto>> GetStatisticDailyAsync(AppleIdStatisticDailyFilterDto input)
        {
            return _appleIdAppService.GetStatisticDailyAsync(input);
        }

        [HttpGet]
        [Route("getUsernameSelection")]
        public Task<List<UsernameSelectionDto>> GetUsernameSelectionAsync()
        {
            return _appleIdAppService.GetUsernameSelectionAsync();
        }
    }
}
