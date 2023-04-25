using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.MomoAccounts.Statistics
{
    public class MomoAccountStatisticFilterDto : PagedAndSortedResultRequestDto
    {
        public DateTime? CreatedTimeForm { get; set; }

        public DateTime? CreatedTimeTo { get; set; }
    }
}
