using GmailServer.AppleIdNones.Statistics;
using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.AppleIdNones
{
    public interface IAppleIdNoneAppService : ICrudAppService<
        AppleIdNoneGetOutputDto,
        AppleIdNoneGetListOutputDto,
        long,
        AppleIdNoneFilterDto,
        CreateUpdateAppleIdNoneDto,
        CreateUpdateAppleIdNoneDto>
    {
        Task CreateManyAsync(CreateManyAppleIdNoneInputDto input);

        Task DeleteAllAsync();

        Task DeleteAsync(DeleteFilter input);

        Task<AppleIdNoneGetOutputDto> GetFirstAppleIdNoneAsync();

        Task<AppleIdNoneGetOutputDto> GetByStatusAsync(AppleIdNoneStatus status);

        Task<AppleIdNoneGetOutputDto> UpdateStatusAsync(string email, AppleIdNoneStatus status);

        Task<PagedResultDto<AppleIdNoneStatisticDto>> GetStatisticAsync(AppleIdNoneStatisticFilterDto input);

        Task<List<UsernameSelectionDto>> GetUsernameSelectionAsync();

        Task<List<AppleIdNoneStatusSelectionDto>> GetAppleIdNoneStatusSelectionsAsync(string username, DateTime? createdFrom, DateTime? createdTo);

        Task<List<AppleIdNoneExcelModel>> GetAppleIdNoneExcelModelsAsync(AppleIdNoneDownloadFilter input);

        Task ResetStatusAsync(ResetStatusFilter input);

        Task<AppleIdNoneGetOutputDto> IncreasePurchaseAsync(string email);

        Task<AppleIdNoneGetOutputDto> SetTakenOutNumberAsync(string email, int value);

        Task<AppleIdNoneGetOutputDto> AddPaymentCompletedAsync(string email); 

        Task<AppleIdNoneGetOutputDto> UpdateRemoveStatusAsync(string email, RemovePaymentStatus status);

        Task<AppleIdNoneGetOutputDto> GetAppleIdToRemoveAsync();
    }
}
