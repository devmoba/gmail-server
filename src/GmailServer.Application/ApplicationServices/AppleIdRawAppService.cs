using GmailServer.AppleIdRaws;
using GmailServer.AppleOrders.Statistics;
using GmailServer.Entities;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class AppleIdRawAppService : ApplicationService, IAppleIdRawAppService
    {
        private readonly IRepository<AppleIdRaw, long> _repository;

        public AppleIdRawAppService(IRepository<AppleIdRaw, long> repository)
        {
            _repository = repository;
        }

        public async Task<AppleIdRawDto> CreateAsync(CreateAppleIdRawInputDto input)
        {
            var appleIdRaw = ObjectMapper.Map<CreateAppleIdRawInputDto, AppleIdRaw>(input);
            appleIdRaw.Created = DateTime.Now;
            var entity = await _repository.InsertAsync(appleIdRaw, true);
            return ObjectMapper.Map<AppleIdRaw, AppleIdRawDto>(entity);
        }

        [Authorize(GmailServerPermissions.AppleIdRaws.Statistic)]
        public async Task<PagedResultDto<AppleIdRawStatisticDailyDto>> GetAppleIdRawStatisticDailyAsync(AppleIdRawStatisticFilterDto input)
        {
            var query = _repository.AsQueryable();
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created < input.CreatedTo.Value.Date.AddDays(1));
            var group = query.GroupBy(x => new { Created = x.Created.Date }).Select(g => new AppleIdRawStatisticDailyDto()
            {
                Created = g.Key.Created,
                Count = g.Count()
            });

            var count = await AsyncExecuter.CountAsync(group);
            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                group = group.Skip(input.SkipCount).Take(input.MaxResultCount);

            var res = await AsyncExecuter.ToListAsync(group);
            return new PagedResultDto<AppleIdRawStatisticDailyDto>(count, res.OrderByDescending(x => x.Created).ToList());
        }
    }
}
