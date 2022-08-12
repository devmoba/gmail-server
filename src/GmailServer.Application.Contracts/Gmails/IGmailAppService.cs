using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace GmailServer.Gmails
{
    public interface IGmailAppService : IReadOnlyAppService<
        GmailDto, 
        long, 
        GmailFilterDto>, 
        ICreateAppService<GmailDto, CreateGmailDto>, 
        IDeleteAppService<long>
    {
        Task<List<GmailDto>> GetByTimeRange(DateTime? from, DateTime? to);

        Task<List<GmailDto>> GetAll();

        Task<PagedResultDto<GmailReportDto>> GetGmailReportsAsync(GmailReportFilterDto input);

        Task<ReportbyStatusDto> GetReportbyStatusAsync();
    }
}
