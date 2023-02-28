using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using GmailServer.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task BulkUpdateAsync(List<GmailResource> gmailResources, List<string> propertiesToInclude)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkUpdateAsync(gmailResources, new BulkConfig()
            {
                PropertiesToInclude = propertiesToInclude
            });
        }

        public async Task BulkDeleteAsync(List<GmailResource> gmailResources)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkDeleteAsync(gmailResources);
        }

        public async Task DeleteAllAsync()
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync("Truncate Table AppGmailResources");
        }

        public async Task UpdateStatusByTimeoutAsync(int minute)
        {
            var dbContext = await GetDbContextAsync();
            var timeCheck = DateTime.Now.AddMinutes(-minute);
            var gmailResources = await dbContext.GmailResources
                .Where(x => x.TakenTime < timeCheck && x.Status == GmailResourceStatus.Pending)
                .ToListAsync();
            if (gmailResources.Count > 0)
            {
                gmailResources.ForEach((gr) =>
                {
                    gr.Status = GmailResourceStatus.Ready;
                    gr.Updated = DateTime.Now;
                });
                var bulkConfig = new BulkConfig()
                {
                    PropertiesToInclude = new List<string>()
                {
                    nameof(GmailResource.Status),
                    nameof(GmailResource.Updated)
                }
                };
                await dbContext.BulkUpdateAsync(gmailResources, bulkConfig);
            }
        }

        public async Task UpdatePremiumTypeByTimeoutAsync(int minute)
        {
            var dbContext = await GetDbContextAsync();
            var timeCheck = DateTime.Now.AddMinutes(-minute);
            var gmailResources = await dbContext.GmailResources
                .Where(x => x.UpdatedPremium < timeCheck && x.PremiumType == PremiumType.Pending)
                .ToListAsync();
            if (gmailResources.Count > 0)
            {
                gmailResources.ForEach((gr) =>
                {
                    gr.PremiumType = PremiumType.Unset;
                    gr.UpdatedPremium = DateTime.Now;
                });
                var bulkConfig = new BulkConfig()
                {
                    PropertiesToInclude = new List<string>()
                {
                    nameof(GmailResource.PremiumType),
                    nameof(GmailResource.UpdatedPremium)
                }
                };
                await dbContext.BulkUpdateAsync(gmailResources, bulkConfig);
            }
        }
    }
}
