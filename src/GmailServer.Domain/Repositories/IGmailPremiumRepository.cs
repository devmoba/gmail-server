using GmailServer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IGmailPremiumRepository : IRepository<GmailPremium, long>
    {
        Task BulkInsertAsync(List<GmailPremium> gmailPremiums);

        Task DeleteGmailPremiumCompleted(int timeCheckDelete);

        Task DeleteAllAsync();

    }
}
