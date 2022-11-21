using GmailServer.AppleIds;
using GmailServer.AppleIds.Statistics;
using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IAppleIdController
    {
        Task<PagedResultDto<AppleIdDto>> GetListAsync(AppleIdFilterDto input);

        Task<AppleIdDto> GetFirstAppleIdAsync();

        Task<AppleIdDto> GetByStatusAsync(AppleIdStatus status);

        Task DeleteAsync(long id);

        Task DeleteAllAsync();

        Task<AppleIdDto> UpdateStatusAsync(string email, AppleIdStatus status);

        Task<AppleIdDto> CreateAsync(string email, string password, string username);

        Task<List<AppleIdStatusSelectionDto>> GetAppleIdStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo, int? updatedHours = null);

        Task<PagedResultDto<AppleIdStatisticDto>> GetStatisticAsync(AppleIdStatisticFilterDto input);

        Task<PagedResultDto<AppleIdStatisticDailyDto>> GetStatisticDailyAsync(AppleIdStatisticDailyFilterDto input);

        Task<StatisticByUsernameDto> GetStatisticByUsernameAsync();
    }
}
