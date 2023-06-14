using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers.Statistics
{
    public  class AppleOrderStatisticWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly int _recoveryDays;

        public AppleOrderStatisticWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _recoveryDays = configuration.GetValue<int>("Workers:Statistics:RecoveryDays");
            timer.Period = configuration.GetValue<int>("Workers:Statistics:AppleOrder:Interval");
        }

        protected override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start AppleIdOrder Statistic Worker.");
            var statisticRepository = workerContext.ServiceProvider.GetRequiredService<IStatisticRepository>();
            statisticRepository.AddOrUpdateForEntityAsync($"{nameof(AppleOrder)}_{nameof(AddPaymentStatus)}", _recoveryDays);
            statisticRepository.AddOrUpdateForEntityAsync($"{nameof(AppleOrder)}_{nameof(LinkStatus)}", _recoveryDays);
            return Task.CompletedTask;
        }
    }
}
