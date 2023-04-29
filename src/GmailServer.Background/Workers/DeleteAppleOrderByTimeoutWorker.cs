using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers
{
    public class DeleteAppleOrderByTimeoutWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;

        public DeleteAppleOrderByTimeoutWorker(AbpAsyncTimer timer, 
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration cfg) : base(timer, serviceScopeFactory)
        {
            _cfg = cfg;
            timer.Period = _cfg.GetValue<int>("Workers:DeleteAppleOrderByTimeoutWorker:Interval");
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start delete Apple Order by timeout worker: Do something...");
            var repository = workerContext.ServiceProvider.GetRequiredService<IAppleOrderRepository>();
            var timeCheck = _cfg.GetValue<int>("Workers:DeleteAppleOrderByTimeoutWorker:CheckDelete");
            await repository.DeleteAppleOrderByTimeoutAsync(timeCheck);
            Logger.LogInformation("Finish worker: Something done...");
        }
    }
}
