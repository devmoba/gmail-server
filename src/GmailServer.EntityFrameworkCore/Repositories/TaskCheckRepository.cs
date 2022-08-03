using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GmailServer.Repositories
{
    public class TaskCheckRepository : EfCoreRepository<GmailServerDbContext, TaskCheck, long>, ITaskCheckRepository
    {
        public TaskCheckRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task BulkDeleteAsync(List<long> keys)
        {
            var dbContext = await GetDbContextAsync();
            var taskChecks = await dbContext.TaskChecks.Where(x => keys.Contains(x.Id)).ToListAsync();
            await dbContext.BulkDeleteAsync(taskChecks);
        }

        public async Task BulkUpdateAsync(List<TaskCheck> taskChecks, List<string> propertiesToInclude)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkUpdateAsync(taskChecks, new BulkConfig()
            {
                PropertiesToInclude = propertiesToInclude,
                BatchSize = 100
            });
        }

        public async Task DeleteTaskCheckFailedAsync(int timeCheckDelete)
        {
            var dbContext = await GetDbContextAsync();
            var conditionTime = DateTime.Now.AddMinutes(-timeCheckDelete);
            var taskChecks = await dbContext.TaskChecks
                .Where(x => x.Created < conditionTime)
                .ToListAsync();
            await dbContext.BulkDeleteAsync(taskChecks);
        }

        public async Task<List<TaskCheck>> GetByCheckerIdAsync(long checkerId)
        {
            var dbContext = await GetDbContextAsync();
            var taskChecks = await dbContext.TaskChecks
                .Where(x => x.CheckerId == checkerId)
                .ToListAsync();
            return taskChecks;
        }
    }
}
