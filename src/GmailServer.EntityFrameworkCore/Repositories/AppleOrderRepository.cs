using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using GmailServer.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GmailServer.Repositories
{
    public class AppleOrderRepository : EfCoreRepository<GmailServerDbContext, AppleOrder, long>, IAppleOrderRepository
    {
        public AppleOrderRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task ExecuteSqlRawAsync(string query)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }

        public IQueryable<AppleOrder> FullTextSearch(IQueryable<AppleOrder> query, Expression<Func<AppleOrder, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }
    }
}
