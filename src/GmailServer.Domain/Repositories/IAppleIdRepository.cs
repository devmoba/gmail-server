using GmailServer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IAppleIdRepository : IRepository<AppleId, long>
    {
        Task BulkInsertAsync(List<AppleId> appleIds);

        Task DeleteAppleIdCompleted(int timeCheckDelete);

        Task DeleteAllAsync();
    }

}
