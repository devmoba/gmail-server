using GmailServer.AppleOrders;
using GmailServer.AppleOrders.Statistics;
using GmailServer.Enums;
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

        Task<int> GetPendingOrderCountByMomoAccountAsync([Required] string momoAccount);

        Task<int> GetOrderCountByStatusAsync([Required] string linkStatus, [Required] string addPaymentStatus);

        Task<int> GetReadyOrderCountAsync();

        Task<int> GetLinkedOrderCountAsync();

        Task<PagedResultDto<AppleOrderStatisticByLinkStatusDto>> GetStatisticByLinkStatusAsync(AppleOrderStatisticFilterDto input);

        Task<PagedResultDto<AppleOrderStatisticByAddPaymentStatusDto>> GetStatisticByAddPaymentStatusAsync(AppleOrderStatisticFilterDto input);

        Task<AppleOrderDto> CreateAsync([Required]string orderId, [Required]string urlPayment);

        Task DeleteAsync(long id);
    }
}
