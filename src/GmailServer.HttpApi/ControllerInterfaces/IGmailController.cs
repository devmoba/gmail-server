using GmailServer.Gmails;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IGmailController
    {
        Task<GmailDto> CreateAsync(CreateGmailDto input);
        Task<PagedResultDto<GmailDto>> GetListAsync(GmailFilterDto input);
    }
}
