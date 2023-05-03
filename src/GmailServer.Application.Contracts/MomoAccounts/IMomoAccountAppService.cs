using GmailServer.Enums;
using GmailServer.MomoAccounts.Statistics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.MomoAccounts
{
    public interface IMomoAccountAppService : 
        IReadOnlyAppService<MomoAccountDto, long, MomoAccountFilterDto>,
        ICreateAppService<MomoAccountDto, CreateMomoAccountInputDto>,
        IDeleteAppService<long>
    {
        Task<MomoAccountDto> GetMomoAcountAsync();

        Task CreateManyAsync(CreateManyMomoAccountInputDto input);

        Task<MomoAccountDto> UpdateMomoAcountAsync(string username, UpdateMomoAccountInputDto input);

        Task<MomoAccountDto> UpdateStatusAsync(string username, MomoAccountStatus status);

        Task<List<UploadGroupSelectionDto>> GetUploadGroupSelectionAsync();

        Task<PagedResultDto<MomoAccountStatisticDto>> GetStatisticAsync(MomoAccountStatisticFilterDto input);

        Task<List<MomoAccountStatusSelectionDto>> GetMomoAcountStatusSelectionsAsync(string uploadGroup, DateTime? createdFrom, DateTime? createdTo);

        Task ResetStatusAsync(ResetStatusFilterInput input);

        Task DeleteFilterAsync(DeleteFilterInput input);

        Task DeleteAllAsync();

        Task<MomoAccountDto> IncreaseLinkCountAsync(string username);
    }
}
