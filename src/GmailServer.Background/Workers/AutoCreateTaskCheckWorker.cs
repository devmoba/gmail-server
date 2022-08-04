using GmailServer.EmailChecks;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers
{
    public class AutoCreateTaskCheckWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;

        public AutoCreateTaskCheckWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            timer.Period = _cfg.GetValue<int>("Workers:AutoCreateTaskCheckWorker:Interval");
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start check mail report worker: Do something...");
            var hourToChecks = _cfg.GetSection("Workers:AutoCreateTaskCheckWorker:HourToChecks").Get<List<int>>();
            var limit = _cfg.GetValue<int>("CheckMail:MailPerTaskCheck");
            var gmailRepository = workerContext.ServiceProvider.GetRequiredService<IGmailRepository>();
            var checkerRepository = workerContext.ServiceProvider.GetRequiredService<ICheckerRepository>();
            var taskCheckRepository = workerContext.ServiceProvider.GetRequiredService<ITaskCheckRepository>();
            var gmailEntities = new List<Gmail>();
            var checker = await checkerRepository.GetCheckerOnlineFirstAsync();
            if (checker != null)
            {
                var current = DateTime.Now;

                foreach (var hour in hourToChecks)
                {
                    var gmails = await gmailRepository.GetByTimeToCheckAsync(hour, maxCount: 50);

                    gmailEntities.AddRange(gmails);
                    gmails.ForEach(gmail =>
                    {
                        gmail.Status = Status.Checking;
                        gmail.LastCheck = current;
                        gmail.TimeDiff = current.Subtract(gmail.Created).TotalHours;
                    });

                    await gmailRepository.BulkUpdateAsync(gmails, new List<string>()
                    {
                        nameof(Gmail.Status),
                        nameof(Gmail.LastCheck),
                        nameof(Gmail.TimeDiff)
                    });
                }

                if (gmailEntities.Count > 0)
                {
                    var emailChecks = gmailEntities.Select(x => new EmailCheck()
                    {
                        Email = x.Email,
                        Id = x.Id
                    }).ToList();

                    await taskCheckRepository.InsertAsync(new TaskCheck()
                    {
                        CheckerId = checker.Id,
                        Username = "Database",
                        EmailChecks = JsonConvert.SerializeObject(emailChecks),
                        Status = TaskCheckStatus.NA,
                        TypeCheck = TypeCheck.OwnerDB,
                        Created = DateTime.Now
                    }, autoSave: true);

                    gmailEntities.ForEach(gmail =>
                    {
                        gmail.Status = Status.Checking;
                        gmail.LastCheck = current;
                        gmail.TimeDiff = current.Subtract(gmail.Created).TotalHours;
                    });

                    await gmailRepository.BulkUpdateAsync(gmailEntities, new List<string>()
                    {
                        nameof(Gmail.Status),
                        nameof(Gmail.LastCheck),
                        nameof(Gmail.TimeDiff)
                    });
                }
            }
            await Task.FromResult(1);
            Logger.LogInformation("Finish worker: Something done...");
        }
    }
}
