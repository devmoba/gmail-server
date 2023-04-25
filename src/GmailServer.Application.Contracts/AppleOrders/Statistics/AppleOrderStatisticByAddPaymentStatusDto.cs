using System;

namespace GmailServer.AppleOrders.Statistics
{
    public class AppleOrderStatisticByAddPaymentStatusDto
    {
        public DateTime CreatedTime { get; set; }

        public int Total { get; set; }

        public int None { get; set; }

        public int InUse { get; set; }

        public int Expired { get; set; }

        public int Error { get; set; }

        public int Completed { get; set; }
    }
}
