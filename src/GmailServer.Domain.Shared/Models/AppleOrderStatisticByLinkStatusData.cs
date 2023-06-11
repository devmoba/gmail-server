namespace GmailServer.Models
{
    public class AppleOrderStatisticByLinkStatusData
    {
        public int Ready { get; set; }

        public int InUse { get; set; }

        public int Expired { get; set; }

        public int Error { get; set; }

        public int Linked { get; set; }
    }
}
