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
        AppleIdGetOutputDto,
        AppleIdGetListOutputDto,
        long, 
        AppleIdFilterDto, 
        CreateUpdateAppleIdDto, 
        CreateUpdateAppleIdDto>
    {
        Task CreateManyAsync(CreateManyAppleIdInputDto input);

        Task DeleteAllAsync();

        Task<AppleIdGetOutputDto> GetFirstAppleIdAsync();

        Task<AppleIdGetOutputDto> GetByStatusAsync(AppleIdStatus status);

        Task<AppleIdGetOutputDto> UpdateStatusAsync(string email, AppleIdStatus status);

        Task<List<UsernameSelectionDto>> GetUsernameSelectionAsync();

        Task<List<AppleIdStatusSelectionDto>> GetAppleIdStatusSelectionAsync(string username, DateTime? createdFrom, DateTime? createdTo, int? updatedHours = null);

        Task<List<AppleIdExcelModel>> GetAppleIdExcelModelsAsync(AppleIdDownloadFilter input);

        Task<PagedResultDto<AppleIdStatisticDto>> GetStatisticAsync(AppleIdStatisticFilterDto input);

        Task<PagedResultDto<AppleIdStatisticDailyDto>> GetStatisticDailyAsync(AppleIdStatisticDailyFilterDto input);

        Task ResetStatusAsync(ResetStatusFilter input);

        Task<StatisticByUsernameDto> GetStatisticByUsernameAsync();

        Task DeleteAsync(DeleteFilter input);

        Task<AppleIdGetOutputDto> IncreasePurchaseAsync(string email);

        Task<AppleIdGetOutputDto> SetTakenOutNumberAsync(string email, int value);
    }
}
