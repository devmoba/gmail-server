using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.AppleIds
{
    public interface IAppleIdAppService : ICrudAppService<
        AppleIdDto, 
        long, 
        AppleIdFilterDto, 
        CreateUpdateAppleIdDto, 
        CreateUpdateAppleIdDto>
    {
        Task CreateManyAsync(CreateManyAppleIdInputDto input);

        Task DeleteAllAsync();

        Task<AppleIdDto> GetFirstAppleIdAsync();
    }
}
