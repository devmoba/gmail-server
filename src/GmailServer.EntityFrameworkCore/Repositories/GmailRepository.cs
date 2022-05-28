using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using GmailServer.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
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
    }
}
