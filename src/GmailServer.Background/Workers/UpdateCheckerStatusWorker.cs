using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers
{
    public class UpdateCheckerStatusWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;
        public UpdateCheckerStatusWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            timer.Period = _cfg.GetValue<int>("Workers:UpdateCheckerStatusWorker:Interval");
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start update checker status worker: Do something...");
            var checkerRepository = workerContext.ServiceProvider.GetRequiredService<ICheckerRepository>();
            var timeout = _cfg.GetValue<int>("Workers:UpdateCheckerStatusWorker:TimeoutOffline");
            await checkerRepository.UpdateStatusByTimeoutAsync(timeout);
            await Task.FromResult(1);
            Logger.LogInformation("Finish worker: Something done...");
        }
    }
}
