using GmailServer.GmailTypes;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IGmailTypeController
    {
        Task<PagedResultDto<GmailTypeDto>> GetListAsync(GmailTypeFilterDto input);

        Task DeleteAsync(long id);
    }
}
