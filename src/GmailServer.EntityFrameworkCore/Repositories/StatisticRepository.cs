using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GmailServer.Repositories
{
    public class StatisticRepository : EfCoreRepository<GmailServerDbContext, Statistic, long>, IStatisticRepository
    {
        public StatisticRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IQueryable<Statistic> FullTextSearch(IQueryable<Statistic> query, Expression<Func<Statistic, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }

        public async Task ExecuteSqlRawAsync(string query)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync($"{query}");
        }

        public async Task DeleteAsync(string entityName, DateTime? from, DateTime? to)
        {
            var dbContext = await GetDbContextAsync();
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(@"Delete From AppStatistics");

            if (!string.IsNullOrEmpty(entityName))
                queryBuilder.AppendLine($"Where EntityName = '{entityName}' ");

            if (from.HasValue)
                queryBuilder.Append($"And Date >= '{from.Value.Date.ToString("yyyy-MM-dd")}' ");

            if (to.HasValue)
                queryBuilder.Append($"And Date < '{to.Value.Date.AddDays(1).ToString("yyyy-MM-dd")}' ");

            await dbContext.Database.ExecuteSqlRawAsync(queryBuilder.ToString());
        }

        public async Task AddOrUpdateForEntityAsync(string entityName, int recoveryDays = 1)
        {
            if (string.IsNullOrEmpty(entityName))
                return;
            for (int i = 0; i < recoveryDays; i++)
            {
                var currentDate = DateTime.Now.Date;
                if (string.Compare(nameof(AppleId), entityName) == 0)
                    await AddOrUpdateForAppleIdAsync(currentDate.AddDays(-i));

                if (string.Compare(nameof(GmailResource), entityName) == 0)
                    await AddOrUpdateForGmailResourceAsync(currentDate.AddDays(-i));

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
        public async Task AddOrUpdateForGmailResourceAsync(DateTime? dateTime = null)
        {
            var dbContext = await GetDbContextAsync();
            var currentDay = dateTime.HasValue ? dateTime.Value.Date : DateTime.Now.Date;
            var query = dbContext.GmailResources.Where(x => x.Created >= currentDay && x.Created < currentDay.AddDays(1));

            var statistics = await query.GroupBy(x => x.Username).Select(g => new Statistic()
            {
                EntityName = nameof(GmailResource),
                Date = currentDay,
                Username = g.Key,
                Total = g.Count(),
                Type = StatisticType.Daily,
                Data = JsonConvert.SerializeObject(new GmailResourceStatisticData()
                {
                    Ready = g.Where(x => x.Status == GmailResourceStatus.Ready).Count(),
                    Success = g.Where(x => x.Status == GmailResourceStatus.Success).Count(),
                    Pending = g.Where(x => x.Status == GmailResourceStatus.Pending).Count(),
                    Used = g.Where(x => x.Status == GmailResourceStatus.Used).Count(),
                    Failed = g.Where(x => x.Status == GmailResourceStatus.Failed).Count(),
                    Error = g.Where(x => x.Status == GmailResourceStatus.Error).Count(),
                    Unknown = g.Where(x => x.Status == GmailResourceStatus.Unknown).Count(),
                }),
                HashCode = CryptoHelper.CreateSHA256($"{nameof(GmailResource)}|{currentDay}|{g.Key}|{StatisticType.Daily}")
            }).ToListAsync();

            foreach (var statistic in statistics)
            {
                var hashData = statistic.Data.CreateSHA256();
                var entity = await dbContext.Statistics
                    .Where(x => x.HashCode == statistic.HashCode)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    statistic.Arg1 = hashData;
                    await dbContext.Statistics.AddAsync(statistic);
                    continue;
                }
                if (hashData != entity.Arg1)
                {
                    entity.Total = statistic.Total;
                    entity.Data = statistic.Data;
                    entity.Arg1 = hashData;
                }
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task AddOrUpdateForAppleIdAsync(DateTime? dateTime = null)
        {
            var dbContext = await GetDbContextAsync();
            var currentDay = dateTime.HasValue ? dateTime.Value.Date : DateTime.Now.Date;
            var query = dbContext.AppleIds.Where(x => x.Created >= currentDay && x.Created < currentDay.AddDays(1));

            var statistics = await query.GroupBy(x => x.Username).Select(g => new Statistic()
            {
                EntityName = nameof(AppleId),
                Date = currentDay,
                Username = g.Key,
                Total = g.Count(),
                Type = StatisticType.Daily,
                Data = JsonConvert.SerializeObject(new AppleIdStatisticData()
                {
                    TotalPurchaseNumber = g.Sum(x => x.PurchaseNumber),
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
                }),
                HashCode = CryptoHelper.CreateSHA256($"{nameof(AppleId)}|{currentDay}|{g.Key}|{StatisticType.Daily}")
            }).ToListAsync();

            foreach (var statistic in statistics)
            {
                var hashData = statistic.Data.CreateSHA256();
                var entity = await dbContext.Statistics
                    .Where(x => x.HashCode == statistic.HashCode)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    statistic.Arg1 = hashData;
                    await dbContext.Statistics.AddAsync(statistic);
                    continue;
                }
                if (hashData != entity.Arg1)
                {
                    entity.Total = statistic.Total;
                    entity.Data = statistic.Data;
                    entity.Arg1 = hashData;
                }
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
