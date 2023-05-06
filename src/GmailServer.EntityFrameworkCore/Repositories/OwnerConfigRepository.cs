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
    public class OwnerConfigRepository : EfCoreRepository<GmailServerDbContext, OwnerConfig, long>, IOwnerConfigRepository
    {
        public OwnerConfigRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IQueryable<OwnerConfig> FullTextSearch(IQueryable<OwnerConfig> query, Expression<Func<OwnerConfig, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }

        public async Task<OwnerConfig> GetByKeyAsync(string key)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.OwnerConfigs.Where(x => x.Key == key).FirstOrDefaultAsync();
        }
    }
}
