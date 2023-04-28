using GmailServer.AppleOrders;
using GmailServer.AppleOrders.Statistics;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class AppleOrderAppService : ReadOnlyAppService<AppleOrder, AppleOrderDto, long, AppleOrderFilterDto>, IAppleOrderAppService
    {
        private readonly new IAppleOrderRepository Repository;

        public AppleOrderAppService(IAppleOrderRepository repository) : base(repository)
        {
            Repository = repository;
        }

        [Authorize(GmailServerPermissions.AppleOrders.Default)]
        public async override Task<PagedResultDto<AppleOrderDto>> GetListAsync(AppleOrderFilterDto input)
        {
            var query = Repository.AsQueryable();

            query.WhereIf(!string.IsNullOrEmpty(input.OrderID), x => x.OrderID == input.OrderID)
                .WhereIf(input.LinkStatus.HasValue, x => x.LinkStatus == input.LinkStatus.Value)
                .WhereIf(input.CreatedTimeFrom.HasValue, x => x.CreatedTime >= input.CreatedTimeFrom.Value)
                .WhereIf(input.CreatedTimeTo.HasValue, x => x.CreatedTime <= input.CreatedTimeTo.Value);

            if (!string.IsNullOrEmpty(input.MomoAccount))
                query = Repository.FullTextSearch(query, x => x.MomoAccount, input.MomoAccount);

            if (!string.IsNullOrEmpty(input.AppleID))
                query = Repository.FullTextSearch(query, x => x.AppleID, input.AppleID);

            var count = await AsyncExecuter.CountAsync(query);
            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);
            var entities = await AsyncExecuter.ToListAsync(query);
            var res = ObjectMapper.Map<List<AppleOrder>, List<AppleOrderDto>>(entities);

            return new PagedResultDto<AppleOrderDto>(count, res);
        }

        [Authorize(GmailServerPermissions.AppleOrders.Default)]
        public override Task<AppleOrderDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        public async Task<AppleOrderDto> TakeOrderToLinkAsync()
        {
            var timeCondition = DateTime.Now.AddMinutes(-5);
            var query = Repository.Where(x => x.LinkStatus == LinkStatus.Ready && x.CreatedTime > timeCondition);
            query = query.OrderBy(x => x.Id);
            var appleOrder = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (appleOrder != null)
            {
                var res = ObjectMapper.Map<AppleOrder, AppleOrderDto>(appleOrder);
                appleOrder.LinkStatus = LinkStatus.InUse;
                appleOrder.LinkTakenTime = DateTime.Now;
                await Repository.UpdateAsync(appleOrder, true);
                return res;
            }
            return null;
        }

        public async Task<AppleOrderDto> UpdateLinkStatusAsync(string orderId, LinkStatus status, string momoAccount = default)
        {
            var appleOrder = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.OrderID == orderId));
            if (appleOrder != null)
            {
                appleOrder.LinkStatus = status;
                appleOrder.LinkCompletedTime = DateTime.Now;
                if (!string.IsNullOrEmpty(momoAccount))
                {
                    appleOrder.MomoAccount = momoAccount;
                }
                var entity = await Repository.UpdateAsync(appleOrder, true);
                return ObjectMapper.Map<AppleOrder, AppleOrderDto>(entity);
            }
            return null;
        }

        public async Task<AppleOrderDto> TakeOrderToAddPaymentAsync()
        {
            var timeCondition = DateTime.Now.AddMinutes(-5);
            var query = Repository.Where(x => x.LinkStatus == LinkStatus.Linked
                    && x.AddPaymentStatus == AddPaymentStatus.None
                    && x.LinkCompletedTime > timeCondition)
                .OrderBy(x => x.LinkCompletedTime);
            var appleOrder = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (appleOrder != null)
            {
                var res = ObjectMapper.Map<AppleOrder, AppleOrderDto>(appleOrder);
                appleOrder.AddPaymentStatus = AddPaymentStatus.InUse;
                appleOrder.AddPaymentTakenTime = DateTime.Now;
                await Repository.UpdateAsync(appleOrder, true);
                return res;
            }
            return null;
        }

        public async Task<AppleOrderDto> UpdateAddPaymentStatusAsync(string orderId, AddPaymentStatus status, string appleId)
        {
            var appleOrder = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.OrderID == orderId));
            if (appleOrder != null)
            {
                appleOrder.AddPaymentStatus = status;
                appleOrder.AddPaymentCompletedTime = DateTime.Now;
                if (!string.IsNullOrEmpty(appleId))
                {
                    appleOrder.AppleID = appleId;
                }
                var entity = await Repository.UpdateAsync(appleOrder, true);
                return ObjectMapper.Map<AppleOrder, AppleOrderDto>(entity);
            }
            return null;
        }

        public async Task<int> GetPendingOrderCountByMomoAccountAsync(string momoAccount)
        {
            var query = Repository.Where(x => x.MomoAccount == momoAccount
                    && x.LinkStatus == LinkStatus.Linked
                    && (x.AddPaymentStatus == AddPaymentStatus.None || x.AddPaymentStatus == AddPaymentStatus.InUse)
                    && (x.LinkCompletedTime > DateTime.Now.AddMinutes(-10)));
            var count = await AsyncExecuter.CountAsync(query);
            return count;
        }

        public async Task<int> GetOrderCountByStatusAsync(LinkStatus[] linkStatus, AddPaymentStatus[] addPaymentStatus)
        {
            var query = Repository.Where(x => linkStatus.Contains(x.LinkStatus)
                    && addPaymentStatus.Contains(x.AddPaymentStatus));
            return await AsyncExecuter.CountAsync(query);
        }

        [Authorize]
        public async Task<List<AppleOrderLinkStatusSelectionDto>> GetAppleOrderLinkStatusSelectionsAsync(DateTime? createdFrom, DateTime? createdTo)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(createdFrom.HasValue, x => x.CreatedTime.Date >= createdFrom.Value.Date);
            query = query.WhereIf(createdTo.HasValue, x => x.CreatedTime.Date <= createdTo.Value.Date);
            var groupBy = query.GroupBy(x => x.LinkStatus).Select(x => new AppleOrderLinkStatusSelectionDto()
            {
                Text = $"{x.Key.ToString()} | {x.Count()}",
                Value = x.Key
            });
            var res = await AsyncExecuter.ToListAsync(groupBy);
            return res;
        }

        [Authorize(GmailServerPermissions.AppleOrders.Statistic)]
        public async Task<PagedResultDto<AppleOrderStatisticByLinkStatusDto>> GetStatisticByLinkStatusAsync(AppleOrderStatisticFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(input.CreatedTimeFrom.HasValue, x => x.CreatedTime.Date >= input.CreatedTimeFrom.Value.Date);
            query = query.WhereIf(input.CreatedTimeTo.HasValue, x => x.CreatedTime.Date <= input.CreatedTimeTo.Value.Date);
            var group = query.GroupBy(x => new { CreatedTime = x.CreatedTime.Date })
                .Select(g => new AppleOrderStatisticByLinkStatusDto()
                {
                    CreatedTime = g.Key.CreatedTime,
                    Total = g.Count(),
                    Ready = g.Where(x => x.LinkStatus == LinkStatus.Ready).Count(),
                    InUse = g.Where(x => x.LinkStatus == LinkStatus.InUse).Count(),
                    Expired = g.Where(x => x.LinkStatus == LinkStatus.Expired).Count(),
                    Error = g.Where(x => x.LinkStatus == LinkStatus.Error).Count(),
                    Linked = g.Where(x => x.LinkStatus == LinkStatus.Linked).Count()
                });
            var count = await AsyncExecuter.CountAsync(group);
            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                group = group.Skip(input.SkipCount).Take(input.MaxResultCount);

            var res = await AsyncExecuter.ToListAsync(group);
            return new PagedResultDto<AppleOrderStatisticByLinkStatusDto>(count, res.OrderByDescending(x => x.CreatedTime).ToList());
        }

        [Authorize(GmailServerPermissions.AppleOrders.Statistic)]
        public async Task<PagedResultDto<AppleOrderStatisticByAddPaymentStatusDto>> GetStatisticByAddPaymentStatusAsync(AppleOrderStatisticFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(input.CreatedTimeFrom.HasValue, x => x.CreatedTime.Date >= input.CreatedTimeFrom.Value.Date);
            query = query.WhereIf(input.CreatedTimeTo.HasValue, x => x.CreatedTime.Date <= input.CreatedTimeTo.Value.Date);
            var group = query.GroupBy(x => new { CreatedTime = x.CreatedTime.Date })
                .Select(g => new AppleOrderStatisticByAddPaymentStatusDto()
                {
                    CreatedTime = g.Key.CreatedTime,
                    Total = g.Count(),
                    None = g.Where(x => x.AddPaymentStatus == AddPaymentStatus.None).Count(),
                    InUse = g.Where(x => x.AddPaymentStatus == AddPaymentStatus.InUse).Count(),
                    Expired = g.Where(x => x.AddPaymentStatus == AddPaymentStatus.Expired).Count(),
                    Error = g.Where(x => x.AddPaymentStatus == AddPaymentStatus.Error).Count(),
                    Completed = g.Where(x => x.AddPaymentStatus == AddPaymentStatus.Completed).Count()
                });
            var count = await AsyncExecuter.CountAsync(group);
            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                group = group.Skip(input.SkipCount).Take(input.MaxResultCount);

            var res = await AsyncExecuter.ToListAsync(group);
            return new PagedResultDto<AppleOrderStatisticByAddPaymentStatusDto>(count, res.OrderByDescending(x => x.CreatedTime).ToList());
        }

        public async Task<AppleOrderDto> CreateAsync(string orderId, string urlPayment)
        {
            if (!string.IsNullOrEmpty(orderId) && !string.IsNullOrEmpty(urlPayment))
            {
                var hasOrder = await AsyncExecuter.AnyAsync(Repository.Where(x => x.OrderID == orderId));
                if (hasOrder)
                    throw new UserFriendlyException("OrderId already exists in the database;");
                var order = new AppleOrder()
                {
                    OrderID = orderId,
                    URLPayment = urlPayment,
                    LinkStatus = LinkStatus.Ready,
                    CreatedTime = DateTime.Now
                };
                var entity = await Repository.InsertAsync(order, true);
                return ObjectMapper.Map<AppleOrder, AppleOrderDto>(entity);
            }
            throw new UserFriendlyException("[orderId, urlPayment] parameter must be required!");
        }

        [Authorize(GmailServerPermissions.AppleOrders.Delete)]
        public async Task DeleteAsync(long id)
        {
            await Repository.DeleteAsync(id);
        }

        //[Authorize(GmailServerPermissions.AppleOrders.ResetLinkStatus)]
        //public async Task ResetLinkStatusAsync(ResetLinkStatusFilterInput input)
        //{

        //}
    }
}
