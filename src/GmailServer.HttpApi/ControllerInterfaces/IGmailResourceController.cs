using GmailServer.Enums;
using GmailServer.GmailResources;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IGmailResourceController
    {
        Task<PagedResultDto<GmailResourceDto>> GetListAsync(GmailResourceFilterDto input);

        Task DeleteAllAsync();

        Task DeleteAsync(long id);

        Task<GmailResourceDto> GetFirstGmailPremiumAsync();

        Task<GmailResourceDto> UpdateStatusAsync(string email, GmailResourceStatus status);
    }
}
