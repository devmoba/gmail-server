using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.GmailResources
{
    public class GmailResourceFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public GmailResourceStatus? Status { get; set; }

        public PremiumType? PremiumType { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
