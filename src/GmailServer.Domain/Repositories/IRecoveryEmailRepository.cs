using GmailServer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IRecoveryEmailRepository : IRepository<RecoveryEmail, long>
    {
        Task BulkInsertAsync(List<RecoveryEmail> recoveryEmails);

        Task DeleteRecoveryEmailCompleted(int timeCheckDelete);

        Task DeleteAllAsync();
    }
}
