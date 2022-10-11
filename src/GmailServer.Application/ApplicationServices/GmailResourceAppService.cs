using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.GmailResources;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class GmailResourceAppService : CrudAppService<
        GmailResource,
        GmailResourceDto,
        long,
        GmailResourceFilterDto,
        CreateUpdateGmailResourceDto,
        CreateUpdateGmailResourceDto>, IGmailResourceAppService
    {
        private new readonly IGmailResourceRepository Repository;

        public GmailResourceAppService(IGmailResourceRepository repository) : base(repository)
        {
            Repository = repository;

            GetPolicyName = GmailServerPermissions.GmailPremiums.Default;
            GetListPolicyName = GmailServerPermissions.GmailPremiums.Default;
            CreatePolicyName = GmailServerPermissions.GmailPremiums.Create;
            UpdatePolicyName = GmailServerPermissions.GmailPremiums.Update;
            DeletePolicyName = GmailServerPermissions.GmailPremiums.Delete;
        }

        public async override Task<PagedResultDto<GmailResourceDto>> GetListAsync(GmailResourceFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);

            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);

            var res = ObjectMapper.Map<List<GmailResource>, List<GmailResourceDto>>(entities);

            return new PagedResultDto<GmailResourceDto>(count, res);
        }

        public async override Task<GmailResourceDto> CreateAsync(CreateUpdateGmailResourceDto input)
        {
            var gmailResource = ObjectMapper.Map<CreateUpdateGmailResourceDto, GmailResource>(input);
            gmailResource.Created = DateTime.Now;
            gmailResource.Status = Enums.GmailResourceStatus.Ready;
            var res = await Repository.InsertAsync(gmailResource, autoSave: true);

            return await MapToGetOutputDtoAsync(res);
        }

        [Authorize(GmailServerPermissions.GmailPremiums.Create)]
        public async Task CreateManyAsync(CreateManyGmailResourceInputDto input)
        {
            var gmailResources = input.Emails.Split("\r\n").ToList();
            if (gmailResources.Count == 0)
                throw new UserFriendlyException("Input empty!");
            var entities = new List<GmailResource>();
            foreach (var gr in gmailResources)
            {
                if (ValidateGmailResourceInput(gr))
                {
                    var gpSplit = gr.Split('|').ToArray();
                    var hasEmail = await Repository.AnyAsync(x => x.Email == gpSplit[0]);
                    if (!hasEmail)
                    {
                        var entity = new GmailResource()
                        {
                            Username = input.Username,
                            Email = gpSplit[0],
                            Password = gpSplit[1],
                            Status = Enums.GmailResourceStatus.Ready,
                            Created = DateTime.Now,
                            Updated = DateTime.Now,
                            TakenTime = DateTime.Now
                        };
                        entity.RecoveryEmail = gpSplit.Length >= 3 ? gpSplit[2] : string.Empty;
                        entities.Add(entity);
                    }
                }
            }

            if (entities.Count > 0)
            {
                await Repository.BulkInsertAsync(entities.DistinctBy(x => x.Email).ToList());
            }
        }

        [Authorize(GmailServerPermissions.GmailPremiums.Delete)]
        public async Task DeleteAllAsync()
        {
            await Repository.DeleteAllAsync();
        }

        public async Task<GmailResourceDto> GetFirstGmailResourceAsync()
        {
            var query = Repository.Where(x => x.Status == Enums.GmailResourceStatus.Ready);
            query = query.OrderByDescending(x => x.Created);
            var gmailResource = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (gmailResource != null)
            {
                var res = ObjectMapper.Map<GmailResource, GmailResourceDto>(gmailResource);
                gmailResource.Status = Enums.GmailResourceStatus.Pending;
                gmailResource.TakenTime = DateTime.Now;
                gmailResource.Updated = DateTime.Now;
                await Repository.UpdateAsync(gmailResource, autoSave: true);
                return res;
            }
            return new GmailResourceDto();
        }

        private bool ValidateGmailResourceInput(string str)
        {
            return Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\|(.+)");

        }

        public async Task<GmailResourceDto> UpdateStatusAsync(string email, GmailResourceStatus status)
        {
            var gmailResource = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (gmailResource != null)
            {
                gmailResource.Status = status;
                gmailResource.Updated = DateTime.Now;
                var res = await Repository.UpdateAsync(gmailResource);
                return await MapToGetOutputDtoAsync(res);
            }
            return new GmailResourceDto();

        }
    }
}
