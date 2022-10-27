using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.GmailResources.Statistics
{
    public class GmailResourceStatisticDailyFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }

}
