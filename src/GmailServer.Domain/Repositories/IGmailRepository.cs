using GmailServer.Entities;
using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IGmailRepository : IRepository<Gmail, long> 
    {
        IQueryable<Gmail> FullTextSearch(IQueryable<Gmail> query, Expression<Func<Gmail, string>> keySelector, string value);

        Task<List<Gmail>> GetAllAsync();

        Task<List<Gmail>> GetByListIdAsync(List<long> ids);

        Task<List<Gmail>> GetByTimeRangeAsync(DateTime from = default, DateTime to = default);

        Task<List<Gmail>> GetByTimeToCheckAsync(int hourCheck);

        Task<List<Gmail>> GetByCheckingTimeoutAsync(DateTime uncheckTime, Status status);

        Task BulkUpdateAsync(List<Gmail> gmails, List<string> propertiesToInclude);
    }
}
