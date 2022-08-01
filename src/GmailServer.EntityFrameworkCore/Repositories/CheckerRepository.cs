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
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GmailServer.Repositories
{
    public class CheckerRepository : EfCoreRepository<GmailServerDbContext, Checker, long>, ICheckerRepository
    {
        public CheckerRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task BulkUpdateAsync(List<Checker> checkers, List<string> propertiesToInclude)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkUpdateAsync(checkers, new BulkConfig()
            {
                PropertiesToInclude = propertiesToInclude
            });
        }

        public IQueryable<Checker> FullTextSearch(IQueryable<Checker> query, Expression<Func<Checker, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }

        public async Task<List<Checker>> GetCheckerTimeoutHasTaskCheckAsync(int second)
        {
            var dbContext = await GetDbContextAsync();
            var current = DateTime.Now;
            var timeout = current.AddSeconds(-second);
            var checkers = await dbContext.Checkers
                .Where(x => x.LastCheck < timeout && x.TaskChecks.Count > 0)
                .ToListAsync();
            return checkers;
        }

        public async Task<Checker> GetCheckerOnlineFirstAsync()
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Checkers
                .Where(x => x.Status == CheckerStatus.Online)
                .OrderBy(x => x.TaskChecks.Count)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateStatusByTimeoutAsync(int second)
        {
            var dbContext = await GetDbContextAsync();
            var current = DateTime.Now;
            var timeout = current.AddSeconds(-second);
            var checkers = await dbContext.Checkers.Where(x => x.LastCheck < timeout).ToListAsync();
            checkers.ForEach(x => x.Status = CheckerStatus.Offline);

            await dbContext.BulkUpdateAsync(checkers, new BulkConfig()
            {
                PropertiesToInclude = new List<string>() { nameof(Checker.Status) }
            });
        }
    }
}
