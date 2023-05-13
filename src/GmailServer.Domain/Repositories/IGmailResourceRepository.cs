using GmailServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IGmailResourceRepository : IRepository<GmailResource, long>
    {
        IQueryable<GmailResource> FullTextSearch(IQueryable<GmailResource> query, Expression<Func<GmailResource, string>> keySelector, string value);

        Task ExecuteSqlRawAsync(string query);

        Task BulkInsertAsync(List<GmailResource> gmailPremiums);

        Task BulkUpdateAsync(List<GmailResource> gmailResources, List<string> propertiesToInclude);

        Task DeleteAllAsync();

        Task BulkDeleteAsync(List<GmailResource> gmailResources);

        Task UpdateStatusByTimeoutAsync(int minute);

        Task UpdatePremiumTypeByTimeoutAsync(int minute);

    }
}
