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
    public class GmailTypeRepository : EfCoreRepository<GmailServerDbContext, GmailType, long>, IGmailTypeRepository
    {
        public GmailTypeRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IQueryable<GmailType> FullTextSearch(IQueryable<GmailType> query, Expression<Func<GmailType, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }
    }
}
