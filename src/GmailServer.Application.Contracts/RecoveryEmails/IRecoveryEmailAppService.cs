using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.RecoveryEmails
{
    public interface IRecoveryEmailAppService : ICrudAppService<
        RecoveryEmailDto, 
        long,
        RecoveryEmailFilterDto, 
        CreateUpdateRecoveryEmailDto, 
        CreateUpdateRecoveryEmailDto>
    {
        Task<RecoveryEmailDto> GetRecoveryEmailRandomAsync();
    }
}
