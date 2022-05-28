using GmailServer.Entities;
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

namespace GmailServer
{
    [RemoteService(IsEnabled = false)]
    public class GmailAppService : ReadOnlyAppService<
        Gmail,
        GmailDto,
        long,
        GmailFilterDto>, IGmailAppService
    {
        private new readonly IGmailRepository Repository;
        public GmailAppService(IGmailRepository repository) : base(repository)
        {
            Repository = repository;
        }

        [Authorize(GmailServerPermissions.Gmails.Default)]
        public async override Task<PagedResultDto<GmailDto>> GetListAsync(GmailFilterDto input)
        { 
            var query = Repository.AsQueryable();

            if (!string.IsNullOrEmpty(input.Email))
                query = Repository.FullTextSearch(query, x => x.Email, input.Email);

            if (!string.IsNullOrEmpty(input.RecoveryEmail))
                query = Repository.FullTextSearch(query, x => x.RecoveryEmail, input.RecoveryEmail);

            if (!string.IsNullOrEmpty(input.Country))
                query = Repository.FullTextSearch(query, x => x.Country, input.Country);

            query = query.WhereIf(input.Status.HasValue, x => x.Status == input.Status);
            query = query.WhereIf(input.Gender.HasValue, x => x.Gender == input.Gender);

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
            var res = await Repository.InsertAsync(gmail);

            return ObjectMapper.Map<Gmail, GmailDto>(gmail);
        }
    }
}
