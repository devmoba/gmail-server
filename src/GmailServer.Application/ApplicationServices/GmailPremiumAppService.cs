using GmailServer.Entities;
using GmailServer.Extensions;
using GmailServer.GmailPremiums;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class GmailPremiumAppService : CrudAppService<
        GmailPremium,
        GmailPremiumDto,
        long,
        GmailPremiumFilterDto,
        CreateUpdateGmailPremiumDto,
        CreateUpdateGmailPremiumDto>, IGmailPremiumAppService
    {
        private new readonly IGmailPremiumRepository Repository;
        private static SemaphoreSlim getSyncLock = new SemaphoreSlim(1, 1);

        public GmailPremiumAppService(IGmailPremiumRepository repository) : base(repository)
        {
            Repository = repository;
            GetPolicyName = GmailServerPermissions.GmailPremiums.Default;
            GetListPolicyName = GmailServerPermissions.GmailPremiums.Default;
            UpdatePolicyName = GmailServerPermissions.GmailPremiums.Update;
            DeletePolicyName = GmailServerPermissions.GmailPremiums.Delete;
        }

        [Authorize(GmailServerPermissions.GmailPremiums.Default)]
        public async override Task<PagedResultDto<GmailPremiumDto>> GetListAsync(GmailPremiumFilterDto input)
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

            var res = ObjectMapper.Map<List<GmailPremium>, List<GmailPremiumDto>>(entities);

            return new PagedResultDto<GmailPremiumDto>(count, res);
        }

        [Authorize(GmailServerPermissions.GmailPremiums.Default)]
        public override Task<GmailPremiumDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        [Authorize(GmailServerPermissions.GmailPremiums.Update)]
        public override Task<GmailPremiumDto> UpdateAsync(long id, CreateUpdateGmailPremiumDto input)
        {
            return base.UpdateAsync(id, input);
        }

        public override async Task<GmailPremiumDto> CreateAsync(CreateUpdateGmailPremiumDto input)
        {
            if (CommonMethod.IsValidEmail(input.Email))
            {
                var gmailPremium = ObjectMapper.Map<CreateUpdateGmailPremiumDto, GmailPremium>(input);
                gmailPremium.Created = DateTime.Now;
                //gmailPremium.Updated = DateTime.Now;    
                gmailPremium.Status = Enums.GmailPremiumStatus.Ready;
                gmailPremium.RecoveryEmail = string.IsNullOrEmpty(input.RecoveryEmail) ? string.Empty : input.RecoveryEmail;
                var res = await Repository.InsertAsync(gmailPremium, autoSave: true);

                return await MapToGetOutputDtoAsync(res);
            }
            else
            {
                throw new UserFriendlyException($"{input.Email} - invalidate!");
            }
           
        }

        [Authorize(GmailServerPermissions.GmailPremiums.Create)]
        public async Task CreateManyAsync(CreateManyGmailPremiumInputDto input)
        {
            var gmailPremiums = input.Emails.Split("\r\n").ToList();
            if (gmailPremiums.Count == 0)
                throw new UserFriendlyException("Input empty!");
            var entities = new List<GmailPremium>();
            foreach (var gmailPremium in gmailPremiums) 
            {
                if (ValidateGmailPremiumInput(gmailPremium))
                {
                    var gmailPremiumSplit = gmailPremium.Split('|').ToArray();
                    var hasEmail = await Repository.AnyAsync(x => x.Email == gmailPremiumSplit[0]);
                    if (!hasEmail)
                    {
                        var entity = new GmailPremium()
                        {
                            Username = input.Username,
                            Email = gmailPremiumSplit[0],
                            Password = gmailPremiumSplit[1],
                            Status = Enums.GmailPremiumStatus.Ready,
                            Created = DateTime.Now
                            //Updated = DateTime.Now
                        };
                        entity.RecoveryEmail = gmailPremiumSplit.Length >= 3 ? gmailPremiumSplit[2] : string.Empty;
                        entities.Add(entity);
                    }
                }
            };
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

        [Authorize(GmailServerPermissions.GmailPremiums.Delete)]
        public override Task DeleteAsync(long id)
        {
            return base.DeleteAsync(id);
        }

        public async Task<GmailPremiumDto> GetFirstGmailPremiumAsync()
        {
            await getSyncLock.WaitAsync();
            try
            {
                var query = Repository.Where(x => x.Status == Enums.GmailPremiumStatus.Ready);
                query = query.OrderByDescending(x => x.Created);
                var gmailPremium = await AsyncExecuter.FirstOrDefaultAsync(query);
                if (gmailPremium != null)
                {
                    var res = ObjectMapper.Map<GmailPremium, GmailPremiumDto>(gmailPremium);
                    gmailPremium.Status = Enums.GmailPremiumStatus.Completed;
                    gmailPremium.TakenTime = DateTime.Now;
                    gmailPremium.Updated = DateTime.Now;
                    await Repository.UpdateAsync(gmailPremium, autoSave: true);
                    return res;
                }
                return new GmailPremiumDto();
            }
            finally
            {
                getSyncLock.Release();
            }
        }

        private bool ValidateGmailPremiumInput(string str)
        {
            return Regex.IsMatch(str, @"^(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\|(.+)");

        }
    }
}
