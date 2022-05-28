using GmailServer.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace GmailServer.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(GmailServerEntityFrameworkCoreModule),
        typeof(GmailServerApplicationContractsModule)
    )]
    public class GmailServerDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.IsJobExecutionEnabled = false;
            });
        }
    }
}
