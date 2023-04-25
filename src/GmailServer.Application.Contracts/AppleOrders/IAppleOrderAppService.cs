using GmailServer.AppleOrders.Statistics;
using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.AppleOrders
{
    public interface IAppleOrderAppService : 
        IReadOnlyAppService<
            AppleOrderDto, 
            long,
            AppleOrderFilterDto>, IDeleteAppService<long>
    {
        Task<List<AppleOrderDto>> GetPendingOrderCountByMomoAccountAsync(string momoAccount);

        Task<List<AppleOrderStatisticByLinkStatusDto>> GetStatisticByLinkStatusAsync(AppleOrderStatisticFilterDto input);

        Task<List<AppleOrderStatisticByAddPaymentStatusDto>> GetStatisticByAddPaymentStatusAsync(AppleOrderStatisticFilterDto input);

        Task<AppleOrderDto> CreateAsync(string orderId, string urlPayment);

        Task<AppleOrderDto> UpdateLinkStatusAsync(string orderId, LinkStatus status, string momoAccount);

        Task<AppleOrderDto> UpdateAddPaymentStatusAsync(string orderId, LinkStatus status, string appleId);

        Task<AppleOrderDto> TakeOrderToLinkAsync();

        Task<AppleOrderDto> TakeOrderToPaymentAsync();

        Task<List<AppleOrderLinkStatusSelectionDto>> GetAppleOrderLinkStatusSelectionsAsync(string username, DateTime? createdFrom, DateTime? createdTo);

        Task ResetLinkStatusAsync(ResetLinkStatusFilterInput input);
    }
}
