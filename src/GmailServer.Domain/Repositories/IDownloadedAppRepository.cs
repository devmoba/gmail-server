using GmailServer.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IDownloadedAppRepository : IRepository<DownloadedApp, long>
    {
        IQueryable<DownloadedApp> FullTextSearch(IQueryable<DownloadedApp> query, Expression<Func<DownloadedApp, string>> keySelector, string value);
    }
}
