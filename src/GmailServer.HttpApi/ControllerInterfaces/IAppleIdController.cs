using GmailServer.AppleIds;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IAppleIdController
    {
        Task<PagedResultDto<AppleIdDto>> GetListAsync(AppleIdFilterDto input);

        Task<AppleIdDto> GetFirstAppleIdAsync();

        Task DeleteAsync(long id);

        Task DeleteAllAsync();
    }
}
