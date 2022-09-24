using GmailServer.AppleIds;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Extensions;
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
    public class AppleIdAppService : CrudAppService<
        AppleId,
        AppleIdDto,
        long,
        AppleIdFilterDto,
        CreateUpdateAppleIdDto,
        CreateUpdateAppleIdDto>, IAppleIdAppService
    {
        private new readonly IAppleIdRepository Repository;

        public AppleIdAppService(IAppleIdRepository repository) : base(repository)
        {
            Repository = repository;
            GetPolicyName = GmailServerPermissions.AppleIds.Default;
            GetListPolicyName = GmailServerPermissions.AppleIds.Default;
            UpdatePolicyName = GmailServerPermissions.AppleIds.Update;
            DeletePolicyName = GmailServerPermissions.AppleIds.Delete;
        }

        public override async Task<PagedResultDto<AppleIdDto>> GetListAsync(AppleIdFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == x.Username);

            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);

            var res = ObjectMapper.Map<List<AppleId>, List<AppleIdDto>>(entities);

            return new PagedResultDto<AppleIdDto>(count, res);
        }

        public async override Task<AppleIdDto> CreateAsync(CreateUpdateAppleIdDto input)
        {
            var appleId = ObjectMapper.Map<CreateUpdateAppleIdDto, AppleId>(input);
            appleId.Created = DateTime.Now;
            appleId.Status = Enums.AppleIdStatus.Ready;
            var res = await Repository.InsertAsync(appleId, autoSave: true);

            return await MapToGetOutputDtoAsync(res);
        }

        [Authorize(GmailServerPermissions.AppleIds.Create)]
        public async Task CreateManyAsync(CreateManyAppleIdInputDto input)
        {
            var appleIds = input.Emails.Split("\r\n").ToList();
            if (appleIds.Count == 0)
                throw new UserFriendlyException("Input empty!");
            var entities = new List<AppleId>();
            foreach (var gp in appleIds)
            {
                if (ValidateAppleIdInput(gp))
                {
                    var gpSplit = gp.Split('|').ToArray();
                    var hasEmail = await Repository.AnyAsync(x => x.Email == gpSplit[0]);
                    if (!hasEmail)
                    {
                        var entity = new AppleId()
                        {
                            Username = input.Username,
                            Email = gpSplit[0],
                            Password = gpSplit[1],
                            Status = Enums.AppleIdStatus.Ready,
                            Created = DateTime.Now
                        };
                        entities.Add(entity);
                    }
                }
            }

            if (entities.Count > 0)
            {
                await Repository.BulkInsertAsync(entities.DistinctBy(x => x.Email).ToList());
            }
        }

        [Authorize(GmailServerPermissions.AppleIds.Delete)]
        public async Task DeleteAllAsync()
        {
            await Repository.DeleteAllAsync();
        }

        public async Task<AppleIdDto> GetFirstAppleIdAsync()
        {
            var query = Repository.Where(x => x.Status == Enums.AppleIdStatus.Ready);
            query = query.OrderByDescending(x => x.Created);
            var appleId = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (appleId != null)
            {
                var res = ObjectMapper.Map<AppleId, AppleIdDto>(appleId);
                appleId.Status = Enums.AppleIdStatus.Pending;
                await Repository.UpdateAsync(appleId, autoSave: true);
                return res;
            }
            return new AppleIdDto();
        }

        private bool ValidateAppleIdInput(string str)
        {
            return Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\|(.+)");

        }

        public async Task<AppleIdDto> UpdateStatusAsync(string email, AppleIdStatus status)
        {
            var appleId = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (appleId != null)
            {
                appleId.Status = status;
                var res = await Repository.UpdateAsync(appleId);
                return await MapToGetOutputDtoAsync(res);
            }
            return new AppleIdDto();
        }
    }
}
