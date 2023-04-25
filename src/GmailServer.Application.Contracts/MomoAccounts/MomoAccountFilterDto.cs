using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.MomoAccounts
{
    public class MomoAccountFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public MomoAccountStatus? Status { get; set; }

        public int? TotalLinkCountMax { get; set; }

        public int? TotalLinkCountMin { get; set; }

        public DateTime? CreatedTimeFrom { get; set; }

        public DateTime? CreatedTimeTo { get; set; }
    }
}
