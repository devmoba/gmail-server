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
    public class GmailRepository : EfCoreRepository<GmailServerDbContext, Gmail, long>, IGmailRepository
    {
        public GmailRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public IQueryable<Gmail> FullTextSearch(IQueryable<Gmail> query, Expression<Func<Gmail, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }

        public async Task<List<Gmail>> GetAll()
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Gmails.ToListAsync();
        }

        public async Task<List<Gmail>> GetByTimeRange(DateTime from = default, DateTime to = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.Gmails.Where(x => x.Created >= from);
            query = query.Where(x => x.Created <= to);
            return await query.ToListAsync();
        }
    }
}
