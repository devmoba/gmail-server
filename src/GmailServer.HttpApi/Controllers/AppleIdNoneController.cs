using GmailServer.AppleIdNones;
using GmailServer.AppleIdNones.Statistics;
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
    [Route("/api/appleIdNones")]
    public class AppleIdNoneController : AbpController, IAppleIdNoneController
    {
        private readonly IAppleIdNoneAppService _appService;

        public AppleIdNoneController(IAppleIdNoneAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public Task<PagedResultDto<AppleIdNoneGetListOutputDto>> GetListAsync(AppleIdNoneFilterDto input)
        {
            return _appService.GetListAsync(input);
        }

        [HttpGet]
        [Route("getAppleIdNoneStatusSelection")]
        public Task<List<AppleIdNoneStatusSelectionDto>> GetAppleIdNoneStatusSelectionsAsync(string username, DateTime? createdFrom, DateTime? createdTo)
        {
            return _appService.GetAppleIdNoneStatusSelectionsAsync(username, createdFrom, createdTo);
        }

        [HttpGet]
        [Route("getAppleIdToRemove")]
        [IgnoreAntiforgeryToken]
        public Task<AppleIdNoneGetOutputDto> GetAppleIdToRemoveAsync()
        {
            return _appService.GetAppleIdToRemoveAsync();
        }

        [HttpGet]
        [Route("getByStatus")]
        [IgnoreAntiforgeryToken]
        public Task<AppleIdNoneGetOutputDto> GetByStatusAsync(AppleIdNoneStatus status)
        {
            return _appService.GetByStatusAsync(status);
        }

        [HttpGet]
        [Route("getFirst")]
        [IgnoreAntiforgeryToken]
        public Task<AppleIdNoneGetOutputDto> GetFirstAppleIdNoneAsync(bool isNone = false)
        {
            return _appService.GetFirstAppleIdNoneAsync(isNone);
        }

        [HttpGet]
        [Route("getStatistic")]
        public Task<PagedResultDto<AppleIdNoneStatisticDto>> GetStatisticAsync(AppleIdNoneStatisticFilterDto input)
        {
            return _appService.GetStatisticAsync(input);
        }

        [HttpPut]
        [Route("increasePurchase")]
        [IgnoreAntiforgeryToken]
        public Task<AppleIdNoneGetOutputDto> IncreasePurchaseAsync(string email)
        {
            return _appService.IncreasePurchaseAsync(email);
        }

        [HttpPut]
        [Route("setTakenOutNumber")]
        [IgnoreAntiforgeryToken]
        public Task<AppleIdNoneGetOutputDto> SetTakenOutNumberAsync(string email, int value)
        {
            return _appService.SetTakenOutNumberAsync(email, value);
        }

        [HttpPut]
        [Route("updateRemoveStatus")]
        [IgnoreAntiforgeryToken]
        public Task<AppleIdNoneGetOutputDto> UpdateRemoveStatusAsync(string email, RemovePaymentStatus status)
        {
            return _appService.UpdateRemoveStatusAsync(email, status);
        }

        [HttpPut]
        [Route("updateStatus")]
        [IgnoreAntiforgeryToken]
        public Task<AppleIdNoneGetOutputDto> UpdateStatusAsync(string email, AppleIdNoneStatus status)
        {
            return _appService.UpdateStatusAsync(email, status);
        }

        [HttpPut]
        [Route("addPaymentCompleted")]
        [IgnoreAntiforgeryToken]
        public Task<AppleIdNoneGetOutputDto> AddPaymentCompletedAsync(string email)
        {
            return _appService.AddPaymentCompletedAsync(email);
        }

        [HttpGet]
        [Route("upload")]
        public Task<AppleIdNoneGetOutputDto> CreateAsync([Required]string email, [Required] string password, [Required] string username, string ccv = null, 
            string secretAnswer1 = null, string secretAnswer2 = null, string secretAnswer3 = null, string dob = null)
        {
            return _appService.CreateAsync(new CreateUpdateAppleIdNoneDto()
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
            return _appService.DeleteAllAsync();
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return _appService.DeleteAsync(id);
        }

    }
}
