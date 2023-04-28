using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.AppleIdRaws
{
    public interface IAppleIdRawAppService : ICreateAppService<AppleIdRawDto, CreateAppleIdRawInputDto>
    {
        Task<PagedResultDto<AppleIdRawStatisticDailyDto>> GetAppleIdRawStatisticDailyAsync(AppleIdRawStatisticFilterDto input);
    }
}
