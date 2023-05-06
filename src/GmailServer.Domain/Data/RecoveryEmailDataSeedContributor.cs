using GmailServer.Constants;
using GmailServer.Entities;
using GmailServer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Data
{
    public class RecoveryEmailDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IOwnerConfigRepository _repository;

        public RecoveryEmailDataSeedContributor(IOwnerConfigRepository repository)
        {
            _repository = repository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedDataAsync();
        }

        private async Task SeedDataAsync()
        {
            var configs = new List<OwnerConfig>();
            var anyReserveQuantity = await _repository.AnyAsync(x => x.Key == RecoveryEmailCfg.RESERVE_QUANTITY);
            if (!anyReserveQuantity)
            {
                configs.Add(new OwnerConfig()
                {
                    Key = RecoveryEmailCfg.RESERVE_QUANTITY,
                    Value = "30000"
                });
            }
            var anyUsername = await _repository.AnyAsync(x => x.Key == RecoveryEmailCfg.USERNAME);
            if (!anyUsername)
            {
                configs.Add(new OwnerConfig()
                {
                    Key = RecoveryEmailCfg.USERNAME,
                    Value = "dviet92"
                });
            }
            var anyMailCodes = await _repository.AnyAsync(x => x.Key == RecoveryEmailCfg.MAILCODES);
            if (!anyMailCodes)
            {
                configs.Add(new OwnerConfig()
                {
                    Key = RecoveryEmailCfg.MAILCODES,
                    Value = "hotmail|outlook"
                });
            }
            var anyApiUrl = await _repository.AnyAsync(x => x.Key == RecoveryEmailCfg.API_URL);
            if (!anyApiUrl)
            {
                configs.Add(new OwnerConfig()
                {
                    Key = RecoveryEmailCfg.API_URL,
                    Value = "https://api.hotmailbox.me/mail/buy"
                });
            }
            var anyApiKey = await _repository.AnyAsync(x => x.Key == RecoveryEmailCfg.API_KEY);
            if (!anyApiKey)
            {
                configs.Add(new OwnerConfig()
                {
                    Key = RecoveryEmailCfg.API_KEY,
                    Value = "2ECEF88227074087840C177074029602"
                });
            }
            var anyQuantity = await _repository.AnyAsync(x => x.Key == RecoveryEmailCfg.QUANTITY);
            if (!anyApiKey)
            {
                configs.Add(new OwnerConfig()
                {
                    Key = RecoveryEmailCfg.QUANTITY,
                    Value = "50"
                });
            }
            await _repository.InsertManyAsync(configs);
        }
    }
}
