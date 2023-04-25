using GmailServer.MomoAccounts.Statistics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.MomoAccounts
{
    public interface IMomoAccountAppService : 
        IReadOnlyAppService<MomoAccountDto, long, MomoAccountFilterDto>,
        IDeleteAppService<long>
    {
        Task<MomoAccountDto> GetMomoAcountAsync();

        Task CreateManyAsync(CreateManyMonoAccountInputDto input);

        Task<MomoAccountDto> UpdateMomoAcountAsync(string username, UpdateMomoAccountInputDto input);

        Task<List<MomoAccountStatisticDto>> GetStatisticAsync(MomoAccountFilterDto input);

        Task<List<MomoAccountStatusSelectionDto>> GetMomoAcountStatusSelectionsAsync(string username, DateTime? createdFrom, DateTime? createdTo);

        Task ResetStatusAsync(ResetStatusFilterInput input);

        Task DeleteAsync(DeleteFilterInput input);

        Task DeleteAllAsync();
    }
}
