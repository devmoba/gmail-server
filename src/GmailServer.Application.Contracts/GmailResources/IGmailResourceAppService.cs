using GmailServer.Enums;
using GmailServer.GmailResources.Statistics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.GmailResources
{
    public interface IGmailResourceAppService : ICrudAppService<
        GmailResourceDto, 
        long, 
        GmailResourceFilterDto, 
        CreateUpdateGmailResourceDto, 
        CreateUpdateGmailResourceDto>
    {
        Task CreateManyAsync(CreateManyGmailResourceInputDto input);

        Task DeleteAllAsync();

        Task<GmailResourceDto> GetFirstGmailResourceAsync();

        Task<List<GmailResourceExcelModel>> GetGmailResourceExcelModelsAsync(GmailResourceDownloadFilter input);

        Task<GmailResourceDto> GetByStatusAsync(GmailResourceStatus status);

        Task<List<string>> GetUsernameSelectionAsync();

        Task<List<GmailResourceStatusSelectionDto>> GetGmailResourceStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo, int? updatedHours = null);

        Task<StatisticByUsernameDto> GetStatisticByUsernameAsync();

        Task<PagedResultDto<GmailResourceStatisticDto>> GetStatisticAsync(GmailResourceStatisticFilterDto input);

        Task<PagedResultDto<GmailResourceStatisticDailyDto>> GetStatisticDailyAsync(GmailResourceStatisticDailyFilterDto input);

        Task<GmailResourceDto> UpdateStatusAsync(string email, GmailResourceStatus status);

        Task ResetStatusAsync(ResetStatusFilter input);

        Task ReupAsync(ReupGmailResourceInputDto input);

        Task DeleteAsync(DeleteFilter input);
    }
}
