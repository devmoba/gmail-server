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
        Task<PagedResultDto<AppleIdGetListOutputDto>> GetListAsync(AppleIdFilterDto input);

        Task<AppleIdGetOutputDto> GetFirstAppleIdAsync();
          
        Task<AppleIdGetOutputDto> GetByStatusAsync(AppleIdStatus status);

        Task DeleteAsync(long id);

        Task DeleteAllAsync();

        Task<AppleIdGetOutputDto> UpdateStatusAsync(string email, AppleIdStatus status);

        Task<AppleIdGetOutputDto> CreateAsync(string email, string password, string username);

        Task<List<AppleIdStatusSelectionDto>> GetAppleIdStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo, int? updatedHours = null);

        Task<PagedResultDto<AppleIdStatisticDto>> GetStatisticAsync(AppleIdStatisticFilterDto input);

        Task<PagedResultDto<AppleIdStatisticDailyDto>> GetStatisticDailyAsync(AppleIdStatisticDailyFilterDto input);

        Task<StatisticByUsernameDto> GetStatisticByUsernameAsync();

        Task<AppleIdGetOutputDto> IncreasePurchaseAsync(string email);
    }
}
