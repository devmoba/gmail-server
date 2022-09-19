using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GmailServer.Repositories
{
    public class GmailPremiumRepository : EfCoreRepository<GmailServerDbContext, GmailPremium, long>, IGmailPremiumRepository
    {
        public GmailPremiumRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task BulkInsertAsync(List<GmailPremium> gmailPremiums)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkInsertAsync(gmailPremiums);
        }

        public async Task DeleteAllAsync()
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync("Truncate Table AppGmailPremiums");
        }

        public async Task DeleteGmailPremiumCompleted(int timeCheckDelete)
        {
            var dbContext = await GetDbContextAsync();
            var timeCheck = DateTime.Now.AddHours(-timeCheckDelete);
            var gmailPremiums = await dbContext.GmailPremiums
                .Where(x => x.Created < timeCheck && x.Status == Enums.GmailPremiumStatus.Completed)
                .ToListAsync();
            await dbContext.BulkDeleteAsync(gmailPremiums);
        }
    }
}
