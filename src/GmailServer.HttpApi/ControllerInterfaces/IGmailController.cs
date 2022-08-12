using GmailServer.Gmails;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IGmailController
    {
        Task<GmailDto> CreateAsync(CreateGmailDto input);

        Task<PagedResultDto<GmailDto>> GetListAsync(GmailFilterDto input);

        Task DeleteAsync(long id);

        Task<PagedResultDto<GmailReportDto>> GetGmailReportsAsync(GmailReportFilterDto input);

        Task<ReportbyStatusDto> GetReportbyStatusAsync();
    }
}
