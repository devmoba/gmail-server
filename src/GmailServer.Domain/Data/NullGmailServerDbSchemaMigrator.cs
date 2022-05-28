using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace GmailServer.Data
{
    /* This is used if database provider does't define
     * IGmailServerDbSchemaMigrator implementation.
     */
    public class NullGmailServerDbSchemaMigrator : IGmailServerDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}