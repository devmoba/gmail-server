using GmailServer.EmailChecks;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.Hubs;
using GmailServer.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers
{
    public class CheckMailReportWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;
        private const string ConnectionName = "CheckMailTool";
        private readonly IHubContext<CheckMailHub, ICheckMailHub> _checkMailHub;

        public CheckMailReportWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration,
            IHubContext<CheckMailHub, ICheckMailHub> checkMailHub) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            _checkMailHub = checkMailHub;
            timer.Period = _cfg.GetValue<int>("Workers:CheckMailReportWoker:Interval");
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start check mail report worker: Do something...");
            var connections = ConnectionMapping<string>.GetInstance().GetConnections(ConnectionName).ToList();

            if (connections.Count > 0)
            {
                var hourToChecks = _cfg.GetSection("Workers:CheckMailReportWoker:HourToChecks").Get<List<int>>();
                var gmailRepository = workerContext.ServiceProvider.GetRequiredService<IGmailRepository>();
                var emailChecks = new List<EmailCheck>();

                foreach (var hour in hourToChecks)
                {
                    var current = DateTime.Now;

                    var gmails = await gmailRepository.GetByTimeToCheckAsync(hour);
                    gmails.ForEach(gmail =>
                    {
                        gmail.Status = Status.Checking;
                        gmail.LastCheck = current;
                        gmail.TimeDiff = current.Subtract(gmail.Created).TotalHours;
                    });

                    emailChecks.AddRange(gmails.Select(x => new EmailCheck()
                    {
                        Email = x.Email,
                        Id = x.Id
                    }).ToList());

                    await gmailRepository.BulkUpdateAsync(gmails, new List<string>()
                    {
                        nameof(Gmail.Status),
                        nameof(Gmail.LastCheck),
                        nameof(Gmail.TimeDiff)
                    });
                }

                var emailCheckSplit = EnumerableExtension.Split<EmailCheck>(
                    emailChecks.DistinctBy(x => x.Id).ToList(),
                    connections.Count)
                    .ToList();

                for (int i = 0; i < connections.Count; i++)
                {
                    await _checkMailHub
                        .Clients
                        .Client(connections[i])
                        .ReceiveEmailCheckAsync(emailCheckSplit[i]);
                }
            }

            await Task.FromResult(1);
            Logger.LogInformation("Finish worker: Something done...");
        }
    }
}
