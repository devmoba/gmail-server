using GmailServer.DownloadedApps;
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
    public class DownloadedAppAppService : ReadOnlyAppService<DownloadedApp, DownloadedAppGetOutputDto, DownloadedAppGetListOutputDto, long, DownloadAppFilterDto>, IDownloadedAppAppService
    {
        private readonly new IDownloadedAppRepository Repository;
        private readonly IAppleIdRepository appleIdRepository;

        public DownloadedAppAppService(IDownloadedAppRepository repository,
            IAppleIdRepository appleIdRepository) : base(repository)
        {
            Repository = repository;
            this.appleIdRepository = appleIdRepository;
        }

        [Authorize(GmailServerPermissions.DownloadedApps.Default)]
        public async override Task<PagedResultDto<DownloadedAppGetListOutputDto>> GetListAsync(DownloadAppFilterDto input)
        {
            var query = await Repository.WithDetailsAsync(x => x.AppleId);
            if (!string.IsNullOrEmpty(input.ProductId))
                query = Repository.FullTextSearch(query, x => x.ProductId, input.ProductId);
            if (!string.IsNullOrEmpty(input.AppId))
                query = Repository.FullTextSearch(query, x => x.AppId, input.AppId);

            query = query.WhereIf(!string.IsNullOrEmpty(input.Email), x => x.Email == input.Email.ToLower().Trim());
            query = query.WhereIf(!string.IsNullOrEmpty(input.AppleId), x => x.AppleId.Email == input.AppleId.ToLower().Trim());
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created < input.CreatedTo.Value.Date.AddDays(1));
            query = query.WhereIf(input.AppleIdFK.HasValue, x => x.AppleIdFK == input.AppleIdFK.Value);

            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);

            var res = ObjectMapper.Map<List<DownloadedApp>, List<DownloadedAppGetListOutputDto>>(entities);

            return new PagedResultDto<DownloadedAppGetListOutputDto>(count, res);
        }

        [Authorize(GmailServerPermissions.DownloadedApps.Default)]
        public override Task<DownloadedAppGetOutputDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        public async Task<DownloadedAppGetOutputDto> CreateAsync(CreateDownloadedAppDto input)
        {
            var appleId = await AsyncExecuter.FirstOrDefaultAsync(
                this.appleIdRepository.Where(x => x.Email == input.Email.ToLower().Trim()));
            var downloadedApp = ObjectMapper.Map<CreateDownloadedAppDto, DownloadedApp>(input);
            downloadedApp.Created = DateTime.Now;
            if (appleId != null)
                downloadedApp.AppleIdFK = appleId.Id;
            var res = await Repository.InsertAsync(downloadedApp);
            return await MapToGetOutputDtoAsync(res);
        }

        [Authorize(GmailServerPermissions.DownloadedApps.Delete)]
        public async Task DeleteAsync(long id)
        {
            await Repository.DeleteAsync(id);
        }
    }
}
