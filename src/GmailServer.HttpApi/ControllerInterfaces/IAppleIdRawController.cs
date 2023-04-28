using GmailServer.AppleIdRaws;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IAppleIdRawController
    {
        Task<PagedResultDto<AppleIdRawStatisticDailyDto>> GetAppleIdRawStatisticDailyAsync(AppleIdRawStatisticFilterDto input);

        Task<AppleIdRawDto> CreateAsync(CreateAppleIdRawInputDto input);
    }
}
