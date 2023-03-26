using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.GmailResources;
using GmailServer.GmailResources.Statistics;
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
using Volo.Abp.Domain.Entities;
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
        private static SemaphoreSlim getSyncLock = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim getByStatusSyncLock = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim getPremiumSyncLock = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim getPremiumByNumberSyncLock = new SemaphoreSlim(1, 1);

        public GmailResourceAppService(IGmailResourceRepository repository) : base(repository)
        {
            Repository = repository;

            GetPolicyName = GmailServerPermissions.GmailResources.Default;
            GetListPolicyName = GmailServerPermissions.GmailResources.Default;
            CreatePolicyName = GmailServerPermissions.GmailResources.Create;
            UpdatePolicyName = GmailServerPermissions.GmailResources.Update;
            DeletePolicyName = GmailServerPermissions.GmailResources.Delete;
        }

        [Authorize(GmailServerPermissions.GmailResources.Default)]
        public async override Task<PagedResultDto<GmailResourceDto>> GetListAsync(GmailResourceFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
            query = query.WhereIf(input.PremiumType.HasValue, x => x.PremiumType == input.PremiumType.Value);
            query = query.WhereIf(!string.IsNullOrEmpty(input.Email), x => x.Email == input.Email.ToLower().Trim());
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(!string.IsNullOrEmpty(input.Country), x => x.Country == input.Country.ToLower().Trim());
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

            var res = ObjectMapper.Map<List<GmailResource>, List<GmailResourceDto>>(entities);

            return new PagedResultDto<GmailResourceDto>(count, res);
        }

        [Authorize(GmailServerPermissions.GmailResources.Default)]
        public override Task<GmailResourceDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        [Authorize(GmailServerPermissions.GmailResources.Update)]
        public override Task<GmailResourceDto> UpdateAsync(long id, CreateUpdateGmailResourceDto input)
        {
            return base.UpdateAsync(id, input);
        }

        public async override Task<GmailResourceDto> CreateAsync(CreateUpdateGmailResourceDto input)
        {
            if (CommonMethod.IsValidEmail(input.Email))
            {
                var gmailResource = ObjectMapper.Map<CreateUpdateGmailResourceDto, GmailResource>(input);
                gmailResource.Created = DateTime.Now;
                //gmailResource.Updated = DateTime.Parse("0001-01-01 00:00:00.0000000");
                gmailResource.PremiumType = PremiumType.Unset;
                //gmailResource.UpdatedPremium = DateTime.Parse("0001-01-01 00:00:00.0000000");
                gmailResource.Status = Enums.GmailResourceStatus.Ready;
                var res = await Repository.InsertAsync(gmailResource, autoSave: true);

                return await MapToGetOutputDtoAsync(res);
            }
            else
            {
                throw new UserFriendlyException($"{input.Email} - invalidate!");
            }

        }

        [Authorize(GmailServerPermissions.GmailResources.Create)]
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
                    var email = gpSplit[0].Trim().ToLower();
                    var hasEmail = await Repository.AnyAsync(x => x.Email == email);
                    if (!hasEmail)
                    {
                        var entity = new GmailResource()
                        {
                            Username = input.Username,
                            Email = email,
                            Password = gpSplit[1].Trim(),
                            Status = Enums.GmailResourceStatus.Ready,
                            Created = DateTime.Now,
                            PremiumType = PremiumType.Unset
                            //Updated = DateTime.Parse("0001-01-01 00:00:00.0000000"),
                            //UpdatedPremium = DateTime.Parse("0001-01-01 00:00:00.0000000")
                        };
                        entity.RecoveryEmail = gpSplit.Length >= 3 ? gpSplit[2].Trim().ToLower() : string.Empty;
                        entity.Country = gpSplit.Length >= 4 ? gpSplit[3].Trim().ToLower() : string.Empty;
                        entities.Add(entity);
                    }
                }
            }

            if (entities.Count > 0)
            {
                await Repository.BulkInsertAsync(entities.DistinctBy(x => x.Email).ToList());
            }
        }

        [Authorize(GmailServerPermissions.GmailResources.ReupEmail)]
        public async Task<List<ReupOutputDto>> ReupAsync(ReupGmailResourceInputDto input)
        {
            var gmailResources = input.Emails.Split("\r\n").ToList();
            if (gmailResources.Count == 0)
                throw new UserFriendlyException("Input empty!");
            var reupOutputs = new List<ReupOutputDto>();
            var entities = new List<GmailResource>();
            foreach (var gr in gmailResources)
            {
                if (ValidateGmailResourceInput(gr))
                {
                    var gpSplit = gr.Split('|').ToArray();
                    var email = gpSplit[0].Trim().ToLower();
                    var oldEmail = await Repository.FirstOrDefaultAsync(x => x.Email == email);
                    if (oldEmail != null)
                    {
                        oldEmail.Username = input.Username;
                        oldEmail.Password = gpSplit[1].Trim();
                        oldEmail.Status = Enums.GmailResourceStatus.Ready;
                        oldEmail.Updated = DateTime.Now;
                        oldEmail.TakenTime = DateTime.Parse("0001-01-01 00:00:00.0000000");
                        oldEmail.PremiumType = PremiumType.Unset;
                        oldEmail.UpdatedPremium = DateTime.Now;
                        oldEmail.RecoveryEmail = gpSplit.Length >= 3 ? gpSplit[2].Trim().ToLower() : string.Empty;
                        oldEmail.Country = gpSplit.Length >= 4 ? gpSplit[3].Trim().ToLower() : string.Empty;
                        entities.Add(oldEmail);
                    }
                    else
                    {
                        reupOutputs.Add(new ReupOutputDto()
                        {
                            Email = email,
                            Password = gpSplit[1].Trim(),
                            RecoveryEmail = gpSplit.Length >= 3 ? gpSplit[2].Trim().ToLower() : string.Empty,
                            Country = gpSplit.Length >= 4 ? gpSplit[3].Trim().ToLower() : string.Empty,
                            OutputType = "NotInDB",
                            ReupStatus = "NA"
                        });
                    }
                }
            }

            if (entities.Count > 0)
            {
                try
                {
                    var duplicates = entities.GetDuplicates(x => x.Email, null).Select(x => new ReupOutputDto()
                    {
                        Email = x.Email,
                        Password = x.Password,
                        RecoveryEmail = x.RecoveryEmail,
                        Country = x.Country,
                        OutputType = "Duplicated",
                        ReupStatus = "Done"
                    }).ToList();

                    if (duplicates.Count > 0)
                    {
                        reupOutputs.AddRange(duplicates);
                    }

                    await Repository.BulkUpdateAsync(entities.DistinctBy(x => x.Email).ToList(), new List<string>()
                    {
                        nameof(GmailResource.Password),
                        nameof(GmailResource.Status),
                        nameof(GmailResource.Updated),
                        nameof(GmailResource.TakenTime),
                        nameof(GmailResource.PremiumType),
                        nameof(GmailResource.UpdatedPremium),
                        nameof(GmailResource.Country),
                        nameof(GmailResource.RecoveryEmail)
                    });
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);
                }
            }
            return reupOutputs;
        }

        [Authorize(GmailServerPermissions.GmailResources.DeleteAll)]
        public async Task DeleteAllAsync()
        {
            await Repository.DeleteAllAsync();
        }

        [Authorize(GmailServerPermissions.GmailResources.Delete)]
        public override Task DeleteAsync(long id)
        {
            return base.DeleteAsync(id);
        }

        public async Task<GmailResourceDto> GetFirstGmailResourceAsync()
        {
            await getSyncLock.WaitAsync();
            try
            {
                var query = Repository.Where(x => x.Status == Enums.GmailResourceStatus.Ready);
                query = query.Where(x => x.TakenTime == DateTime.Parse("0001-01-01 00:00:00.0000000"));
                query = query.OrderBy(x => x.Created);
                var gmailResource = await AsyncExecuter.FirstOrDefaultAsync(query);
                if (gmailResource == null)
                {
                    var query2 = Repository
                      .Where(x => x.Status == Enums.GmailResourceStatus.Ready)
                      .OrderBy(x => x.TakenTime);
                    gmailResource = await AsyncExecuter.FirstOrDefaultAsync(query2);
                }

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
            finally
            {
                getSyncLock.Release();
            }
        }

        private bool ValidateGmailResourceInput(string str)
        {
            return Regex.IsMatch(str, @"^(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\|(.+)");
        }

        public async Task<GmailResourceDto> UpdateStatusAsync(string email, GmailResourceStatus status)
        {
            var gmailResource = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (gmailResource != null && gmailResource.Status != GmailResourceStatus.Success)
            {
                gmailResource.Status = status;
                gmailResource.Updated = DateTime.Now;
                var res = await Repository.UpdateAsync(gmailResource);
                return await MapToGetOutputDtoAsync(res);
            }
            return new GmailResourceDto();

        }

        public async Task<GmailResourceDto> GetByStatusAsync(GmailResourceStatus status)
        {
            await getByStatusSyncLock.WaitAsync();
            try
            {
                var query = Repository.Where(x => x.Status == status);
                query = query.OrderBy(x => x.TakenTime);
                var gmailResource = await AsyncExecuter.FirstOrDefaultAsync(query);
                if (gmailResource != null)
                {
                    var res = ObjectMapper.Map<GmailResource, GmailResourceDto>(gmailResource);
                    gmailResource.TakenTime = DateTime.Now;
                    await Repository.UpdateAsync(gmailResource, autoSave: true);
                    return res;
                }
                return new GmailResourceDto();
            }
            finally
            {
                getByStatusSyncLock.Release();
            }
        }

        [Authorize]
        public async Task<List<string>> GetUsernameSelectionAsync()
        {
            var query = Repository.GroupBy(x => x.Username).Select(x => x.Key);
            var res = await AsyncExecuter.ToListAsync(query);
            return res;
        }

        [Authorize]
        public async Task<List<GmailResourceStatusSelectionDto>> GetGmailResourceStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo, int? updatedHours = null)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(!string.IsNullOrEmpty(username), x => x.Username == username);
            query = query.WhereIf(createdFrom.HasValue, x => x.Created.Date >= createdFrom.Value.Date);
            query = query.WhereIf(createdTo.HasValue, x => x.Created.Date <= createdTo.Value.Date);
            if (updatedHours.HasValue)
            {
                var current = DateTime.Now;
                var timeCheck = current.AddHours(-updatedHours.Value);
                query = query.Where(x => x.Updated < timeCheck);
            }
            var groupBy = query.GroupBy(x => x.Status).Select(x => new GmailResourceStatusSelectionDto()
            {
                Text = $"{x.Key.ToString()} | {x.Count()}",
                Value = x.Key
            });
            var res = await AsyncExecuter.ToListAsync(groupBy);
            return res;
        }

        [Authorize(GmailServerPermissions.GmailResources.Statistic)]
        public async Task<StatisticByUsernameDto> GetStatisticByUsernameAsync()
        {
            var query = Repository.AsQueryable();
            var total = await AsyncExecuter.CountAsync(query);
            var queryGroupByStatus = query.GroupBy(x => x.Username).Select(x => new StatusPoint()
            {
                Name = x.Key.ToString(),
                Y = x.Count()
            });
            var statusPoints = await AsyncExecuter.ToListAsync(queryGroupByStatus);
            statusPoints.OrderByDescending(x => x.Y).ToList();
            if (statusPoints.Count > 0)
            {
                statusPoints[0].Exploded = true;
            }
            return new StatisticByUsernameDto()
            {
                Total = total,
                StatusPoints = statusPoints
            };
        }

        [Authorize(GmailServerPermissions.GmailResources.Statistic)]
        public async Task<PagedResultDto<GmailResourceStatisticDto>> GetStatisticAsync(GmailResourceStatisticFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
            var queryGroupBy = query.GroupBy(x => new { Created = x.Created.Date, Username = x.Username }).Select(g => new GmailResourceStatisticDto()
            {
                Created = g.Key.Created.Date,
                Username = g.Key.Username,
                Total = g.Count(),
                Ready = g.Where(x => x.Status == GmailResourceStatus.Ready).Count(),
                Success = g.Where(x => x.Status == GmailResourceStatus.Success).Count(),
                Failed = g.Where(x => x.Status == GmailResourceStatus.Failed).Count(),
                Pending = g.Where(x => x.Status == GmailResourceStatus.Pending).Count(),
                Used = g.Where(x => x.Status == GmailResourceStatus.Used).Count(),
                Error = g.Where(x => x.Status == GmailResourceStatus.Error).Count(),
                Unknown = g.Where(x => x.Status == GmailResourceStatus.Unknown).Count()
            });

            var count = await AsyncExecuter.CountAsync(queryGroupBy);
            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                queryGroupBy = queryGroupBy.Skip(input.SkipCount).Take(input.MaxResultCount);

            var res = await AsyncExecuter.ToListAsync(queryGroupBy);
            return new PagedResultDto<GmailResourceStatisticDto>(count, res.OrderByDescending(x => x.Created.Date).ToList());
        }

        [Authorize(GmailServerPermissions.GmailResources.StatisticDaily)]
        public async Task<PagedResultDto<GmailResourceStatisticDailyDto>> GetStatisticDailyAsync(GmailResourceStatisticDailyFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);

            var queryGroupBy = query.GroupBy(x => new { Created = x.Created.Date }).Select(g => new GmailResourceStatisticDailyDto()
            {
                Created = g.Key.Created.Date,
                Total = g.Count(),
                Ready = g.Where(x => x.Status == GmailResourceStatus.Ready).Count(),
                Success = g.Where(x => x.Status == GmailResourceStatus.Success).Count(),
                Failed = g.Where(x => x.Status == GmailResourceStatus.Failed).Count(),
                Pending = g.Where(x => x.Status == GmailResourceStatus.Pending).Count(),
                Used = g.Where(x => x.Status == GmailResourceStatus.Used).Count(),
                Error = g.Where(x => x.Status == GmailResourceStatus.Error).Count(),
                Unknown = g.Where(x => x.Status == GmailResourceStatus.Unknown).Count()
            });
            var count = await AsyncExecuter.CountAsync(queryGroupBy);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                queryGroupBy = queryGroupBy.Skip(input.SkipCount).Take(input.MaxResultCount);

            var res = await AsyncExecuter.ToListAsync(queryGroupBy);
            return new PagedResultDto<GmailResourceStatisticDailyDto>(
                count, res.OrderByDescending(x => x.Created).ToList());
        }

        [Authorize(GmailServerPermissions.GmailResources.ResetStatus)]
        public async Task ResetStatusAsync(ResetStatusFilter input)
        {
            if (input.Statuses.Count > 0)
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("Update AppGmailResources");
                queryBuilder.AppendLine($"Set Status = {(int)input.TargetStatus}, Updated = GETDATE()");
                queryBuilder.AppendLine($"From AppGmailResources");
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
                if (input.UpdatedHours.HasValue)
                {
                    var current = DateTime.Now;
                    var timeCheck = current.AddHours(-input.UpdatedHours.Value);
                    queryBuilder.Append($"And Updated < '{timeCheck.ToString("yyyy-MM-dd HH:mm:ss")}' ");
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

                //var query = Repository.AsQueryable();

                //query = query.Where(x => input.Statuses.Contains(x.Status));
                //query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
                //query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
                //query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);

                //if (input.UpdatedHours.HasValue)
                //{
                //    var current = DateTime.Now;
                //    var timeCheck = current.AddHours(-input.UpdatedHours.Value);
                //    query = query.Where(x => x.Updated < timeCheck);
                //}

                //var gmailResources = await AsyncExecuter.ToListAsync(query);
                //gmailResources.ForEach((gmailResource) =>
                //{
                //    gmailResource.Status = input.TargetStatus;
                //    gmailResource.Updated = DateTime.Now;
                //});

                //await Repository.BulkUpdateAsync(gmailResources, new List<string>()
                //{
                //    nameof(GmailResource.Status),
                //    nameof(GmailResource.Updated)
                //});
            }
        }

        [Authorize(GmailServerPermissions.GmailResources.Download)]
        public async Task<List<GmailResourceExcelModel>> GetGmailResourceExcelModelsAsync(GmailResourceDownloadFilter input)
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
            return ObjectMapper.Map<List<GmailResource>, List<GmailResourceExcelModel>>(res);
        }

        [Authorize(GmailServerPermissions.GmailResources.DeleteFilter)]
        public async Task DeleteAsync(DeleteFilter input)
        {
            if (input.Statuses.Count > 0)
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("DELETE FROM AppGmailResources WHERE ");
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
                if (input.UpdatedHours.HasValue)
                {
                    var current = DateTime.Now;
                    var timeCheck = current.AddHours(-input.UpdatedHours.Value);
                    queryBuilder.Append($"And Updated < '{timeCheck.ToString("yyyy-MM-dd HH:mm:ss")}' ");
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

            //var query = Repository.AsQueryable();

            //query = query.Where(x => input.Statuses.Contains(x.Status));
            //query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
            //query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
            //query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);

            //if (input.UpdatedHours.HasValue)
            //{
            //    var current = DateTime.Now;
            //    var timeCheck = current.AddHours(-input.UpdatedHours.Value);
            //    query = query.Where(x => x.Updated < timeCheck);
            //}

            //var gmailResources = await AsyncExecuter.ToListAsync(query);
            //await Repository.BulkDeleteAsync(gmailResources);
        }

        public async Task<GmailResourceDto> SetPremiumTypeAsync(string email, PremiumType type)
        {
            var gmail = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (gmail != null)
            {
                gmail.UpdatedPremium = DateTime.Now;
                gmail.PremiumType = type;
                return await MapToGetOutputDtoAsync(gmail);
            }
            return new GmailResourceDto();
        }

        public async Task<GmailResourceDto> GetGmailPremiumAsync(DateTime time = default)
        {
            await getPremiumSyncLock.WaitAsync();
            try
            {
                var query = Repository.AsQueryable();
                if (time != DateTime.MinValue)
                {
                    query = query.Where(x => x.Created >= time);
                }
                query = query.Where(x => x.Status == GmailResourceStatus.Success && x.PremiumType == PremiumType.Unset);

                var nonUpdatedPreQuery = query.Where(x => x.UpdatedPremium == DateTime.Parse("0001-01-01 00:00:00.0000000"));
                nonUpdatedPreQuery = nonUpdatedPreQuery.OrderBy(x => x.Updated);
                var gmailResource = await AsyncExecuter.FirstOrDefaultAsync(nonUpdatedPreQuery);

                if (gmailResource == null)
                {
                    var hasUpdatedPreQuery = query.OrderBy(x => x.UpdatedPremium);
                    gmailResource = await AsyncExecuter.FirstOrDefaultAsync(hasUpdatedPreQuery);
                }
                if (gmailResource != null)
                {
                    var res = ObjectMapper.Map<GmailResource, GmailResourceDto>(gmailResource);
                    gmailResource.UpdatedPremium = DateTime.Now;
                    gmailResource.PremiumType = PremiumType.Pending;
                    await Repository.UpdateAsync(gmailResource, autoSave: true);
                    return res;
                }
                return new GmailResourceDto();
            }
            finally
            {
                getPremiumSyncLock.Release();
            }
        }

        public async Task<List<GmailResourceDto>> GetGmailsPremiumByNumber(DateTime time = default, int number = 1)
        {
            await getPremiumByNumberSyncLock.WaitAsync();
            try
            {
                var query = Repository.AsQueryable();
                if (time != DateTime.MinValue)
                {
                    query = query.Where(x => x.Created >= time);
                }
                query = query.Where(x => x.Status == GmailResourceStatus.Success && x.PremiumType == PremiumType.Unset);

                var nonUpdatedPreQuery = query.Where(x => x.UpdatedPremium == DateTime.Parse("0001-01-01 00:00:00.0000000"));
                nonUpdatedPreQuery = nonUpdatedPreQuery.OrderBy(x => x.Updated);
                nonUpdatedPreQuery = nonUpdatedPreQuery.Take(number);
                var gmailPremiums = await AsyncExecuter.ToListAsync(nonUpdatedPreQuery);
                if (gmailPremiums.Count == 0)
                {
                    var hasUpdatedPreQuery = query.OrderBy(x => x.UpdatedPremium).Take(number);
                    gmailPremiums = await AsyncExecuter.ToListAsync(hasUpdatedPreQuery);
                }

                if (gmailPremiums.Count > 0)
                {
                    var res = ObjectMapper.Map<List<GmailResource>, List<GmailResourceDto>>(gmailPremiums);
                    gmailPremiums.ForEach(gmail =>
                    {
                        gmail.PremiumType = PremiumType.Pending;
                        gmail.UpdatedPremium = DateTime.Now;
                    });
                    await Repository.BulkUpdateAsync(gmailPremiums, new List<string>()
                {
                    nameof(GmailResource.UpdatedPremium),
                    nameof(GmailResource.PremiumType)
                });
                    return res;
                }
                return new List<GmailResourceDto>();
            }
            finally
            {
                getPremiumByNumberSyncLock.Release();
            }
        }
    }
}
