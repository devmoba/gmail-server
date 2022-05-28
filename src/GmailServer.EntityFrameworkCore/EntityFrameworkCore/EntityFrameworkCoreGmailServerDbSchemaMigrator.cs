using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GmailServer.Data;
using Volo.Abp.DependencyInjection;

namespace GmailServer.EntityFrameworkCore
{
    public class EntityFrameworkCoreGmailServerDbSchemaMigrator
        : IGmailServerDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreGmailServerDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the GmailServerDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<GmailServerDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
