using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using GmailServer.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GmailServer.Repositories
{
    public class AppleIdNoneRepository : EfCoreRepository<GmailServerDbContext, AppleIdNone, long>, IAppleIdNoneRepository
    {
        public AppleIdNoneRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task BulkInsertAsync(List<AppleIdNone> appleIdNones)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkInsertAsync(appleIdNones);
        }

        public async Task BulkUpdateAsync(List<AppleIdNone> appleIdNones, List<string> propertiesToInclude)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkUpdateAsync(appleIdNones, new BulkConfig()
            {
                PropertiesToInclude = propertiesToInclude,
                BatchSize = 4000
            });
        }

        public async Task DeleteAllAsync()
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync("Truncate Table AppAppleIdNones");
        }

        public async Task ExecuteSqlRawAsync(string query)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync($"{query}");
        }

        public async Task UpdateStatusByTimeoutAsync(int minute)
        {
            var dbContext = await GetDbContextAsync();
            var timeCheck = DateTime.Now.AddMinutes(-minute);
            var appleIdNones = await dbContext.AppleIdNones
                .Where(x => x.TakenTime < timeCheck && x.Status == Enums.AppleIdNoneStatus.Pending)
                .ToListAsync();
            if (appleIdNones.Count > 0)
            {
                appleIdNones.ForEach((appleIdNone) =>
                {
                    appleIdNone.Status = Enums.AppleIdNoneStatus.Ready;
                    appleIdNone.Updated = DateTime.Now;
                });
                var bulkConfig = new BulkConfig()
                {
                    PropertiesToInclude = new List<string>()
                    {
                        nameof(AppleIdNone.Status),
                        nameof(AppleIdNone.Updated)
                    }
                };
                await dbContext.BulkUpdateAsync(appleIdNones, bulkConfig);
            }
        }

        public async Task UpdateRemovePaymentStatusByTimeoutAsync(int minute)
        {
            var dbContext = await GetDbContextAsync();
            var timeCheck = DateTime.Now.AddMinutes(-minute);
            var appleIdNones = await dbContext.AppleIdNones
                .Where(x => x.RemoveTakenTime < timeCheck && x.RemovePaymentStatus == Enums.RemovePaymentStatus.InUse)
                .ToListAsync();
            if (appleIdNones.Count > 0)
            {
                appleIdNones.ForEach((appleIdNone) =>
                {
                    appleIdNone.RemovePaymentStatus = Enums.RemovePaymentStatus.Ready;
                    appleIdNone.RemoveUpdateTime = DateTime.Now;
                });
                var bulkConfig = new BulkConfig()
                {
                    PropertiesToInclude = new List<string>()
                    {
                        nameof(AppleIdNone.RemovePaymentStatus),
                        nameof(AppleIdNone.RemoveUpdateTime)
                    }
                };
                await dbContext.BulkUpdateAsync(appleIdNones, bulkConfig);
            }
        }

        public IQueryable<AppleIdNone> FullTextSearch(IQueryable<AppleIdNone> query, Expression<Func<AppleIdNone, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }
    }
}
