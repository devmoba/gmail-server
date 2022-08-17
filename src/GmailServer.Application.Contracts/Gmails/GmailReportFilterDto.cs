using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.Gmails
{
    public class GmailReportFilterDto : PagedAndSortedResultRequestDto
    {
        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
