using GmailServer.AppleOrders.Statistics;
using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.AppleOrders
{
    public interface IAppleOrderAppService : 
        IReadOnlyAppService<
            AppleOrderDto, 
            long,
            AppleOrderFilterDto>, IDeleteAppService<long>
    {
        Task<int> GetPendingOrderCountByMomoAccountAsync(string momoAccount);

        Task<int> GetOrderCountByStatusAsync(LinkStatus[] linkStatus, AddPaymentStatus[] addPaymentStatus);

        Task<PagedResultDto<AppleOrderStatisticByLinkStatusDto>> GetStatisticByLinkStatusAsync(AppleOrderStatisticFilterDto input);

        Task<PagedResultDto<AppleOrderStatisticByAddPaymentStatusDto>> GetStatisticByAddPaymentStatusAsync(AppleOrderStatisticFilterDto input);

        Task<AppleOrderDto> CreateAsync(string orderId, string urlPayment);

        Task<AppleOrderDto> UpdateLinkStatusAsync(string orderId, LinkStatus status, string momoAccount = default);

        Task<AppleOrderDto> UpdateAddPaymentStatusAsync(string orderId, AddPaymentStatus status, string appleId);

        Task<AppleOrderDto> TakeOrderToLinkAsync();

        Task<AppleOrderDto> TakeOrderToAddPaymentAsync();

        Task<List<AppleOrderLinkStatusSelectionDto>> GetAppleOrderLinkStatusSelectionsAsync(DateTime? createdFrom, DateTime? createdTo);

        //Task ResetLinkStatusAsync(ResetLinkStatusFilterInput input);
    }
}
