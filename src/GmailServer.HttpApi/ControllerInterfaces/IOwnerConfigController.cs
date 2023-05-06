using GmailServer.OwnerConfigs;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IOwnerConfigController
    {
        Task<PagedResultDto<OwnerConfigDto>> GetListAsync(OwnerConfigFilterDto input);

        Task DeleteAsync(long id);
    }
}
