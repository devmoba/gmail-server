using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIdRaws
{
    public class AppleIdRawStatisticFilterDto : PagedAndSortedResultRequestDto
    {
        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
