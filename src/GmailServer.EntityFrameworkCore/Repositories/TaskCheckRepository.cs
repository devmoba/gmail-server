using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task BulkUpdateAsync(List<TaskCheck> taskChecks, List<string> propertiesToInclude)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkUpdateAsync(taskChecks, new BulkConfig()
            {
                PropertiesToInclude = propertiesToInclude
            });
        }
    }
}
