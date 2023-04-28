using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.MomoAccounts;
using GmailServer.MomoAccounts.Statistics;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class MomoAccountAppService : ReadOnlyAppService<
        MomoAccount,
        MomoAccountDto,
        long, MomoAccountFilterDto>, IMomoAccountAppService
    {
        private readonly new IMomoAccountRepository Repository;
        public MomoAccountAppService(IMomoAccountRepository repository) : base(repository)
        {
            Repository = repository;
        }

        [Authorize(GmailServerPermissions.MomoAccounts.Default)]
        public override async Task<PagedResultDto<MomoAccountDto>> GetListAsync(MomoAccountFilterDto input)
        {
            var query = Repository.AsQueryable();
            query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username)
                .WhereIf(!string.IsNullOrEmpty(input.Email), x => x.Email == input.Email)
                .WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value)
                .WhereIf(input.TotalLinkCountMax.HasValue, x => x.TotalLinkCount <= input.TotalLinkCountMax.Value)
                .WhereIf(input.TotalLinkCountMin.HasValue, x => x.TotalLinkCount >= input.TotalLinkCountMin.Value)
                .WhereIf(input.CreatedTimeFrom.HasValue, x => x.CreatedTime >= input.CreatedTimeFrom.Value)
                .WhereIf(input.CreatedTimeTo.HasValue, x => x.CreatedTime <= input.CreatedTimeTo.Value);

            var count = await AsyncExecuter.CountAsync(query);
            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);
            var entities = await AsyncExecuter.ToListAsync(query);
            var res = ObjectMapper.Map<List<MomoAccount>, List<MomoAccountDto>>(entities);

            return new PagedResultDto<MomoAccountDto>(count, res);
        }

        [Authorize(GmailServerPermissions.MomoAccounts.Default)]
        public override Task<MomoAccountDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        public async Task<MomoAccountDto> GetMomoAcountAsync()
        {
            var query = Repository.Where(x => x.Status == MomoAccountStatus.NotUse)
                .OrderBy(x => x.LastUpdateTime);
            var momoAccount = await AsyncExecuter.FirstOrDefaultAsync(query);

            if (momoAccount != null)
            {
                var res = ObjectMapper.Map<MomoAccount, MomoAccountDto>(momoAccount);
                momoAccount.Status = MomoAccountStatus.InUse;
                momoAccount.LastTakenTime = DateTime.Now;
                momoAccount.LastUpdateTime = DateTime.Now;
                return res;
            }
            return null;
        }

        [Authorize]
        public async Task<List<MomoAccountStatusSelectionDto>> GetMomoAcountStatusSelectionsAsync(string uploadGroup, DateTime? createdFrom, DateTime? createdTo)
        {
            var query = Repository.WhereIf(!string.IsNullOrEmpty(uploadGroup), x => x.UploadGroup == uploadGroup)
                .WhereIf(createdFrom.HasValue, x => x.CreatedTime.Date >= createdFrom.Value.Date)
                .WhereIf(createdTo.HasValue, x => x.CreatedTime.Date <= createdTo.Value.Date);
            var group = query.GroupBy(x => x.Status).Select(x => new MomoAccountStatusSelectionDto()
            {
                Text = $"{x.Key.ToString()} | {x.Count()}",
                Value = x.Key
            });
            var res = await AsyncExecuter.ToListAsync(group);
            return res;
        }

        [Authorize(GmailServerPermissions.MomoAccounts.Statistic)]
        public async Task<PagedResultDto<MomoAccountStatisticDto>> GetStatisticAsync(MomoAccountStatisticFilterDto input)
        {
            var query = Repository.WhereIf(!string.IsNullOrEmpty(input.UploadGroup), x => x.UploadGroup == input.UploadGroup)
               .WhereIf(input.CreatedTimeForm.HasValue, x => x.CreatedTime.Date >= input.CreatedTimeForm.Value.Date)
               .WhereIf(input.CreatedTimeTo.HasValue, x => x.CreatedTime.Date <= input.CreatedTimeTo.Value.Date);
            var group = query.GroupBy(x => new { CreatedTime = x.CreatedTime.Date, UploadGroup = x.UploadGroup })
                .Select(g => new MomoAccountStatisticDto()
                {
                    CreatedTime = g.Key.CreatedTime.Date,
                    UploadGroup = g.Key.UploadGroup,
                    Total = g.Count(),
                    NotUse = g.Where(x => x.Status == MomoAccountStatus.NotUse).Count(),
                    InUse = g.Where(x => x.Status == MomoAccountStatus.InUse).Count(),
                    Lock = g.Where(x => x.Status == MomoAccountStatus.Lock).Count(),
                    WrongPassword = g.Where(x => x.Status == MomoAccountStatus.WrongPassword).Count()
                });
            var count = await AsyncExecuter.CountAsync(group);
            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                group = group.Skip(input.SkipCount).Take(input.MaxResultCount);
            var res = await AsyncExecuter.ToListAsync(group);
            return new PagedResultDto<MomoAccountStatisticDto>(
                count, res.OrderByDescending(x => x.CreatedTime).ToList());
        }

        public async Task<MomoAccountDto> CreateAsync(CreateMomoAccountInputDto input)
        {
            var momoAccount = ObjectMapper.Map<CreateMomoAccountInputDto, MomoAccount>(input);
            momoAccount.CreatedTime = DateTime.Now;
            momoAccount.Status = MomoAccountStatus.NotUse;
            try
            {
                var entity = await Repository.InsertAsync(momoAccount, autoSave: true);
                var res = ObjectMapper.Map<MomoAccount, MomoAccountDto>(entity);
                return res;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize(GmailServerPermissions.MomoAccounts.CreateMany)]
        public async Task CreateManyAsync(CreateManyMonoAccountInputDto input)
        {
            var accounts = input.Accounts.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            if (accounts.Count == 0)
                throw new UserFriendlyException("Input empty!");
            var entities = new List<MomoAccount>();
            foreach (var account in accounts)
            {
                if (!string.IsNullOrEmpty(account))
                {
                    var accountSplit = account.Split('|', StringSplitOptions.RemoveEmptyEntries).ToArray();
                    if (accountSplit.Length >= 2)
                    {
                        var username = accountSplit[0].Trim();
                        var hasEmail = await Repository.AnyAsync(x => x.Username == username);
                        if (!hasEmail)
                        {
                            var entity = new MomoAccount()
                            {
                                UploadGroup = input.UploadGroup,
                                Username = username,
                                Password = accountSplit[1].Trim(),
                                Status = MomoAccountStatus.NotUse,
                                CreatedTime = DateTime.Now
                            };
                            entity.Email = accountSplit.Length >= 3 ? accountSplit[2].Trim().ToLower() : string.Empty;
                            entities.Add(entity);
                        }
                    }
                }
            }
            if (entities.Count > 0)
            {
                await Repository.BulkInsertAsync(entities.DistinctBy(x => x.Username).ToList());
            }
        }

        public async Task<MomoAccountDto> UpdateMomoAcountAsync(string username, UpdateMomoAccountInputDto input)
        {
            var query = Repository.Where(x => x.Username == username);
            var momoAccount = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (momoAccount != null)
            {
                momoAccount.Password = input.Password;
                momoAccount.Email = input.Email;
                momoAccount.Status = input.Status;
                momoAccount.UDid1 = input.UDid1;
                momoAccount.UDid2 = input.UDid2;
                momoAccount.RefreshToken = input.RefreshToken;
                momoAccount.AuthenticateToken = input.AuthenticateToken;
                momoAccount.SessionKey = input.SessionKey;
                momoAccount.SessionKey2 = input.SessionKey2;
                momoAccount.SetupKey = input.SetupKey;
                momoAccount.CurrentLinkCount = input.CurrentLinkCount;
                momoAccount.TotalLinkCount = input.TotalLinkCount;
                momoAccount.CustmArg1 = input.CustmArg1;
                momoAccount.CustmArg2 = input.CustmArg2;
                momoAccount.CustmArg3 = input.CustmArg3;
                var entity = await Repository.UpdateAsync(momoAccount, true);
                return ObjectMapper.Map<MomoAccount, MomoAccountDto>(entity);
            }
            return null;
        }

        [Authorize(GmailServerPermissions.MomoAccounts.DeleteAll)]
        public async Task DeleteAllAsync()
        {
            await Repository.DeleteAllAsync();
        }

        [Authorize(GmailServerPermissions.MomoAccounts.DeleteFilter)]
        public async Task DeleteFilterAsync(DeleteFilterInput input)
        {
            if (input.Statuses.Count > 0)
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("DELETE FROM AppMomoAccounts WHERE ");
                queryBuilder.Append($"Status IN ({string.Join(",", input.Statuses.Select(x => (int)x).ToArray())}) ");
                if (!string.IsNullOrEmpty(input.UploadGroup))
                {
                    queryBuilder.Append($"And UploadGroup = '{input.UploadGroup}' ");
                }
                if (input.CreatedTimeFrom.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, CreatedTime) >= '{input.CreatedTimeFrom.Value.Date.ToString("yyyy-MM-dd")}' ");
                }
                if (input.CreatedTimeTo.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, CreatedTime) <= '{input.CreatedTimeTo.Value.Date.ToString("yyyy-MM-dd")}' ");
                }
                var query = queryBuilder.ToString();
                try
                {
                    await Repository.ExecuteSqlRawAsync(query);
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);
                }
            }
            else 
                throw new UserFriendlyException("The status filter is required");
        }

        [Authorize(GmailServerPermissions.MomoAccounts.Delete)]
        public async Task DeleteAsync(long id)
        {
            await Repository.DeleteAsync(id);
        }

        [Authorize(GmailServerPermissions.MomoAccounts.ResetStatus)]
        public async Task ResetStatusAsync(ResetStatusFilterInput input)
        {
            if (input.Statuses.Count > 0)
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("Update AppMomoAccounts");
                queryBuilder.AppendLine($"Set Status = {(int)input.TargetStatus}, LastUpdateTime = GETDATE()");
                queryBuilder.AppendLine($"Where ");
                queryBuilder.Append($"Status IN ({string.Join(",", input.Statuses.Select(x => (int)x).ToArray())}) ");

                if (!string.IsNullOrEmpty(input.UploadGroup))
                {
                    queryBuilder.Append($"And Username = '{input.UploadGroup}' ");
                }
                if (input.CreatedTimeFrom.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, Created) >= '{input.CreatedTimeFrom.Value.Date.ToString("yyyy-MM-dd")}' ");
                }
                if (input.CreatedTimeTo.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, Created) <= '{input.CreatedTimeTo.Value.Date.ToString("yyyy-MM-dd")}' ");
                }

                string query = queryBuilder.ToString();
                try
                {
                    await Repository.ExecuteSqlRawAsync(query);
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);
                }
            }
            else 
                throw new UserFriendlyException("The status filter is required");
        }
    }
}
