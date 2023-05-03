using GmailServer.MomoAccounts;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using GmailServer.MomoAccounts.Statistics;
using GmailServer.Enums;

namespace GmailServer.ControllerInterfaces
{
    public interface IMomoAccountController
    {
        Task<PagedResultDto<MomoAccountDto>> GetListAsync(MomoAccountFilterDto input);

        Task<MomoAccountDto> GetAsync(long id);

        Task<MomoAccountDto> GetMomoAcountAsync();

        Task<List<MomoAccountStatusSelectionDto>> GetMomoAcountStatusSelectionsAsync(string uploadGroup, DateTime? createdFrom, DateTime? createdTo);

        Task<PagedResultDto<MomoAccountStatisticDto>> GetStatisticAsync(MomoAccountStatisticFilterDto input);

        Task<MomoAccountDto> CreateAsync(CreateMomoAccountInputDto input);

        Task<MomoAccountDto> UpdateMomoAcountAsync(string username, UpdateMomoAccountInputDto input);

        Task<MomoAccountDto> UpdateStatusAsync(string username, MomoAccountStatus status);

        Task DeleteAllAsync();

        Task DeleteAsync(long id);

        Task<MomoAccountDto> IncreaseLinkCountAsync(string username);
    }
}
