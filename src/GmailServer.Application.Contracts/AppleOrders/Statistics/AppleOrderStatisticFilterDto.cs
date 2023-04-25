using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleOrders.Statistics
{
    public class AppleOrderStatisticFilterDto : PagedAndSortedResultRequestDto
    {
        public DateTime? CreatedTimeFrom { get; set; }

        public DateTime? CreatedTimeTo { get; set; }
    }
}
