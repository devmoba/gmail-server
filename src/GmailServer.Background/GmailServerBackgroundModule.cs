using GmailServer.Background.Workers;
using GmailServer.Background.Workers.Statistics;
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
            context.AddBackgroundWorker<AutoCreateTaskCheckWorker>();
            context.AddBackgroundWorker<UncheckMailWorker>();
            context.AddBackgroundWorker<UpdateCheckerStatusWorker>();
            context.AddBackgroundWorker<DeleteTaskCheckFailedWorker>();
            context.AddBackgroundWorker<DeleteRecoveryEmailCompletedWorker>();
            context.AddBackgroundWorker<GetAndInsertHotmailWorker>();
            context.AddBackgroundWorker<UpdateAppleIdStatusWorker>();
            context.AddBackgroundWorker<UpdateMomoAccountStatusWorker>();
            context.AddBackgroundWorker<UpdateGmailResourceStatusWorker>();
            context.AddBackgroundWorker<UpdateGmailResourcePremiumTyeWorker>();
            context.AddBackgroundWorker<UpdateAppleIdNoneStatusWorker>();
            context.AddBackgroundWorker<UpdateAppleIdNoneRemovePaymentStatus>();

            #region Statistic
            context.AddBackgroundWorker<AppleIdStatisticWorker>();
            context.AddBackgroundWorker<AppleIdRawStatisticWorker>();
            context.AddBackgroundWorker<AppleOrderStatisticWorker>();
            context.AddBackgroundWorker<GmailResourceStatisticWorker>();
            context.AddBackgroundWorker<GmailStatisticWorker>();
            #endregion
        }
    }
}
