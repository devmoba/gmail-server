using GmailServer.GmailPremiums;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IGmailPremiumController
    {
        Task<PagedResultDto<GmailPremiumDto>> GetListAsync(GmailPremiumFilterDto input);

        Task<GmailPremiumDto> GetFirstGmailPremiumAsync();

        Task DeleteAsync(long id);

        Task DeleteAllAsync();

        Task<GmailPremiumDto> CreateAsync(CreateUpdateGmailPremiumDto input);
    }
}
