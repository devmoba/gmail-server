using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers
{
    public class UpdateAppleIdNoneRemovePaymentStatus : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;

        public UpdateAppleIdNoneRemovePaymentStatus(AbpAsyncTimer timer, 
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            timer.Period = _cfg.GetValue<int>("Workers:UpdateAppleIdNoneRemovePaymentStatus:Interval");
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {

            Logger.LogInformation("Start update AppleIdNone RemovePaymentStatus worker: Do something...");
            var timeout = _cfg.GetValue<int>("Workers:UpdateAppleIdNoneRemovePaymentStatus:Timeout");
            var repository = workerContext.ServiceProvider.GetRequiredService<IAppleIdNoneRepository>();
            await repository.UpdateRemovePaymentStatusByTimeoutAsync(timeout);
            Logger.LogInformation("Finish worker: Something done...");
        }
    }
}
