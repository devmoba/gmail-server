using GmailServer.Checkers;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface ICheckerController
    {
        Task<PagedResultDto<CheckerDto>> GetListAsync(CheckerFilterDto input);

        Task<CheckerDto> GetAsync(long id);
    }
}
