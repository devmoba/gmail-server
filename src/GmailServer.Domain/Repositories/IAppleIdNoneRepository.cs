using GmailServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IAppleIdNoneRepository : IRepository<AppleIdNone, long>
    {
        IQueryable<AppleIdNone> FullTextSearch(IQueryable<AppleIdNone> query, Expression<Func<AppleIdNone, string>> keySelector, string value);

        Task BulkInsertAsync(List<AppleIdNone> appleIdNones);

        Task BulkUpdateAsync(List<AppleIdNone> appleIdNones, List<string> propertiesToInclude);

        Task ExecuteSqlRawAsync(string query);

        Task DeleteAllAsync();

        Task UpdateStatusByTimeoutAsync(int minute);

        Task UpdateRemovePaymentStatusByTimeoutAsync(int minute);
    }
}
