using GmailServer.Entities;
using GmailServer.GmailTypes;
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
    public class GmailTypeAppService : CrudAppService<
        GmailType,
        GmailTypeDto,
        long,
        GmailTypeFilterDto,
        CreateUpdateGmailTypeDto,
        CreateUpdateGmailTypeDto>, IGmailTypeAppService
    {
        private readonly new IGmailTypeRepository Repository;

        public GmailTypeAppService(IGmailTypeRepository repository) : base(repository)
        {
            Repository = repository;
            GetListPolicyName = GmailServerPermissions.GmailTypes.Default;
            GetPolicyName = GmailServerPermissions.GmailTypes.Default;
            CreatePolicyName = GmailServerPermissions.GmailTypes.Create;
            UpdatePolicyName = GmailServerPermissions.GmailTypes.Update;
            DeletePolicyName = GmailServerPermissions.GmailTypes.Delete;
        }

        public async override Task<PagedResultDto<GmailTypeDto>> GetListAsync(GmailTypeFilterDto input)
        {
            var query = Repository.AsQueryable();

            if (!string.IsNullOrEmpty(input.Name))
                query = Repository.FullTextSearch(query, x => x.Name, input.Name);

            if (!string.IsNullOrEmpty(input.DeviceType))
                query = Repository.FullTextSearch(query, x => x.DeviceType, input.DeviceType);

            if (!string.IsNullOrEmpty(input.FakeVersion))
                query = Repository.FullTextSearch(query, x => x.FakeVersion, input.FakeVersion);

            if (!string.IsNullOrEmpty(input.Version))
                query = Repository.FullTextSearch(query, x => x.Version, input.Version);

            if (!string.IsNullOrEmpty(input.Country))
                query = Repository.FullTextSearch(query, x => x.Country, input.Country);

            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);

            var res = ObjectMapper.Map<List<GmailType>, List<GmailTypeDto>>(entities);

            return new PagedResultDto<GmailTypeDto>(count, res);
        }

        public override async Task<GmailTypeDto> CreateAsync(CreateUpdateGmailTypeDto input)
        {
            try
            {
                var gmailType = new GmailType()
                {
                    Name = input.Name,
                    DeviceType = !string.IsNullOrEmpty(input.DeviceType) ? input.DeviceType.ToLower() : null, 
                    Version = !string.IsNullOrEmpty(input.Version) ? input.Version.ToLower() : null,
                    FakeVersion = !string.IsNullOrEmpty(input.FakeVersion) ? input.FakeVersion.ToLower() : null,
                    Country = !string.IsNullOrEmpty(input.Country) ? input.Country.ToLower() : null,
                };

                var res = await Repository.InsertAsync(gmailType, autoSave: true);
                return await MapToGetOutputDtoAsync(res);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<GmailTypeDto> UpdateAsync(long id, CreateUpdateGmailTypeDto input)
        {
            try
            {
                var entity = await Repository.FindAsync(id);
                entity.Name = input.Name;
                entity.DeviceType = input.DeviceType.ToLower();
                entity.Version = input.Version.ToLower();
                entity.FakeVersion = input.FakeVersion.ToLower();
                entity.Country = input.Country.ToLower();
                var res = await Repository.UpdateAsync(entity);

                return await MapToGetOutputDtoAsync(res);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        //public override async Task DeleteAsync(long id)
        //{
        //    try
        //    {
        //        await Repository.DeleteAsync(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}

        public async Task<List<GmailTypeSelectionDto>> GetAllSelectionAsync()
        {
            var gmailTypes = await Repository.GetListAsync();

            return ObjectMapper.Map<List<GmailType>, List<GmailTypeSelectionDto>>(gmailTypes);
        }
    }
}
