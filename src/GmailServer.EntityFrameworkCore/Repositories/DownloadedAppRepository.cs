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
    public class DownloadedAppRepository : EfCoreRepository<GmailServerDbContext, DownloadedApp, long>, IDownloadedAppRepository
    {
        public DownloadedAppRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IQueryable<DownloadedApp> FullTextSearch(IQueryable<DownloadedApp> query, Expression<Func<DownloadedApp, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }
    }
}
