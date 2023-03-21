using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.GmailResources.Statistics
{
    public class GmailResourceStatisticFilterDto : PagedAndSortedResultRequestDto
    {
        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public string Username { get; set; }
    }
}
