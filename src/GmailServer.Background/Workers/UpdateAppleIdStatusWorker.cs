using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers
{
    public class UpdateAppleIdStatusWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;

        public UpdateAppleIdStatusWorker(AbpAsyncTimer timer, 
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            timer.Period = _cfg.GetValue<int>("Workers:UpdateAppleIdStatusWorker:Interval");
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start update AppleId status worker: Do something...");
            var timeout = _cfg.GetValue<int>("Workers:UpdateAppleIdStatusWorker:Timeout");
            var appleIdRepository = workerContext.ServiceProvider.GetRequiredService<IAppleIdRepository>();
            await appleIdRepository.UpdateStatusByTimeoutAsync(timeout);
            await Task.FromResult(1);
            Logger.LogInformation("Finish worker: Something done...");
        }
    }
}
