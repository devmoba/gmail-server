using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using GmailServer.Enums;
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
    public class GmailRepository : EfCoreRepository<GmailServerDbContext, Gmail, long>, IGmailRepository
    {
        public GmailRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task BulkUpdateAsync(List<Gmail> gmails, List<string> propertiesToInclude)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkUpdateAsync(gmails, new BulkConfig()
            {
                PropertiesToInclude = propertiesToInclude
            });
        }

        public IQueryable<Gmail> FullTextSearch(IQueryable<Gmail> query, Expression<Func<Gmail, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }

        public async Task<List<Gmail>> GetAllAsync()
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Gmails.ToListAsync();
        }

        public async Task<List<Gmail>> GetByCheckingTimeoutAsync(DateTime uncheckTime, Status status)
        {
            var dbConetxt = await GetDbContextAsync();
            var query = dbConetxt.Gmails.AsQueryable();
            query = query.Where(x => x.LastCheck < uncheckTime);
            query = query.Where(x => x.Status == status);

            return await query.ToListAsync();
        }

        public async Task<List<Gmail>> GetByListIdAsync(List<long> ids)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Gmails.Where(x => ids.Contains(x.Id)).OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<List<Gmail>> GetByTimeRangeAsync(DateTime from = default, DateTime to = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.Gmails.Where(x => x.Created >= from);
            query = query.Where(x => x.Created <= to);
            return await query.ToListAsync();
        }

        public async Task<List<Gmail>> GetByTimeToCheckAsync(int hourCheck, int maxcount = 100)
        {
            var timeToCheck = DateTime.Now.AddHours(-hourCheck);
            var dbConetxt = await GetDbContextAsync();
            var query = dbConetxt.Gmails.AsQueryable();

            query = query.Where(x => x.Created < timeToCheck && (x.TimeDiff == 0 || x.TimeDiff < hourCheck))
                .TakeLast(maxcount);

            return await query.ToListAsync();
        }
    }
}
