using EFCore.BulkExtensions;
using GmailServer.Entities;
using GmailServer.EntityFrameworkCore;
using GmailServer.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GmailServer.Repositories
{
    public class RecoveryEmailRepository : EfCoreRepository<GmailServerDbContext, RecoveryEmail, long>, IRecoveryEmailRepository
    {
        public RecoveryEmailRepository(IDbContextProvider<GmailServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task BulkInsertAsync(List<RecoveryEmail> recoveryEmails)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkInsertAsync(recoveryEmails);
        }

        public async Task DeleteAllAsync()
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync("Truncate Table AppRecoveryEmails");
        }

        public async Task DeleteRecoveryEmailCompleted(int timeCheckDelete)
        {
            var dbContext = await GetDbContextAsync();
            var timeCheck = DateTime.Now.AddHours(-timeCheckDelete);
            var recoveryEmails = await dbContext.RecoveryEmails
                .Where(x => x.Created < timeCheck && x.Status == Enums.RecoveryEmailStatus.Completed)
                .ToListAsync();
            await dbContext.BulkDeleteAsync(recoveryEmails);    
        }

        public async Task<bool> IsReserveQuantityEnoughAsync(int reserveQuantity)
        {
            var dbContext = await GetDbContextAsync();
            var count = await dbContext.RecoveryEmails.Where(x => x.Status == RecoveryEmailStatus.Ready).CountAsync();
            return count > reserveQuantity; // true
        }
    }
}
