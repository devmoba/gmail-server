using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.OwnerConfigs
{
    public interface IOwnerConfigAppService : ICrudAppService<
        OwnerConfigDto, 
        long, 
        OwnerConfigFilterDto, 
        CreateUpdateOwnerConfigDto, 
        CreateUpdateOwnerConfigDto>
    {

    }
}
