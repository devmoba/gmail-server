using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.GmailPremiums
{
    public interface IGmailPremiumAppService : ICrudAppService<
        GmailPremiumDto, 
        long, 
        GmailPremiumFilterDto, 
        CreateUpdateGmailPremiumDto, 
        CreateUpdateGmailPremiumDto>
    {
        Task CreateManyAsync(CreateManyGmailPremiumInputDto input);

        Task DeleteAllAsync();

        Task<GmailPremiumDto> GetFirstGmailPremiumAsync();
    }
}
