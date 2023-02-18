using GmailServer.Enums;
using GmailServer.GmailResources;
using GmailServer.GmailResources.Statistics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IGmailResourceController
    {
        Task<PagedResultDto<GmailResourceDto>> GetListAsync(GmailResourceFilterDto input);

        Task DeleteAllAsync();

        Task DeleteAsync(long id);

        Task<List<string>> GetUsernameSelectionAsync();

        Task<GmailResourceDto> GetFirstGmailResourceAsync();

        Task<GmailResourceDto> UpdateStatusAsync(string email, GmailResourceStatus status);

        Task<GmailResourceDto> CreateAsync(CreateUpdateGmailResourceDto input);

        Task<GmailResourceDto> GetByStatusAsync(GmailResourceStatus status);

        Task<StatisticByUsernameDto> GetStatisticByUsernameAsync();

        Task<List<GmailResourceStatusSelectionDto>> GetGmailResourceStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo, int? updatedHours = null);

        Task<PagedResultDto<GmailResourceStatisticDto>> GetStatisticAsync(GmailResourceStatisticFilterDto input);

        Task<PagedResultDto<GmailResourceStatisticDailyDto>> GetStatisticDailyAsync(GmailResourceStatisticDailyFilterDto input);
    }
}
