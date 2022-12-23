using System;
using System.Collections.Generic;
using System.Text;

namespace GmailServer.AppleIds.Statistics
{
    public class AppleIdStatisticDailyDto
    {
        public DateTime Created { get; set; }

        public int Total { get; set; }

        public int Ready { get; set; }

        public int Completed1 { get; set; }

        public int Completed2 { get; set; }

        public int Completed3 { get; set; }

        public int Completed4 { get; set; }

        public int Pending { get; set; }

        public int WrongPass { get; set; }

        public int Subed { get; set; }

        public int Locked1 { get; set; }

        public int Locked2 { get; set; }

        public int Review { get; set; }

        public int Error { get; set; }

        public int Unknown { get; set; }
    }
}
