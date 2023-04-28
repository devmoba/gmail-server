using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            await dbContext.Database.ExecuteSqlRawAsync("Truncate Table AppAppleIds");
        }

        public async Task ExecuteSqlRawAsync(string query)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync($"{query}");
        }

        public Task UpdateStatusByTimeoutAsync(int minute)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRemovePaymentStatusByTimeoutAsync(int minute)
        {
            throw new NotImplementedException();
        }
    }
}
