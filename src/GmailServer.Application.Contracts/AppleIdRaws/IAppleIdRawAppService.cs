using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.AppleIdRaws
{
    public interface IAppleIdRawAppService : ICreateAppService<AppleIdRawDto, CreateAppleIdRawInputDto>
    {
        Task<List<AppleIdRawDto>> DownloadByCreatedAsync(DateTime? createdFrom, DateTime? createdTo);

        Task<PagedResultDto<AppleIdRawStatisticDailyDto>> GetAppleIdRawStatisticDailyAsync(AppleIdRawStatisticFilterDto input);
    }
}
