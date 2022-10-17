using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIds.Statistics
{
    public class AppleIdStatisticDailyFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
