using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Hotmails;
using GmailServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace GmailServer.Background.Workers
{
    public class GetAndInsertHotmailWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly int _reserveQuantity;
        private readonly string _username;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly int _quantity;
        private readonly List<string> _mailCodes;
        private readonly Random random = new Random();

        public GetAndInsertHotmailWorker(AbpAsyncTimer timer, 
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            _httpClientFactory = httpClientFactory;
            _username = _cfg.GetValue<string>("Workers:GetAndInsertHotmailWorker:Username");
            _apiUrl = _cfg.GetValue<string>("Workers:GetAndInsertHotmailWorker:ApiConfig:ApiUrl");
            _apiKey = _cfg.GetValue<string>("Workers:GetAndInsertHotmailWorker:ApiConfig:ApiKey");
            _quantity = _cfg.GetValue<int>("Workers:GetAndInsertHotmailWorker:ApiConfig:Quantity");
            _reserveQuantity = _cfg.GetValue<int>("Workers:GetAndInsertHotmailWorker:ReserveQuantity");
            _mailCodes = _cfg.GetSection("Workers:GetAndInsertHotmailWorker:ApiConfig:MailCodes").Get<List<string>>(); 
            timer.Period = _cfg.GetValue<int>("Workers:GetAndInsertHotmailWorker:Interval");
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start get and insert hotmail worker: Do something...");
            var recoveryEmailRepository = workerContext.ServiceProvider.GetRequiredService<IRecoveryEmailRepository>();
            var checkReserveQuantity = await recoveryEmailRepository.IsReserveQuantityEnoughAsync(_reserveQuantity);
            if (!checkReserveQuantity)
            {
                var index = random.Next(_mailCodes.Count);
                var requestUri = $"{_apiUrl}?apikey={_apiKey}&mailcode={_mailCodes[index]}&quantity={_quantity}";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var hotmailResponse = JsonConvert.DeserializeObject<HotmailResponse>(content);
                    Logger.LogDebug($"Hotmail response: Code = {hotmailResponse.Code} -- Message = {hotmailResponse.Message}");
                    if (hotmailResponse.Code == 0)
                    {
                        var recoveryEmails = new List<RecoveryEmail>();
                        hotmailResponse.Data.Emails.ForEach(hm =>
                        {
                            recoveryEmails.Add(new RecoveryEmail()
                            {
                                Email = hm.Email,
                                Password = hm.Password,
                                Username = _username,
                                Created = DateTime.Now,
                                Status = RecoveryEmailStatus.Ready
                            });
                        });
                        await recoveryEmailRepository.BulkInsertAsync(recoveryEmails);
                    }
                }
            }
            await Task.FromResult(1);
            Logger.LogInformation("Finish worker: Something done...");
        }
    }
}
