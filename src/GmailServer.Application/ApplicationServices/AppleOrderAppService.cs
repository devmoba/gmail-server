using GmailServer.AppleOrders;
using GmailServer.AppleOrders.Statistics;
using GmailServer.Entities;
using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class AppleOrderAppService : ReadOnlyAppService<AppleOrder, AppleOrderDto, long, AppleOrderFilterDto>, IAppleOrderAppService
    {
        public AppleOrderAppService(IReadOnlyRepository<AppleOrder, long> repository) : base(repository)
        {
        }

        public Task<List<AppleOrderLinkStatusSelectionDto>> GetAppleOrderLinkStatusSelectionsAsync(string username, DateTime? createdFrom, DateTime? createdTo)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppleOrderDto>> GetPendingOrderCountByMomoAccountAsync(string momoAccount)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppleOrderStatisticByLinkStatusDto>> GetStatisticByLinkStatusAsync(AppleOrderStatisticFilterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppleOrderStatisticByAddPaymentStatusDto>> GetStatisticByAddPaymentStatusAsync(AppleOrderStatisticFilterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<AppleOrderDto> TakeOrderToLinkAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppleOrderDto> TakeOrderToPaymentAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppleOrderDto> CreateAsync(string orderId, string urlPayment)
        {
            throw new NotImplementedException();
        }

        public Task<AppleOrderDto> UpdateAddPaymentStatusAsync(string orderId, LinkStatus status, string appleId)
        {
            throw new NotImplementedException();
        }

        public Task<AppleOrderDto> UpdateLinkStatusAsync(string orderId, LinkStatus status, string momoAccount)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task ResetLinkStatusAsync(ResetLinkStatusFilterInput input)
        {
            throw new NotImplementedException();
        }
    }
}
