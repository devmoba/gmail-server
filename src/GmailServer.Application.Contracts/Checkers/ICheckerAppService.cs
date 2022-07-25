using Volo.Abp.Application.Services;

namespace GmailServer.Checkers
{
    public interface ICheckerAppService : IReadOnlyAppService<
        CheckerDto, 
        long, 
        CheckerFilterDto>, IDeleteAppService<long>
    {

    }
}
