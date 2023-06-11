namespace GmailServer.Models
{
    public class AppleOrderStatisticByAddPaymentStatusData
    {
        public int None { get; set; }

        public int InUse { get; set; }

        public int Expired { get; set; }

        public int Error { get; set; }

        public int Completed { get; set; }
    }
}
