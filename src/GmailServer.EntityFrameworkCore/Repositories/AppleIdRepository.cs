using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
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

        public async Task DeleteAllAsync()
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync("Truncate Table AppAppleIds");
        }

        public async Task DeleteAppleIdCompleted(int timeCheckDelete)
        {
            var dbContext = await GetDbContextAsync();
            var timeCheck = DateTime.Now.AddHours(-timeCheckDelete);
            var appleIds = await dbContext.AppleIds
                .Where(x => x.Created < timeCheck && x.Status == Enums.AppleIdStatus.Completed)
                .ToListAsync();
            await dbContext.BulkDeleteAsync(appleIds);
        }
    }
}
