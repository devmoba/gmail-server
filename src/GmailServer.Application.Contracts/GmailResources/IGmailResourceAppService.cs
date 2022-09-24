using GmailServer.Enums;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.GmailResources
{
    public interface IGmailResourceAppService : ICrudAppService<
        GmailResourceDto, 
        long, 
        GmailResourceFilterDto, 
        CreateUpdateGmailResourceDto, 
        CreateUpdateGmailResourceDto>
    {
        Task CreateManyAsync(CreateManyGmailResourceInputDto input);

        Task DeleteAllAsync();

        Task<GmailResourceDto> GetFirstGmailResourceAsync();

        Task<GmailResourceDto> UpdateStatusAsync(string email, GmailResourceStatus status);
    }
}
