using GmailServer.AppleIdNones;
using GmailServer.AppleIdNones.Statistics;
using GmailServer.Entities;
using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class AppleIdNoneAppService : CrudAppService<
        AppleIdNone,
        AppleIdNoneGetOutputDto,
        AppleIdNoneGetListOutputDto,
        long,
        AppleIdNoneFilterDto,
        CreateUpdateAppleIdNoneDto,
        CreateUpdateAppleIdNoneDto>, IAppleNoneAppService
    {
        public AppleIdNoneAppService(IRepository<AppleIdNone, long> repository) : base(repository)
        {
        }

        public override Task<PagedResultDto<AppleIdNoneGetListOutputDto>> GetListAsync(AppleIdNoneFilterDto input)
        {
            return base.GetListAsync(input);
        }

        public override Task<AppleIdNoneGetOutputDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        public Task<List<AppleIdNoneExcelModel>> GetAppleIdNoneExcelModelsAsync(AppleIdNoneDownloadFilter input)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppleIdNoneStatusSelectionDto>> GetAppleIdNoneStatusSelectionsAsync(string username, DateTime? createdFrom, DateTime? createdTo)
        {
            throw new NotImplementedException();
        }

        public Task<AppleIdNoneGetOutputDto> GetByStatusAsync(AppleIdNoneStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<AppleIdNoneGetOutputDto> GetFirstAppleIdNoneDto()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<AppleIdNoneStatisticDto>> GetStatisticAsync(AppleIdNoneStatisticFilterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsernameSelectionDto>> GetUsernameSelectionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppleIdNoneGetOutputDto> IncreasePurchaseAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<AppleIdNoneGetOutputDto> SetTakenOutNumberAsync(string email, int value)
        {
            throw new NotImplementedException();
        }

        public override Task<AppleIdNoneGetOutputDto> CreateAsync(CreateUpdateAppleIdNoneDto input)
        {
            return base.CreateAsync(input);
        }

        public Task CreateManyAsync(CreateManyAppleIdNoneInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<AppleIdNoneGetOutputDto> UpdateStatusAsync(string email, AppleIdNoneStatus status)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(DeleteFilter input)
        {
            throw new NotImplementedException();
        }

        public Task ResetStatusAsync(ResetStatusFilter input)
        {
            throw new NotImplementedException();
        }
    }
}
