using EFCore.BulkExtensions;
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
    public class MomoAccountRepository : EfCoreRepository<GmailServerDbContext, MomoAccount, long>, IMomoAccountRepository
    {
        public MomoAccountRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task BulkInsertAsync(List<MomoAccount> momoAccounts)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkInsertAsync(momoAccounts);
        }

        public async Task DeleteAllAsync()
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync("Truncate Table AppMomoAccounts");
        }

        public async Task ExecuteSqlRawAsync(string query)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync($"{query}");
        }

        public IQueryable<MomoAccount> FullTextSearch(IQueryable<MomoAccount> query, Expression<Func<MomoAccount, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }
    }
}
