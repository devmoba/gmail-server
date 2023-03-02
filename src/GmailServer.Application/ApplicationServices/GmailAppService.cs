using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Gmails;
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
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class GmailAppService : ReadOnlyAppService<
        Gmail,
        GmailDto,
        long,
        GmailFilterDto>, IGmailAppService
    {
        private readonly new IGmailRepository Repository;
        private readonly new IGmailTypeRepository gmailTypeRepository;

        public GmailAppService(IGmailRepository repository, IGmailTypeRepository gmailTypeRepository) : base(repository)
        {
            Repository = repository;
            this.gmailTypeRepository = gmailTypeRepository;
        }

        [Authorize(GmailServerPermissions.Gmails.Default)]
        public async override Task<PagedResultDto<GmailDto>> GetListAsync(GmailFilterDto input)
        {
            var query = await Repository.WithDetailsAsync(x => x.GmailType);

            if (!string.IsNullOrEmpty(input.Email))
                query = Repository.FullTextSearch(query, x => x.Email, input.Email);

            if (!string.IsNullOrEmpty(input.RecoveryEmail))
                query = Repository.FullTextSearch(query, x => x.RecoveryEmail, input.RecoveryEmail);

            if (!string.IsNullOrEmpty(input.Country))
                query = Repository.FullTextSearch(query, x => x.Country, input.Country);

            query = query.WhereIf(input.Status.HasValue, x => x.Status == input.Status);
            query = query.WhereIf(input.GmailTypeId.HasValue, x => x.GmailTypeId == input.GmailTypeId);

            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);

            var res = ObjectMapper.Map<List<Gmail>, List<GmailDto>>(entities);

            return new PagedResultDto<GmailDto>(count, res);
        }

        [Authorize(GmailServerPermissions.Gmails.Default)]
        public override Task<GmailDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        public async Task<GmailDto> CreateAsync(CreateGmailDto input)
        {
            var gmail = ObjectMapper.Map<CreateGmailDto, Gmail>(input);

            gmail.Status = Enums.Status.Unknown;
            gmail.Created = DateTime.Now;
            gmail.Updated = DateTime.Now;
            gmail.LastCheck = DateTime.Now;
            gmail.TimeDiff = 0;
            gmail.RecoveryEmail = string.IsNullOrEmpty(input.RecoveryEmail) ? string.Empty : input.RecoveryEmail;
            var gmailTypes = await this.gmailTypeRepository.GetListAsync();
            foreach (var gmailType in gmailTypes)
            {
                if ((!string.IsNullOrEmpty(gmailType.DeviceType) && gmailType.DeviceType != gmail.DeviceType) || 
                    (!string.IsNullOrEmpty(gmailType.Version) && gmailType.Version != gmail.Version) || 
                    (!string.IsNullOrEmpty(gmailType.FakeVersion) && gmailType.FakeVersion != gmail.FakeVersion) ||
                    (!string.IsNullOrEmpty(gmailType.Country) && gmailType.Country != gmail.Country))
                {
                    break;
                }

                gmail.GmailTypeId = gmailType.Id;
                break;
            }
            var res = await Repository.InsertAsync(gmail);
            return ObjectMapper.Map<Gmail, GmailDto>(res);
        }

        [Authorize(GmailServerPermissions.Gmails.Download)]
        public async Task<List<GmailDto>> GetByTimeRange(DateTime? from, DateTime? to)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(from.HasValue, x => x.Created >= from);
            query = query.WhereIf(to.HasValue, x => x.Created <= to);
            var res = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<Gmail>, List<GmailDto>>(res);
        }

        [Authorize(GmailServerPermissions.Gmails.Download)]
        public async Task<List<GmailDto>> GetAll()
        {
            var res = await Repository.ToListAsync();
            return ObjectMapper.Map<List<Gmail>, List<GmailDto>>(res);
        }

        [Authorize(GmailServerPermissions.Gmails.Delete)]
        public async Task DeleteAsync(long id)
        {
            await Repository.DeleteAsync(id);
        }

        [Authorize(GmailServerPermissions.Dashboard.Home)]
        public async Task<PagedResultDto<GmailReportDto>> GetGmailReportsAsync(GmailReportFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);

            var queryGroupBy = query.GroupBy(x => new { Created = x.Created.Date }).Select(g => new GmailReportDto()
            {
                Created = g.Key.Created.Date,
                TotalDaily = g.Count(),
                Unknown = g.Where(x => x.Status == Status.Unknown).Count(),
                Good = g.Where(x => x.Status == Status.Good).Count(),
                Disable = g.Where(x => x.Status == Status.Disable).Count(),
                Notexist = g.Where(x => x.Status == Status.Notexist).Count(),
                Verify = g.Where(x => x.Status == Status.Verify).Count(),
                Checking = g.Where(x => x.Status == Status.Checking).Count(),
                Uncheck = g.Where(x => x.Status == Status.Uncheck).Count()
            });
            queryGroupBy = queryGroupBy.OrderByDescending(x => x.Created);

            var count = await AsyncExecuter.CountAsync(queryGroupBy);
            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                queryGroupBy = queryGroupBy.Skip(input.SkipCount).Take(input.MaxResultCount);

            var res = await AsyncExecuter.ToListAsync(queryGroupBy);
            return new PagedResultDto<GmailReportDto>(count, res);
        }

        [Authorize(GmailServerPermissions.Dashboard.Home)]
        public async Task<ReportbyStatusDto> GetReportbyStatusAsync()
        {

            var query = Repository.AsQueryable();
            var total = await AsyncExecuter.CountAsync(query);
            var queryGroupByStatus = query.GroupBy(x => x.Status).Select(x => new StatusPoint()
            {
                Name = x.Key.ToString(),
                Y = x.Count(),
                Exploded = x.Key == Status.Good ? true : false
            });
            var statusPoints = await AsyncExecuter.ToListAsync(queryGroupByStatus);

            return new ReportbyStatusDto()
            {
                Total = total,
                StatusPoints = statusPoints
            };
        }
    }
}
