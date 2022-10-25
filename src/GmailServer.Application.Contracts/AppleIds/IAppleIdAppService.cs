using GmailServer.AppleIds.Statistics;
using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.AppleIds
{
    public interface IAppleIdAppService : ICrudAppService<
        AppleIdDto, 
        long, 
        AppleIdFilterDto, 
        CreateUpdateAppleIdDto, 
        CreateUpdateAppleIdDto>
    {
        Task CreateManyAsync(CreateManyAppleIdInputDto input);

        Task DeleteAllAsync();

        Task<AppleIdDto> GetFirstAppleIdAsync();

        Task<AppleIdDto> GetByStatusAsync(AppleIdStatus status);

        Task<AppleIdDto> UpdateStatusAsync(string email, AppleIdStatus status);

        Task<List<string>> GetUsernameSelectionAsync();

        Task<List<AppleIdStatusSelectionDto>> GetAppleIdStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo);

        Task<List<AppleIdExcelModel>> GetAppleIdExcelModelsAsync(AppleIdDownloadFilter input);

        Task<PagedResultDto<AppleIdStatisticDto>> GetStatisticAsync(AppleStatisticFilterDto input);

        Task<PagedResultDto<AppleStatisticDailyDto>> GetStatisticDailyAsync(AppleIdStatisticDailyFilterDto input);

        Task ResetStatusAsync(List<AppleIdStatus> statuses, int? hour = null, AppleIdStatus targetStatus = AppleIdStatus.Ready);

        Task<StatisticByUsernameDto> GetStatisticByUsernameAsync();
    }
}
