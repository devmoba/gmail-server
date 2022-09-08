using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers
{
    public class UncheckMailWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;

        public UncheckMailWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            timer.Period = _cfg.GetValue<int>("Workers:UncheckMailWorker:Interval");
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start uncheck mail worker: Do something...");
            var gmailRepository = workerContext.ServiceProvider.GetRequiredService<IGmailRepository>();
            var minuteSetUncheck = _cfg.GetValue<int>("Workers:UncheckMailWorker:MinuteSetUncheck");
            var currentDate = DateTime.Now;
            var uncheckTime = currentDate.AddMinutes(-minuteSetUncheck);
            var gmails = await gmailRepository.GetByCheckingTimeoutAsync(uncheckTime, Status.Checking);
            gmails.ForEach(x =>
            {
                x.Status = Status.Uncheck;
                x.TimeDiff = 0;
                x.Updated = DateTime.Now;
            });
            await gmailRepository.BulkUpdateAsync(gmails, new List<string>()
            {
                nameof(Gmail.Status),
                nameof(Gmail.TimeDiff),
                nameof(Gmail.Updated),
            });
            await Task.FromResult(1);
            Logger.LogInformation("Finish worker: Something done...");
        }
    }
}
