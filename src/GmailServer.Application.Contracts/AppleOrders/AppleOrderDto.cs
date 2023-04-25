using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleOrders
{
    public class AppleOrderDto : EntityDto<long>
    {
        public string OrderID { get; set; }

        public string URLPayment { get; set; }

        public LinkStatus LinkStatus { get; set; }

        public AddPaymentStatus AddPaymentStatus { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime LinkTakenTime { get; set; }

        public DateTime LinkCompletedTime { get; set; }

        public DateTime AddPaymentTakenTime { get; set; }

        public DateTime AddPaymentCompletedTime { get; set; }

        public string MomoAccount { get; set; }

        public string AppleID { get; set; }
    }
}
