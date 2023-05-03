using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.MomoAccounts
{
    public class MomoAccountFilterDto : PagedAndSortedResultRequestDto
    {
        public string UploadGroup { get; set; }

        public string Username { get; set; }

        public MomoAccountStatus? Status { get; set; }

        public int? TotalLinkCountMax { get; set; }

        public int? TotalLinkCountMin { get; set; }

        public DateTime? CreatedTimeFrom { get; set; }

        public DateTime? CreatedTimeTo { get; set; }
    }
}
