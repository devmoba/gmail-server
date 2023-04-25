using GmailServer.Entities;
using System.Linq.Expressions;
using System.Linq;
using System;
using Volo.Abp.Domain.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GmailServer.Repositories
{
    public interface IMomoAccountRepository : IRepository<MomoAccount, long>
    {
        IQueryable<MomoAccount> FullTextSearch(IQueryable<MomoAccount> query, Expression<Func<MomoAccount, string>> keySelector, string value);

        Task BulkInsertAsync(List<MomoAccount> momoAccounts);

        Task ExecuteSqlRawAsync(string query);

        Task DeleteAllAsync();
    }
}
