using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIdNones
{
    public class AppleIdNoneFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public int? PurchaseNumberMax { get; set; }

        public int? PurchaseNumberMin { get; set; }

        public int? TakenOutNumberMax { get; set; }

        public int? TakenOutNumberMin { get; set; }

        public AppleIdNoneStatus? Status { get; set; }

        public RemovePaymentStatus? RemovePaymentStatus { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
