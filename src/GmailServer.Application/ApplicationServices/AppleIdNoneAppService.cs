using GmailServer.AppleIdNones;
using GmailServer.AppleIdNones.Statistics;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class AppleIdNoneAppService : CrudAppService<
        AppleIdNone,
        AppleIdNoneGetOutputDto,
        AppleIdNoneGetListOutputDto,
        long,
        AppleIdNoneFilterDto,
        CreateUpdateAppleIdNoneDto,
        CreateUpdateAppleIdNoneDto>, IAppleIdNoneAppService
    {
        private readonly new IAppleIdNoneRepository Repository;
        private static SemaphoreSlim getSyncLock = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim getByStatusSyncLock = new SemaphoreSlim(1, 1);

        public AppleIdNoneAppService(IAppleIdNoneRepository repository) : base(repository)
        {
            Repository = repository;
        }

        [Authorize(GmailServerPermissions.AppleIdNones.Default)]
        public async override Task<PagedResultDto<AppleIdNoneGetListOutputDto>> GetListAsync(AppleIdNoneFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(!string.IsNullOrEmpty(input.Email), x => x.Email == input.Email.ToLower().Trim());
            query = query.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
            query = query.WhereIf(input.AddPaymentCompleted.HasValue, x => x.AddPaymentCompleted == input.AddPaymentCompleted.Value);
            query = query.WhereIf(input.RemovePaymentStatus.HasValue, x => x.RemovePaymentStatus == input.RemovePaymentStatus.Value);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(input.PurchaseNumberMax.HasValue, x => x.PurchaseNumber <= input.PurchaseNumberMax.Value);
            query = query.WhereIf(input.PurchaseNumberMin.HasValue, x => x.PurchaseNumber >= input.PurchaseNumberMin.Value);
            query = query.WhereIf(input.TakenOutNumberMin.HasValue, x => x.TakenOutNumber >= input.TakenOutNumberMin.Value);
            query = query.WhereIf(input.TakenOutNumberMax.HasValue, x => x.TakenOutNumber <= input.TakenOutNumberMax.Value);

            var currentUser = CurrentUser;
            if (currentUser.IsInRole(RoleName.RoleNameAppleIdMember))
            {
                query = query.Where(x => x.Username == currentUser.UserName);
            }
            else
            {
                query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
            }

            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);

            var res = ObjectMapper.Map<List<AppleIdNone>, List<AppleIdNoneGetListOutputDto>>(entities);

            return new PagedResultDto<AppleIdNoneGetListOutputDto>(count, res);
        }

        [Authorize(GmailServerPermissions.AppleIdNones.Default)]
        public override Task<AppleIdNoneGetOutputDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        [Authorize(GmailServerPermissions.AppleIdNones.Download)]
        public async Task<List<AppleIdNoneExcelModel>> GetAppleIdNoneExcelModelsAsync(AppleIdNoneDownloadFilter input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
            if (input.Statuses.Count > 0)
            {
                query = query.Where(x => input.Statuses.Contains(x.Status));
            }
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);

            var res = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<AppleIdNone>, List<AppleIdNoneExcelModel>>(res);
        }

        [Authorize]
        public async Task<List<AppleIdNoneStatusSelectionDto>> GetAppleIdNoneStatusSelectionsAsync(string username, DateTime? createdFrom, DateTime? createdTo)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(!string.IsNullOrEmpty(username), x => x.Username == username);
            query = query.WhereIf(createdFrom.HasValue, x => x.Created.Date >= createdFrom.Value.Date);
            query = query.WhereIf(createdTo.HasValue, x => x.Created.Date <= createdTo.Value.Date);
            var groupBy = query.GroupBy(x => x.Status).Select(x => new AppleIdNoneStatusSelectionDto()
            {
                Text = $"{x.Key.ToString()} | {x.Count()}",
                Value = x.Key
            });
            var res = await AsyncExecuter.ToListAsync(groupBy);
            return res;
        }

        public async Task<AppleIdNoneGetOutputDto> GetByStatusAsync(AppleIdNoneStatus status)
        {
            await getByStatusSyncLock.WaitAsync();
            try
            {
                var query = Repository.Where(x => x.Status == status);
                query = query.OrderBy(x => x.TakenTime);
                var appleIdNone = await AsyncExecuter.FirstOrDefaultAsync(query);
                if (appleIdNone != null)
                {
                    var res = ObjectMapper.Map<AppleIdNone, AppleIdNoneGetOutputDto>(appleIdNone);
                    appleIdNone.TakenTime = DateTime.Now;
                    //appleId.TakenOutNumber += 1;
                    await Repository.UpdateAsync(appleIdNone, autoSave: true);
                    return res;
                }
                return null;
            }
            finally
            {
                getByStatusSyncLock.Release(); ;
            }
        }

        public async Task<AppleIdNoneGetOutputDto> GetFirstAppleIdNoneAsync(bool isNone = false)
        {
            await getSyncLock.WaitAsync();
            try
            {
                var query = Repository.Where(x => x.AddPaymentCompleted == false);
                if (isNone)
                {
                    query = query.Where(x => x.PurchaseNumber == 0);
                }
                else
                {
                    query = query.Where(x => x.Status == Enums.AppleIdNoneStatus.Ready);
                    query = query.Where(x => x.TakenTime == DateTime.Parse("0001-01-01 00:00:00.0000000"));
                }
                query = query.OrderBy(x => x.Updated);

                var appleId = await AsyncExecuter.FirstOrDefaultAsync(query);
                if (appleId == null)
                {
                    var query2 = Repository
                        .Where(x => x.Status == Enums.AppleIdNoneStatus.Ready && x.AddPaymentCompleted == false)
                        .OrderBy(x => x.TakenTime);
                    appleId = await AsyncExecuter.FirstOrDefaultAsync(query2);
                }

                if (appleId != null)
                {
                    var res = ObjectMapper.Map<AppleIdNone, AppleIdNoneGetOutputDto>(appleId);
                    appleId.Status = Enums.AppleIdNoneStatus.Pending;
                    appleId.TakenTime = DateTime.Now;
                    appleId.Updated = DateTime.Now;
                    await Repository.UpdateAsync(appleId, autoSave: true);
                    return res;
                }
                return null;
            }
            finally
            {
                getSyncLock.Release();
            }
        }

        [Authorize(GmailServerPermissions.AppleIdNones.Statistic)]
        public async Task<PagedResultDto<AppleIdNoneStatisticDto>> GetStatisticAsync(AppleIdNoneStatisticFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
            var queryGroupBy = query.GroupBy(x => new { Created = x.Created.Date, Username = x.Username }).Select(g => new AppleIdNoneStatisticDto()
            {
                Created = g.Key.Created.Date,
                Username = g.Key.Username,
                Total = g.Count(),
                TotalPurchaseNumber = g.Sum(x => x.PurchaseNumber),
                Ready = g.Where(x => x.Status == AppleIdNoneStatus.Ready).Count(),
                Completed1 = g.Where(x => x.Status == AppleIdNoneStatus.Completed1).Count(),
                Completed2 = g.Where(x => x.Status == AppleIdNoneStatus.Completed2).Count(),
                Completed3 = g.Where(x => x.Status == AppleIdNoneStatus.Completed3).Count(),
                Completed4 = g.Where(x => x.Status == AppleIdNoneStatus.Completed4).Count(),
                Pending = g.Where(x => x.Status == AppleIdNoneStatus.Pending).Count(),
                WrongPass = g.Where(x => x.Status == AppleIdNoneStatus.WrongPass).Count(),
                Subed = g.Where(x => x.Status == AppleIdNoneStatus.Subed).Count(),
                Locked1 = g.Where(x => x.Status == AppleIdNoneStatus.Locked1).Count(),
                Locked2 = g.Where(x => x.Status == AppleIdNoneStatus.Locked2).Count(),
                Review = g.Where(x => x.Status == AppleIdNoneStatus.Review).Count(),
                Error = g.Where(x => x.Status == AppleIdNoneStatus.Error).Count(),
                Unknown = g.Where(x => x.Status == AppleIdNoneStatus.Unknown).Count()
            });

            var count = await AsyncExecuter.CountAsync(queryGroupBy);
            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                queryGroupBy = queryGroupBy.Skip(input.SkipCount).Take(input.MaxResultCount);

            var res = await AsyncExecuter.ToListAsync(queryGroupBy);
            return new PagedResultDto<AppleIdNoneStatisticDto>(count, res.OrderByDescending(x => x.Created).ToList());
        }

        [Authorize]
        public async Task<List<UsernameSelectionDto>> GetUsernameSelectionAsync()
        {
            var query = Repository.GroupBy(x => x.Username).Select(x => new UsernameSelectionDto()
            {
                Text = x.Key,
                Value = x.Key
            });
            var res = await AsyncExecuter.ToListAsync(query);
            return res;
        }

        public async Task<AppleIdNoneGetOutputDto> IncreasePurchaseAsync(string email)
        {
            var appleIdNone = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (appleIdNone != null)
            {
                appleIdNone.PurchaseNumber += 1;
                appleIdNone.Updated = DateTime.Now;
                await Repository.UpdateAsync(appleIdNone);
                return await MapToGetOutputDtoAsync(appleIdNone);
            }
            return null;
        }

        public async Task<AppleIdNoneGetOutputDto> SetTakenOutNumberAsync(string email, int value)
        {
            var appleId = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (appleId != null)
            {
                appleId.TakenOutNumber = value;
                appleId.Updated = DateTime.Now;
                await Repository.UpdateAsync(appleId);
                return await MapToGetOutputDtoAsync(appleId);
            }
            return null;
        }

        public async override Task<AppleIdNoneGetOutputDto> CreateAsync(CreateUpdateAppleIdNoneDto input)
        {
            if (CommonMethod.IsValidEmail(input.Email))
            {
                var appleIdNone = ObjectMapper.Map<CreateUpdateAppleIdNoneDto, AppleIdNone>(input);
                appleIdNone.Created = DateTime.Now;
                appleIdNone.Status = Enums.AppleIdNoneStatus.Ready;
                appleIdNone.RemovePaymentStatus = RemovePaymentStatus.Ready;
                appleIdNone.AddPaymentCompleted = false;
                appleIdNone.PurchaseNumber = 0;
                appleIdNone.TakenOutNumber = 0;
                var res = await Repository.InsertAsync(appleIdNone, autoSave: true);
                return await MapToGetOutputDtoAsync(res);
            }
            else
            {
                throw new UserFriendlyException($"{input.Email} - invalidate!");
            }
        }

        [Authorize(GmailServerPermissions.AppleIdNones.Create)]
        public async Task CreateManyAsync(CreateManyAppleIdNoneInputDto input)
        {
            var appleIds = input.Emails.Split("\r\n").ToList();
            if (appleIds.Count == 0)
                throw new UserFriendlyException("Input empty!");
            var entities = new List<AppleIdNone>();
            foreach (var appleId in appleIds)
            {
                if (ValidateAppleIdInput(appleId))
                {
                    var appleIdSplit = appleId.Split('|').ToArray();
                    var email = appleIdSplit[0].ToLower();
                    var hasEmail = await Repository.AnyAsync(x => x.Email == email);
                    if (!hasEmail)
                    {
                        var entity = new AppleIdNone()
                        {
                            Username = input.Username,
                            Email = email,
                            Password = appleIdSplit[1],
                            Status = AppleIdNoneStatus.Ready,
                            RemovePaymentStatus = RemovePaymentStatus.Ready,
                            Created = DateTime.Now,
                            PurchaseNumber = 0,
                            TakenOutNumber = 0
                        };
                        entity.SecretAnswer1 = appleIdSplit.Length >= 3 ? appleIdSplit[2] : null;
                        entity.SecretAnswer2 = appleIdSplit.Length >= 4 ? appleIdSplit[3] : null;
                        entity.SecretAnswer3 = appleIdSplit.Length >= 5 ? appleIdSplit[4] : null;
                        entity.DateOfBirth = appleIdSplit.Length >= 6 ? appleIdSplit[5] : null;
                        entities.Add(entity);
                    }
                }
            }

            if (entities.Count > 0)
            {
                await Repository.BulkInsertAsync(entities.DistinctBy(x => x.Email).ToList());
            }
        }

        private bool ValidateAppleIdInput(string str)
        {
            return Regex.IsMatch(str, @"^(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\|(.+)");

        }

        public async Task<AppleIdNoneGetOutputDto> UpdateStatusAsync(string email, AppleIdNoneStatus status)
        {
            var appleIdNone = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (appleIdNone != null)
            {
                if ((status == AppleIdNoneStatus.Ready && appleIdNone.Status == AppleIdNoneStatus.Pending) ||
                    (status != AppleIdNoneStatus.Ready && appleIdNone.Status != AppleIdNoneStatus.Completed1
                        && appleIdNone.Status != AppleIdNoneStatus.Completed2
                        && appleIdNone.Status != AppleIdNoneStatus.Completed3
                        && appleIdNone.Status != AppleIdNoneStatus.Completed4))
                {
                    appleIdNone.Status = status;
                    appleIdNone.Updated = DateTime.Now;
                    var res = await Repository.UpdateAsync(appleIdNone);
                    return await MapToGetOutputDtoAsync(res);
                }
            }
            return null;
        }

        [Authorize(GmailServerPermissions.AppleIdNones.DeleteAll)]
        public async Task DeleteAllAsync()
        {
            await Repository.DeleteAllAsync();
        }

        [Authorize(GmailServerPermissions.AppleIdNones.DeleteFilter)]
        public async Task DeleteAsync(DeleteFilter input)
        {
            if (input.Statuses.Count > 0)
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("DELETE FROM AppAppleIdNones WHERE ");
                queryBuilder.Append($"Status IN ({string.Join(",", input.Statuses.Select(x => (int)x).ToArray())}) ");

                if (!string.IsNullOrEmpty(input.Username))
                {
                    queryBuilder.Append($"And Username = '{input.Username}' ");
                }
                if (input.CreatedFrom.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, Created) >= '{input.CreatedFrom.Value.Date.ToString("yyyy-MM-dd")}' ");
                }
                if (input.CreatedTo.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, Created) <= '{input.CreatedTo.Value.Date.ToString("yyyy-MM-dd")}' ");
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

        [Authorize(GmailServerPermissions.AppleIdNones.ResetStatus)]
        public async Task ResetStatusAsync(ResetStatusFilter input)
        {
            if (input.Statuses.Count > 0)
            {

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("Update AppAppleIdNones");
                queryBuilder.AppendLine($"Set Status = {(int)input.TargetStatus}, TakenOutNumber = 0, Updated = GETDATE()");
                queryBuilder.AppendLine($"Where ");
                queryBuilder.Append($"Status IN ({string.Join(",", input.Statuses.Select(x => (int)x).ToArray())}) ");

                if (!string.IsNullOrEmpty(input.Username))
                {
                    queryBuilder.Append($"And Username = '{input.Username}' ");
                }
                if (input.CreatedFrom.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, Created) >= '{input.CreatedFrom.Value.Date.ToString("yyyy-MM-dd")}' ");
                }
                if (input.CreatedTo.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, Created) <= '{input.CreatedTo.Value.Date.ToString("yyyy-MM-dd")}' ");
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

        public async Task<AppleIdNoneGetOutputDto> AddPaymentCompletedAsync(string email)
        {
            var appleIdNone = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (appleIdNone != null)
            {
                appleIdNone.AddPaymentCompleted = true;
                var entity = await Repository.UpdateAsync(appleIdNone, true);
                return await MapToGetOutputDtoAsync(entity);
            }
            return null;
        }

        public async Task<AppleIdNoneGetOutputDto> UpdateRemoveStatusAsync(string email, RemovePaymentStatus status)
        {
            var appleIdNone = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (appleIdNone != null)
            {
                appleIdNone.RemovePaymentStatus = status;
                appleIdNone.RemoveUpdateTime = DateTime.Now;
                var entity = await Repository.UpdateAsync(appleIdNone, true);
                return await MapToGetOutputDtoAsync(entity);
            }
            return null;
        }

        public async Task<AppleIdNoneGetOutputDto> GetAppleIdToRemoveAsync()
        {
            var query = Repository.Where(x => x.AddPaymentCompleted == true
                && x.Status != AppleIdNoneStatus.Pending
                && x.RemovePaymentStatus == RemovePaymentStatus.Ready);
            var appleIdNone = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (appleIdNone != null)
            {
                var res = await MapToGetOutputDtoAsync(appleIdNone);
                appleIdNone.RemovePaymentStatus = RemovePaymentStatus.InUse;
                appleIdNone.RemoveTakenTime = DateTime.Now;
                await Repository.UpdateAsync(appleIdNone, true);
                return res;
            }
            return null;
        }

        [Authorize]
        public async Task<List<AppleIdNoneRemoveStatusSelectionDto>> GetAppleIdNoneRemoveStatusSelectionsAsync(
            string username, 
            DateTime? createdFrom, 
            DateTime? createdTo, 
            DateTime? removeTakenTimeFrom, 
            DateTime? removeTakenTimeTo)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(!string.IsNullOrEmpty(username), x => x.Username == username);
            query = query.WhereIf(createdFrom.HasValue, x => x.Created.Date >= createdFrom.Value.Date);
            query = query.WhereIf(createdTo.HasValue, x => x.Created.Date <= createdTo.Value.Date);
            query = query.WhereIf(removeTakenTimeFrom.HasValue, x => x.RemoveTakenTime.Date >= removeTakenTimeFrom.Value.Date);
            query = query.WhereIf(removeTakenTimeTo.HasValue, x => x.RemoveTakenTime.Date <= removeTakenTimeTo.Value.Date);
            var groupBy = query.GroupBy(x => x.RemovePaymentStatus).Select(x => new AppleIdNoneRemoveStatusSelectionDto()
            {
                Text = $"{x.Key.ToString()} | {x.Count()}",
                Value = x.Key
            });
            var res = await AsyncExecuter.ToListAsync(groupBy);
            return res;
        }

        [Authorize(GmailServerPermissions.AppleIdNones.ResetRemovePaymentStatus)]
        public async Task ResetRemovePaymentStatusAsync(ResetRemovePaymentStatusFilter input)
        {
            if (input.Statuses.Count > 0)
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("Update AppAppleIdNones");
                queryBuilder.AppendLine($"Set RemovePaymentStatus = {(int)input.TargetStatus}, RemoveUpdateTime = GETDATE()");
                queryBuilder.AppendLine($"Where ");
                queryBuilder.Append($"RemovePaymentStatus IN ({string.Join(",", input.Statuses.Select(x => (int)x).ToArray())}) ");

                if (!string.IsNullOrEmpty(input.Username))
                {
                    queryBuilder.Append($"And Username = '{input.Username}' ");
                }
                if (input.CreatedFrom.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, Created) >= '{input.CreatedFrom.Value.Date.ToString("yyyy-MM-dd")}' ");
                }
                if (input.CreatedTo.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, Created) <= '{input.CreatedTo.Value.Date.ToString("yyyy-MM-dd")}' ");
                }
                if (input.RemoveTakenTimeFrom.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, RemoveTakenTime) >= '{input.RemoveTakenTimeFrom.Value.Date.ToString("yyyy-MM-dd")}' ");
                }
                if (input.RemoveTakenTimeTo.HasValue)
                {
                    queryBuilder.Append($"And CONVERT(DATE, RemoveTakenTime) <= '{input.RemoveTakenTimeTo.Value.Date.ToString("yyyy-MM-dd")}' ");
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
