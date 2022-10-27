using System;

namespace GmailServer.GmailResources.Statistics
{
    public class GmailResourceStatisticDailyDto
    {
        public DateTime Created { get; set; }

        public int Total { get; set; }

        public int Ready { get; set; }

        public int Success { get; set; }

        public int Pending { get; set; }

        public int Used { get; set; }

        public int Failed { get; set; }

        public int Error { get; set; }

        public int Unknown { get; set; }
    }
}
