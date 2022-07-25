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
    public class FakeSettingRepository : EfCoreRepository<GmailServerDbContext, FakeSetting, long>, IFakeSettingRepository
    {
        public FakeSettingRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public IQueryable<FakeSetting> FullTextSearch(IQueryable<FakeSetting> query, Expression<Func<FakeSetting, string>> keySelector, string value)
        {
            return query.FullTextFreeText(keySelector, value);
        }
    }
}
