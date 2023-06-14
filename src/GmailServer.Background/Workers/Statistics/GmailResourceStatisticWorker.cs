using GmailServer.Entities;
using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers.Statistics
{
    public class GmailResourceStatisticWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly int _recoveryDays;

        public GmailResourceStatisticWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _recoveryDays = configuration.GetValue<int>("Workers:Statistics:RecoveryDays");
            timer.Period = configuration.GetValue<int>("Workers:Statistics:GmailResource:Interval");
        }

        protected override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start GmailResource Statistic Worker.");
            var statisticRepository = workerContext.ServiceProvider.GetRequiredService<IStatisticRepository>();
            statisticRepository.AddOrUpdateForEntityAsync(nameof(GmailResource), _recoveryDays);
            return Task.CompletedTask;
        }
    }
}
