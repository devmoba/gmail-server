using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers.Statistics
{
    public class AppleIdStatisticWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly bool _isRecover;
        private readonly int _recoveryDate;

        public AppleIdStatisticWorker(AbpAsyncTimer timer, 
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _isRecover = configuration.GetValue<bool>("Workers:Statistics:Recover:Enable");
            _recoveryDate = configuration.GetValue<int>("Workers:Statistics:Recover:Days");
            timer.Period = configuration.GetValue<int>("Workers:Statistics:AppleId:Interval");
        }

        protected override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start AppleId Statistic Worker.");
            var statisticRepository = workerContext.ServiceProvider.GetRequiredService<IStatisticRepository>();
            if (_isRecover)
                statisticRepository.AddOrUpdateForEntityAsync(_recoveryDate);
            else
                statisticRepository.AddOrUpdateForAppleIdAsync(DateTime.Now.Date);
            return Task.CompletedTask;
        }
    }
}
