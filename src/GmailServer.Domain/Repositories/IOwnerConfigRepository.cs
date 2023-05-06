using GmailServer.Entities;
using System.Linq.Expressions;
using System.Linq;
using System;
using Volo.Abp.Domain.Repositories;
using System.Threading.Tasks;

namespace GmailServer.Repositories
{
    public interface IOwnerConfigRepository : IRepository<OwnerConfig, long>
    {
        IQueryable<OwnerConfig> FullTextSearch(IQueryable<OwnerConfig> query, Expression<Func<OwnerConfig, string>> keySelector, string value);

        Task<OwnerConfig> GetByKeyAsync(string key);
    }
}
