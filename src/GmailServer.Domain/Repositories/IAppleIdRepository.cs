using GmailServer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IAppleIdRepository : IRepository<AppleId, long>
    {
        Task BulkInsertAsync(List<AppleId> appleIds);

        Task BulkUpdateAsync(List<AppleId> appleIds, List<string> propertiesToInclude);

        Task BulkDeleteAsync(List<AppleId> appleIds);

        Task DeleteAppleIdCompletedAsync(int timeCheckDelete);

        Task DeleteAllAsync();

        Task UpdateStatusByTimeoutAsync(int minute);

    }

}
