using GmailServer.AppleOrders;
using GmailServer.AppleOrders.Statistics;
using GmailServer.ControllerInterfaces;
using GmailServer.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/appleOrders")]
    public class AppleOrderController : AbpController, IAppleOrderController
    {
        private readonly IAppleOrderAppService _appService;
        public AppleOrderController(IAppleOrderAppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        [Route("create")]
        [IgnoreAntiforgeryToken]
        public Task<AppleOrderDto> CreateAsync([Required]string orderId, [Required]string urlPayment)
        {
            return _appService.CreateAsync(orderId, urlPayment);
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(long id)
        {
            return _appService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<AppleOrderDto> GetAsync(long id)
        {
            return _appService.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<AppleOrderDto>> GetListAsync(AppleOrderFilterDto input)
        {
            return _appService.GetListAsync(input);
        }

        [HttpGet]
        [Route("getOrderCountByStatus")]
        public Task<int> GetOrderCountByStatusAsync([Required]string linkStatus, [Required] string addPaymentStatus)
        {
            var linkStatusArr = linkStatus.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Enum.Parse<LinkStatus>(x))
                .ToArray();
            var addPaymentStatusArr = addPaymentStatus.Split(',', StringSplitOptions.RemoveEmptyEntries)
               .Select(x => Enum.Parse<AddPaymentStatus>(x))
               .ToArray();
            return _appService.GetOrderCountByStatusAsync(linkStatusArr, addPaymentStatusArr);
        }

        [HttpGet]
        [Route("getPendingOrderCountByMomoAccount")]
        [IgnoreAntiforgeryToken]
        public Task<int> GetPendingOrderCountByMomoAccountAsync([Required] string momoAccount)
        {
            return _appService.GetPendingOrderCountByMomoAccountAsync(momoAccount);
        }

        [HttpGet]
        [Route("getStatisticByAddPaymentStatus")]
        public Task<PagedResultDto<AppleOrderStatisticByAddPaymentStatusDto>> GetStatisticByAddPaymentStatusAsync(AppleOrderStatisticFilterDto input)
        {
            return _appService.GetStatisticByAddPaymentStatusAsync(input);
        }

        [HttpGet]
        [Route("getStatisticByLinkStatus")]
        public Task<PagedResultDto<AppleOrderStatisticByLinkStatusDto>> GetStatisticByLinkStatusAsync(AppleOrderStatisticFilterDto input)
        {
            return _appService.GetStatisticByLinkStatusAsync(input);
        }

        [HttpGet]
        [Route("takeOrderToAddPayment")]
        [IgnoreAntiforgeryToken]
        public Task<AppleOrderDto> TakeOrderToAddPaymentAsync()
        {
            return _appService.TakeOrderToAddPaymentAsync();
        }

        [HttpGet]
        [Route("takeOrderToLink")]
        [IgnoreAntiforgeryToken]
        public Task<AppleOrderDto> TakeOrderToLinkAsync()
        {
            return _appService.TakeOrderToLinkAsync();
        }

        [HttpPut]
        [Route("updateAddPaymentStatus")]
        [IgnoreAntiforgeryToken]
        public Task<AppleOrderDto> UpdateAddPaymentStatusAsync(string orderId, AddPaymentStatus status, string appleId)
        {
            return _appService.UpdateAddPaymentStatusAsync(orderId, status, appleId);
        }

        [HttpPut]
        [Route("updateLinkStatus")]
        [IgnoreAntiforgeryToken]
        public Task<AppleOrderDto> UpdateLinkStatusAsync(string orderId, LinkStatus status, string momoAccount = null)
        {
            return _appService.UpdateLinkStatusAsync(orderId, status, momoAccount);
        }
    }
}
