using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleOrders
{
    public class AppleOrderFilterDto : PagedAndSortedResultRequestDto
    {
        public string OrderID { get; set; }

        public string URLPayment { get; set; }

        public LinkStatus? LinkStatus { get; set; }

        public string MomoAccount { get; set; }

        public string AppleID { get; set; }

        public DateTime? CreatedTimeFrom { get; set; }

        public DateTime? CreatedTimeTo { get; set; }
    }
}
