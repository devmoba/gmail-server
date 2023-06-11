namespace GmailServer.Models
{
    public class GmailResourceStatisticData
    {
        public int Ready { get; set; }

        public int Success { get; set; }

        public int Pending { get; set; }

        public int Used { get; set; }

        public int Failed { get; set; }

        public int Error { get; set; }

        public int Unknown { get; set; }
    }
}
