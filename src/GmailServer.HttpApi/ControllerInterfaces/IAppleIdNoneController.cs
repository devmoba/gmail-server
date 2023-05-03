using GmailServer.AppleIdNones;
using GmailServer.AppleIdNones.Statistics;
using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IAppleIdNoneController 
    {
        Task<PagedResultDto<AppleIdNoneGetListOutputDto>> GetListAsync(AppleIdNoneFilterDto input);

        Task<AppleIdNoneGetOutputDto> GetFirstAppleIdNoneAsync(bool isNone = false);

        Task<AppleIdNoneGetOutputDto> GetByStatusAsync(AppleIdNoneStatus status);

        Task DeleteAllAsync();

        Task DeleteAsync(long id);

        Task<AppleIdNoneGetOutputDto> UpdateStatusAsync(string email, AppleIdNoneStatus status);

        Task<AppleIdNoneGetOutputDto> CreateAsync(string email, string password, string username, string ccv = null
           , string secretAnswer1 = default, string secretAnswer2 = default, string secretAnswer3 = default, string dob = default);

        Task<PagedResultDto<AppleIdNoneStatisticDto>> GetStatisticAsync(AppleIdNoneStatisticFilterDto input);

        Task<List<AppleIdNoneStatusSelectionDto>> GetAppleIdNoneStatusSelectionsAsync(string username, DateTime? createdFrom, DateTime? createdTo);

        Task<List<AppleIdNoneRemoveStatusSelectionDto>> GetAppleIdNoneRemoveStatusSelectionsAsync(
            string username,
            DateTime? createdFrom,
            DateTime? createdTo,
            DateTime? removeTakenTimeFrom,
            DateTime? removeTakenTimeTo);

        Task<AppleIdNoneGetOutputDto> IncreasePurchaseAsync(string email);

        Task<AppleIdNoneGetOutputDto> SetTakenOutNumberAsync(string email, int value);

        Task<AppleIdNoneGetOutputDto> AddPaymentCompletedAsync(string email);

        Task<AppleIdNoneGetOutputDto> UpdateRemoveStatusAsync(string email, RemovePaymentStatus status);

        Task<AppleIdNoneGetOutputDto> GetAppleIdToRemoveAsync();
    }
}
