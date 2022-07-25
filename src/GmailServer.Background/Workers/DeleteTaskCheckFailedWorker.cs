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
    public class DeleteTaskCheckFailedWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;

        public DeleteTaskCheckFailedWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            timer.Period = _cfg.GetValue<int>("Workers:CheckMailReportWoker:Interval");
        }
        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start check mail report worker: Do something...");
            var taskCheckRepository = workerContext.ServiceProvider.GetRequiredService<ITaskCheckRepository>();
            var timeCheckDelete = _cfg.GetValue<int>("Workers:DeleteTaskCheckFailedWorker:CheckDelete");

            await Task.FromResult(1);
            Logger.LogInformation("Finish worker: Something done...");
            throw new NotImplementedException();
        }
    }
}
