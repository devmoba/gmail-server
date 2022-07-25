using GmailServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface ICheckerRepository : IRepository<Checker, long>
    {
        IQueryable<Checker> FullTextSearch(IQueryable<Checker> query, Expression<Func<Checker, string>> keySelector, string value);

        Task BulkUpdateAsync(List<Checker> checkers, List<string> propertiesToInclude);

        Task UpdateStatusByTimeoutAsync(int second);

        Task<Checker> GetCheckerOnlineFirstAsync();
    }
}
