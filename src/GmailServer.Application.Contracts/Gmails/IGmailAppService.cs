using Volo.Abp.Application.Services;

namespace GmailServer.Gmails
{
    public interface IGmailAppService : IReadOnlyAppService<
        GmailDto, 
        long, 
        GmailFilterDto>, ICreateAppService<GmailDto, CreateGmailDto>
    {

    }
}
