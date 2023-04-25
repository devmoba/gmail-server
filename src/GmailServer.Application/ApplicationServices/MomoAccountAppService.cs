using GmailServer.Entities;
using GmailServer.MomoAccounts;
using GmailServer.MomoAccounts.Statistics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class MomoAccountAppService : ReadOnlyAppService<
        MomoAccount,
        MomoAccountDto,
        long, MomoAccountFilterDto>, IMomoAccountAppService
    {
        public MomoAccountAppService(IReadOnlyRepository<MomoAccount, long> repository) : base(repository)
        {
        }

        public Task<MomoAccountDto> GetMomoAcountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<MomoAccountStatusSelectionDto>> GetMomoAcountStatusSelectionsAsync(string username, DateTime? createdFrom, DateTime? createdTo)
        {
            throw new NotImplementedException();
        }

        public Task<List<MomoAccountStatisticDto>> GetStatisticAsync(MomoAccountFilterDto input)
        {
            throw new NotImplementedException();
        }

        public Task CreateManyAsync(CreateManyMonoAccountInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<MomoAccountDto> UpdateMomoAcountAsync(string username, UpdateMomoAccountInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(DeleteFilterInput input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task ResetStatusAsync(ResetStatusFilterInput input)
        {
            throw new NotImplementedException();
        }
    }
}
