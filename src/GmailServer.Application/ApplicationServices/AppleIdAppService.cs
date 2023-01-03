using GmailServer.AppleIds;
using GmailServer.AppleIds.Statistics;
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
        AppleIdGetOutputDto,
        AppleIdGetListOutputDto,
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

        [Authorize(GmailServerPermissions.AppleIds.Default)]
        public override async Task<PagedResultDto<AppleIdGetListOutputDto>> GetListAsync(AppleIdFilterDto input)
        {
            var query = await Repository.WithDetailsAsync(x => x.DownloadedApps);
            //if (!string.IsNullOrEmpty(input.Email))
            //    query = Repository.FullTextSearch(query, x => x.Email, input.Email);
            query = query.WhereIf(!string.IsNullOrEmpty(input.Email), x => x.Email == input.Email.ToLower().Trim());
            query = query.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);

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

            var res = ObjectMapper.Map<List<AppleId>, List<AppleIdGetListOutputDto>>(entities);

            return new PagedResultDto<AppleIdGetListOutputDto>(count, res);
        }

        public async override Task<AppleIdGetOutputDto> CreateAsync(CreateUpdateAppleIdDto input)
        {
            if (CommonMethod.IsValidEmail(input.Email))
            {
                var appleId = ObjectMapper.Map<CreateUpdateAppleIdDto, AppleId>(input);
                appleId.Created = DateTime.Now;
                appleId.Updated = DateTime.Now;
                appleId.Status = Enums.AppleIdStatus.Ready;
                var res = await Repository.InsertAsync(appleId, autoSave: true);
                return await MapToGetOutputDtoAsync(res);
            }
            else
            {
                throw new UserFriendlyException($"{input.Email} - invalidate!");
            }
        }

        [Authorize(GmailServerPermissions.AppleIds.Create)]
        public async Task CreateManyAsync(CreateManyAppleIdInputDto input)
        {
            var appleIds = input.Emails.Split("\r\n").ToList();
            if (appleIds.Count == 0)
                throw new UserFriendlyException("Input empty!");
            var entities = new List<AppleId>();
            foreach (var appleId in appleIds)
            {
                if (ValidateAppleIdInput(appleId))
                {
                    var appleIdSplit = appleId.Split('|').ToArray();
                    var email = appleIdSplit[0].ToLower();
                    var hasEmail = await Repository.AnyAsync(x => x.Email == email);
                    if (!hasEmail)
                    {
                        var entity = new AppleId()
                        {
                            Username = input.Username,
                            Email = email,
                            Password = appleIdSplit[1],
                            Status = Enums.AppleIdStatus.Ready,
                            Created = DateTime.Now,
                            Updated = DateTime.Now,
                            //TakenTime = DateTime.Now
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

        public async Task<AppleIdGetOutputDto> GetFirstAppleIdAsync()
        {
            var query = Repository.Where(x => x.Status == Enums.AppleIdStatus.Ready);
            query = query.Where(x => x.TakenTime == DateTime.Parse("0001-01-01 00:00:00.0000000"));
            query = query.OrderBy(x => x.Updated);
            //query = query.OrderBy(x => Guid.NewGuid());
            var appleId = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (appleId == null)
            {
                var query2 = Repository
                    .Where(x => x.Status == Enums.AppleIdStatus.Ready)
                    .OrderBy(x => x.TakenTime);
                appleId = await AsyncExecuter.FirstOrDefaultAsync(query2);
            }

            if (appleId != null)
            {
                var res = ObjectMapper.Map<AppleId, AppleIdGetOutputDto>(appleId);
                appleId.Status = Enums.AppleIdStatus.Pending;
                appleId.TakenTime = DateTime.Now;
                appleId.Updated = DateTime.Now;
                await Repository.UpdateAsync(appleId, autoSave: true);
                return res;
            }
            return new AppleIdGetOutputDto();
        }

        public async Task<AppleIdGetOutputDto> GetByStatusAsync(AppleIdStatus status)
        {
            var query = Repository.Where(x => x.Status == status);
            query = query.OrderBy(x => x.TakenTime);
            var appleId = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (appleId != null)
            {
                var res = ObjectMapper.Map<AppleId, AppleIdGetOutputDto>(appleId);
                appleId.TakenTime = DateTime.Now;
                await Repository.UpdateAsync(appleId, autoSave: true);
                return res;
            }
            return new AppleIdGetOutputDto();
        }

        private bool ValidateAppleIdInput(string str)
        {
            return Regex.IsMatch(str, @"^(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\|(.+)");

        }

        public async Task<AppleIdGetOutputDto> UpdateStatusAsync(string email, AppleIdStatus status)
        {
            var appleId = await AsyncExecuter.FirstOrDefaultAsync(Repository.Where(x => x.Email == email));
            if (appleId != null && appleId.Status != AppleIdStatus.Completed1)
            {
                appleId.Status = status;
                appleId.Updated = DateTime.Now;
                var res = await Repository.UpdateAsync(appleId);
                return await MapToGetOutputDtoAsync(res);
            }
            return new AppleIdGetOutputDto();
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

        [Authorize(GmailServerPermissions.AppleIds.Download)]
        public async Task<List<AppleIdExcelModel>> GetAppleIdExcelModelsAsync(AppleIdDownloadFilter input)
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
            return ObjectMapper.Map<List<AppleId>, List<AppleIdExcelModel>>(res);
        }

        [Authorize]
        public async Task<List<AppleIdStatusSelectionDto>> GetAppleIdStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo, int? updatedHours = null)
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
            var groupBy = query.GroupBy(x => x.Status).Select(x => new AppleIdStatusSelectionDto()
            {
                Text = $"{x.Key.ToString()} | {x.Count()}",
                Value = x.Key
            });
            var res = await AsyncExecuter.ToListAsync(groupBy);
            return res;
        }

        [Authorize(GmailServerPermissions.AppleIds.Statistic)]
        public async Task<PagedResultDto<AppleIdStatisticDto>> GetStatisticAsync(AppleIdStatisticFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
            var queryGroupBy = query.GroupBy(x => new { Username = x.Username }).Select(g => new AppleIdStatisticDto()
            {
                Username = g.Key.Username,
                Total = g.Count(),
                Ready = g.Where(x => x.Status == AppleIdStatus.Ready).Count(),
                Completed1 = g.Where(x => x.Status == AppleIdStatus.Completed1).Count(),
                Completed2 = g.Where(x => x.Status == AppleIdStatus.Completed2).Count(),
                Completed3 = g.Where(x => x.Status == AppleIdStatus.Completed3).Count(),
                Completed4 = g.Where(x => x.Status == AppleIdStatus.Completed4).Count(),
                Pending = g.Where(x => x.Status == AppleIdStatus.Pending).Count(),
                WrongPass = g.Where(x => x.Status == AppleIdStatus.WrongPass).Count(),
                Subed = g.Where(x => x.Status == AppleIdStatus.Subed).Count(),
                Locked1 = g.Where(x => x.Status == AppleIdStatus.Locked1).Count(),
                Locked2 = g.Where(x => x.Status == AppleIdStatus.Locked2).Count(),
                Review = g.Where(x => x.Status == AppleIdStatus.Review).Count(),
                Error = g.Where(x => x.Status == AppleIdStatus.Error).Count(),
                Unknown = g.Where(x => x.Status == AppleIdStatus.Unknown).Count()
            });

            var count = await AsyncExecuter.CountAsync(queryGroupBy);
            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                queryGroupBy = queryGroupBy.Skip(input.SkipCount).Take(input.MaxResultCount);

            var res = await AsyncExecuter.ToListAsync(queryGroupBy);
            return new PagedResultDto<AppleIdStatisticDto>(count, res);
        }

        [Authorize(GmailServerPermissions.AppleIds.StatisticDaily)]
        public async Task<PagedResultDto<AppleIdStatisticDailyDto>> GetStatisticDailyAsync(AppleIdStatisticDailyFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);

            var queryGroupBy = query.GroupBy(x => new { Created = x.Created.Date }).Select(g => new AppleIdStatisticDailyDto()
            {
                Created = g.Key.Created.Date,
                Total = g.Count(),
                Ready = g.Where(x => x.Status == AppleIdStatus.Ready).Count(),
                Completed1 = g.Where(x => x.Status == AppleIdStatus.Completed1).Count(),
                Completed2 = g.Where(x => x.Status == AppleIdStatus.Completed2).Count(),
                Completed3 = g.Where(x => x.Status == AppleIdStatus.Completed3).Count(),
                Completed4 = g.Where(x => x.Status == AppleIdStatus.Completed4).Count(),
                Pending = g.Where(x => x.Status == AppleIdStatus.Pending).Count(),
                WrongPass = g.Where(x => x.Status == AppleIdStatus.WrongPass).Count(),
                Subed = g.Where(x => x.Status == AppleIdStatus.Subed).Count(),
                Locked1 = g.Where(x => x.Status == AppleIdStatus.Locked1).Count(),
                Locked2 = g.Where(x => x.Status == AppleIdStatus.Locked2).Count(),
                Review = g.Where(x => x.Status == AppleIdStatus.Review).Count(),
                Error = g.Where(x => x.Status == AppleIdStatus.Error).Count(),
                Unknown = g.Where(x => x.Status == AppleIdStatus.Unknown).Count()
            });
            queryGroupBy = queryGroupBy.OrderByDescending(x => x.Created);
            var count = await AsyncExecuter.CountAsync(queryGroupBy);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                queryGroupBy = queryGroupBy.Skip(input.SkipCount).Take(input.MaxResultCount);

            var res = await AsyncExecuter.ToListAsync(queryGroupBy);
            return new PagedResultDto<AppleIdStatisticDailyDto>(count, res);
        }

        [Authorize(GmailServerPermissions.AppleIds.Statistic)]
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

        [Authorize(GmailServerPermissions.AppleIds.ResetStatus)]
        public async Task ResetStatusAsync(ResetStatusFilter input)
        {
            if (input.Statuses.Count > 0)
            {
                var query = Repository.AsQueryable();

                query = query.Where(x => input.Statuses.Contains(x.Status));
                query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
                query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
                query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);

                if (input.UpdatedHours.HasValue)
                {
                    var current = DateTime.Now;
                    var timeCheck = current.AddHours(-input.UpdatedHours.Value);
                    query = query.Where(x => x.Updated < timeCheck);
                }
                var appleIds = await AsyncExecuter.ToListAsync(query);
                appleIds.ForEach((appleId) =>
                {
                    appleId.Status = input.TargetStatus;
                    appleId.Updated = DateTime.Now;
                });

                await Repository.BulkUpdateAsync(appleIds, new List<string>()
                {
                    nameof(AppleId.Status),
                    nameof(AppleId.Updated)
                });
            }
        }

        public async Task DeleteAsync(DeleteFilter input)
        {
            var query = Repository.AsQueryable();

            query = query.Where(x => input.Statuses.Contains(x.Status));
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == input.Username);
            query = query.WhereIf(input.CreatedFrom.HasValue, x => x.Created.Date >= input.CreatedFrom.Value.Date);
            query = query.WhereIf(input.CreatedTo.HasValue, x => x.Created.Date <= input.CreatedTo.Value.Date);

            if (input.UpdatedHours.HasValue)
            {
                var current = DateTime.Now;
                var timeCheck = current.AddHours(-input.UpdatedHours.Value);
                query = query.Where(x => x.Updated < timeCheck);
            }

            var appleIds = await AsyncExecuter.ToListAsync(query);
            await Repository.BulkDeleteAsync(appleIds);
        }
    }
}
