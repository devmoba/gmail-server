using GmailServer.DownloadedApps;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IDownloadedAppController
    {
        Task<DownloadedAppGetOutputDto> CreateAsync(CreateDownloadedAppDto input);

        Task<PagedResultDto<DownloadedAppGetListOutputDto>> GetListAsync(DownloadAppFilterDto input);

        Task DeleteAsync(long id);
    }
}
