using GmailServer.RecoveryEmails;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IRecoveryEmailController
    {
        Task<PagedResultDto<RecoveryEmailDto>> GetListAsync(RecoveryEmailFilterDto input);

        Task<RecoveryEmailDto> GetRecoveryEmailRandomAsync();

        Task DeleteAsync(long id);
    }
}
