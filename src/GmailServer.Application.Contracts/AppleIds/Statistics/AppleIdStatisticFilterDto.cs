using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIds.Statistics
{
    public class AppleIdStatisticFilterDto : PagedAndSortedResultRequestDto
    {
        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public string Username { get; set; }
    }
}
