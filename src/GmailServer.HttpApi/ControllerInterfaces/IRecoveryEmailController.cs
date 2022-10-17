using GmailServer.RecoveryEmails;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IRecoveryEmailController
    {
        Task<PagedResultDto<RecoveryEmailDto>> GetListAsync(RecoveryEmailFilterDto input);

        Task<RecoveryEmailDto> GetRecoveryEmailRandomAsync();

        Task<RecoveryEmailDto> GetFirstRecoveryEmailAsync();

        Task<RecoveryEmailDto> CreateAsync(CreateUpdateRecoveryEmailDto input);

        Task DeleteAsync(long id);

        Task DeleteAllAsync();

        Task<RecoveryEmailReportStatusDto> GetRecoveryEmailReportAsync();
    }
}
