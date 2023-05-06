using GmailServer.Constants;
using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Hotmails;
using GmailServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private int _reserveQuantity;
        private string _username;
        private string _apiKey;
        private string _apiUrl;
        private int _quantity;
        private List<string> _mailCodes;
        private readonly Random random = new Random();

        public GetAndInsertHotmailWorker(AbpAsyncTimer timer, 
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory) : base(timer, serviceScopeFactory)
        {
            _cfg = configuration;
            _httpClientFactory = httpClientFactory;
            _quantity = 1;
            _reserveQuantity = 10000;
            timer.Period = _cfg.GetValue<int>("Workers:GetAndInsertHotmailWorker:Interval");
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start get and insert hotmail worker: Do something...");
            var ownerConfigRepository = workerContext.ServiceProvider.GetRequiredService<IOwnerConfigRepository>();
            var recoveryEmailRepository = workerContext.ServiceProvider.GetRequiredService<IRecoveryEmailRepository>();

            _username = (await ownerConfigRepository.GetByKeyAsync(RecoveryEmailCfg.USERNAME)).Value;
            _apiUrl = (await ownerConfigRepository.GetByKeyAsync(RecoveryEmailCfg.API_URL)).Value; ;
            _apiKey = (await ownerConfigRepository.GetByKeyAsync(RecoveryEmailCfg.API_KEY)).Value;
            var quantityStr = (await ownerConfigRepository.GetByKeyAsync(RecoveryEmailCfg.QUANTITY)).Value;
            int.TryParse(quantityStr, out _quantity);
            var reserveQuantityStr = (await ownerConfigRepository.GetByKeyAsync(RecoveryEmailCfg.RESERVE_QUANTITY)).Value;
            int.TryParse(reserveQuantityStr, out _reserveQuantity);
            var mailCodesStr = (await ownerConfigRepository.GetByKeyAsync(RecoveryEmailCfg.MAILCODES)).Value; ;
            _mailCodes = mailCodesStr.Split("|").ToList();

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
