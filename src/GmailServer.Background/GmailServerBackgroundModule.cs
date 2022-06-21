using GmailServer.Background.Workers;
using GmailServer.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;

namespace GmailServer.Background
{
    [DependsOn(
        typeof(GmailServerDomainModule),
        typeof(GmailServerDomainSharedModule),
        typeof(GmailServerEntityFrameworkCoreModule),
        typeof(AbpBackgroundWorkersModule)
        )]
    public class GmailServerBackgroundModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.AddBackgroundWorker<CheckMailReportWorker>();
            context.AddBackgroundWorker<UncheckMailWorker>();
        }
    }
}
