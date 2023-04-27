using GmailServer.AppleOrders;
using GmailServer.AppleOrders.Statistics;
using GmailServer.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IAppleOrderController
    {
        Task<PagedResultDto<AppleOrderDto>> GetListAsync(AppleOrderFilterDto input);

        Task<AppleOrderDto> GetAsync(long id);

        Task<AppleOrderDto> TakeOrderToLinkAsync();

        Task<AppleOrderDto> UpdateLinkStatusAsync(string orderId, LinkStatus status, string momoAccount = default);

        Task<AppleOrderDto> TakeOrderToAddPaymentAsync();

        Task<AppleOrderDto> UpdateAddPaymentStatusAsync(string orderId, AddPaymentStatus status, string appleId);

        Task<List<AppleOrderDto>> GetPendingOrderCountByMomoAccountAsync(string momoAccount);

        Task<PagedResultDto<AppleOrderStatisticByLinkStatusDto>> GetStatisticByLinkStatusAsync(AppleOrderStatisticFilterDto input);

        Task<PagedResultDto<AppleOrderStatisticByAddPaymentStatusDto>> GetStatisticByAddPaymentStatusAsync(AppleOrderStatisticFilterDto input);

        Task<AppleOrderDto> CreateAsync([Required]string orderId, [Required]string urlPayment);

        Task DeleteAsync(long id);
    }
}
