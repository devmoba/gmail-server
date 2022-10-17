using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using GmailServer.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GmailServer.Repositories
{
    public class AppleIdRepository : EfCoreRepository<GmailServerDbContext, AppleId, long>, IAppleIdRepository
    {
        public AppleIdRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task BulkInsertAsync(List<AppleId> appleIds)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkInsertAsync(appleIds);
        }

        public async Task BulkUpdateAsync(List<AppleId> appleIds, List<string> propertiesToExclude)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkUpdateAsync(appleIds, new BulkConfig()
            {
                PropertiesToExclude = propertiesToExclude
            });
        }

        public async Task DeleteAllAsync()
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync("Truncate Table AppAppleIds");
        }

        public async Task DeleteAppleIdCompletedAsync(int timeCheckDelete)
        {
            var dbContext = await GetDbContextAsync();
            var timeCheck = DateTime.Now.AddHours(-timeCheckDelete);
            var appleIds = await dbContext.AppleIds
                .Where(x => x.Created < timeCheck && x.Status == AppleIdStatus.Completed)
                .ToListAsync();
            await dbContext.BulkDeleteAsync(appleIds);
        }

        public async Task UpdateStatusByTimeoutAsync(int hour)
        {
            var dbContext = await GetDbContextAsync();
            var timeCheck = DateTime.Now.AddHours(-hour);
            var appleIds = await dbContext.AppleIds
                .Where(x => x.TakenTime < timeCheck && x.Status == AppleIdStatus.Pending)
                .ToListAsync();
            if (appleIds.Count > 0)
            {
                appleIds.ForEach((appleId) =>
                {
                    appleId.Status = AppleIdStatus.Ready;
                    appleId.Updated = DateTime.Now;
                });
                var bulkConfig = new BulkConfig()
                {
                    PropertiesToInclude = new List<string>()
                {
                    nameof(AppleId.Status)
                }
                };
                await dbContext.BulkUpdateAsync(appleIds, bulkConfig);
            }
        }
    }
}
