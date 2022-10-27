using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GmailServer.Repositories
{
    public class GmailResourceRepository : EfCoreRepository<GmailServerDbContext, GmailResource, long>, IGmailResourceRepository
    {
        public GmailResourceRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task BulkInsertAsync(List<GmailResource> gmails)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkInsertAsync(gmails);
        }

        public async Task BulkUpdateAsync(List<GmailResource> gmailResources, List<string> propertiesToExclude)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkUpdateAsync(gmailResources, new BulkConfig()
            {
                PropertiesToExclude = propertiesToExclude
            });
        }

        public async Task DeleteAllAsync()
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync("Truncate Table AppGmailResources");
        }
    }
}
