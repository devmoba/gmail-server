using GmailServer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IAppleIdNoneRepository : IRepository<AppleIdNone, long>
    {
        Task BulkInsertAsync(List<AppleIdNone> appleIdNones);

        Task BulkUpdateAsync(List<AppleIdNone> appleIdNones, List<string> propertiesToInclude);

        Task ExecuteSqlRawAsync(string query);

        Task DeleteAllAsync();

        Task UpdateStatusByTimeoutAsync(int minute);

        Task UpdateRemovePaymentStatusByTimeoutAsync(int minute);
    }
}
