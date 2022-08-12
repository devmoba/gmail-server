using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.Gmails
{
    public class GmailReportFilterDto : PagedAndSortedResultRequestDto
    {
        public DateTime? CreatedMin { get; set; }

        public DateTime? CreatedMax { get; set; }
    }
}
