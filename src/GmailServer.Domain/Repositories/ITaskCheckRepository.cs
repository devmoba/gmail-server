using GmailServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface ITaskCheckRepository : IRepository<TaskCheck, long>
    {
        Task BulkUpdateAsync(List<TaskCheck> taskChecks, List<string> propertiesToInclude);

        Task DeleteTaskCheckFailedAsync(int timeCheckDelete);

        Task<List<TaskCheck>> GetByCheckerIdAsync(long checkerId);
    }
}
