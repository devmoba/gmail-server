using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIdNones.Statistics
{
    public class AppleIdNoneStatisticFilterDto : PagedAndSortedResultRequestDto
    {
        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public string Username { get; set; }
    }
}
