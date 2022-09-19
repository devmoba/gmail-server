using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers
{
    public class DeleteRecoveryEmailCompletedWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;
        public DeleteRecoveryEmailCompletedWorker(AbpAsyncTimer timer, 
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            timer.Period = _cfg.GetValue<int>("Workers:DeleteRecoveryEmailCompletedWorker:Interval");
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start delete recovery email completed worker: Do something...");
            var recoveryEmailRepository = workerContext.ServiceProvider.GetRequiredService<IRecoveryEmailRepository>();
            var timeCheckDelete = _cfg.GetValue<int>("Workers:DeleteRecoveryEmailCompletedWorker:CheckDelete");
            await recoveryEmailRepository.DeleteRecoveryEmailCompleted(timeCheckDelete);
            await Task.FromResult(1);
            Logger.LogInformation("Finish worker: Something done...");
        }
    }
}
