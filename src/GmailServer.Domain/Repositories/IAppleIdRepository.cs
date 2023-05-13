using GmailServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IAppleIdRepository : IRepository<AppleId, long>
    {
        IQueryable<AppleId> FullTextSearch(IQueryable<AppleId> query, Expression<Func<AppleId, string>> keySelector, string value);

        Task BulkInsertAsync(List<AppleId> appleIds);

        Task BulkUpdateAsync(List<AppleId> appleIds, List<string> propertiesToInclude);

        Task ExecuteSqlRawAsync(string query);

        Task BulkDeleteAsync(List<AppleId> appleIds);

        Task DeleteAppleIdCompletedAsync(int timeCheckDelete);

        Task DeleteAllAsync();

        Task UpdateStatusByTimeoutAsync(int minute);

    }

}
