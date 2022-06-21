using GmailServer.Entities;
using GmailServer.FakeSettings;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
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
    public class FakeSettingAppService : CrudAppService<
        FakeSetting, 
        FakeSettingDto, 
        long, 
        FakeSettingFilterDto, 
        CreateUpdateFakeSettingDto, 
        CreateUpdateFakeSettingDto>, IFakeSettingAppService
    {
        private new readonly IFakeSettingRepository Repository;

        public FakeSettingAppService(IFakeSettingRepository repository) : base(repository)
        {
            Repository = repository;

        }

        [Authorize(GmailServerPermissions.FakeSettings.Default)]
        public async override Task<PagedResultDto<FakeSettingDto>> GetListAsync(FakeSettingFilterDto input)
        {
            var query = Repository.AsQueryable();
            if (!string.IsNullOrEmpty(input.Version))
            {
                query = Repository.FullTextSearch(query, x => x.Version, input.Version);
            }
            if (!string.IsNullOrEmpty(input.FakeVersion))
            {
                query = Repository.FullTextSearch(query, x => x.FakeVersion, input.FakeVersion);
            }
            if (!string.IsNullOrEmpty(input.DeviceType))
            {
                query = Repository.FullTextSearch(query, x => x.DeviceType, input.DeviceType);
            }

            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);

            var res = ObjectMapper.Map<List<FakeSetting>, List<FakeSettingDto>>(entities);

            return new PagedResultDto<FakeSettingDto>(count, res);
        }

        [Authorize(GmailServerPermissions.FakeSettings.Default)]
        public override Task<FakeSettingDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        [Authorize(GmailServerPermissions.FakeSettings.Create)]
        public override Task<FakeSettingDto> CreateAsync(CreateUpdateFakeSettingDto input)
        {
            return base.CreateAsync(input);
        }

        [Authorize(GmailServerPermissions.FakeSettings.Update)]
        public override Task<FakeSettingDto> UpdateAsync(long id, CreateUpdateFakeSettingDto input)
        {
            return base.UpdateAsync(id, input); 
        }

        [Authorize(GmailServerPermissions.FakeSettings.Delete)]
        public override Task DeleteAsync(long id)
        {
            return base.DeleteAsync(id);
        }

        public async Task<List<FakeSettingDto>> GetAll()
        {
            var res = await Repository.ToListAsync();
            return ObjectMapper.Map<List<FakeSetting>, List<FakeSettingDto>>(res);
        }
    }
}
