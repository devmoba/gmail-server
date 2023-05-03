using GmailServer.Enums;

namespace GmailServer.AppleIdNones
{
    public class AppleIdNoneStatusSelectionDto
    {
        public AppleIdNoneStatus Value { get; set; }

        public string Text { get; set; }
    }

    public class AppleIdNoneRemoveStatusSelectionDto
    {
        public RemovePaymentStatus Value { get; set; }

        public string Text { get; set; }
    }
}
