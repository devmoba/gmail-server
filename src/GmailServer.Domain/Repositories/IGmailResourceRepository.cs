using GmailServer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IGmailResourceRepository : IRepository<GmailResource, long>
    {
        Task BulkInsertAsync(List<GmailResource> gmailPremiums);

        Task BulkUpdateAsync(List<GmailResource> gmailResources, List<string> propertiesToExclude);

        Task DeleteAllAsync();

        Task BulkDeleteAsync(List<GmailResource> gmailResources);

    }
}
