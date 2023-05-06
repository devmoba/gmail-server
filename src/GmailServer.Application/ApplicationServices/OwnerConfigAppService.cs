using GmailServer.Entities;
using GmailServer.OwnerConfigs;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class OwnerConfigAppService : CrudAppService<
        OwnerConfig, 
        OwnerConfigDto, 
        long, 
        OwnerConfigFilterDto, 
        CreateUpdateOwnerConfigDto, 
        CreateUpdateOwnerConfigDto>, IOwnerConfigAppService
    {
        private readonly IOwnerConfigRepository _repository;
        public OwnerConfigAppService(IOwnerConfigRepository repository) : base(repository)
        {
            _repository = repository;
        }

        [Authorize(GmailServerPermissions.OwnerConfigs.Default)]
        public async override Task<PagedResultDto<OwnerConfigDto>> GetListAsync(OwnerConfigFilterDto input)
        {
            var query = _repository.AsQueryable();
            if (!string.IsNullOrEmpty(input.Key))
            {
                query = _repository.FullTextSearch(query, x => x.Key, input.Key);
            }
            query = query.WhereIf(!string.IsNullOrEmpty(input.Value), x => x.Value == input.Value);
            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);
            var res = ObjectMapper.Map<List<OwnerConfig>, List<OwnerConfigDto>>(entities);
            return new PagedResultDto<OwnerConfigDto>(count, res);
        }

        [Authorize(GmailServerPermissions.OwnerConfigs.Default)]
        public override Task<OwnerConfigDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        [Authorize(GmailServerPermissions.OwnerConfigs.Create)]
        public override Task<OwnerConfigDto> CreateAsync(CreateUpdateOwnerConfigDto input)
        {
            return base.CreateAsync(input);
        }

        [Authorize(GmailServerPermissions.OwnerConfigs.Delete)]
        public override Task DeleteAsync(long id)
        {
            return base.DeleteAsync(id);
        }
    }
}
