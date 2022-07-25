using GmailServer.Checkers;
using GmailServer.Entities;
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
    [Authorize]
    public class CheckerAppService : ReadOnlyAppService<
        Checker, 
        CheckerDto, 
        long, 
        CheckerFilterDto>, ICheckerAppService
    {
        private new readonly ICheckerRepository Repository;
        public CheckerAppService(ICheckerRepository repository) : base(repository)
        {
            Repository = repository;
            GetListPolicyName = GmailServerPermissions.Checkers.Default;
            GetPolicyName = GmailServerPermissions.Checkers.Default;
        }

        public override async Task<PagedResultDto<CheckerDto>> GetListAsync(CheckerFilterDto input)
        {
            var query = Repository.AsQueryable();
            if (!string.IsNullOrEmpty(input.CheckerId))
            {
                if (Guid.TryParse(input.CheckerId, out Guid checkerId))
                {
                    query = query.Where(x => x.CheckerId == checkerId);
                };
            }

            if (!string.IsNullOrEmpty(input.CheckerIP))
            {
                query = Repository.FullTextSearch(query, x => x.CheckerIP, input.CheckerIP);
            }

            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);

            var res = ObjectMapper.Map<List<Checker>, List<CheckerDto>>(entities);

            return new PagedResultDto<CheckerDto>(count, res);
        }

        public override Task<CheckerDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        public async Task DeleteAsync(long id)
        {
            await Repository.DeleteAsync(id, true);
        }
    }
}
