using System;
using System.Collections.Generic;
using System.Text;

namespace GmailServer.AppleIds.Statistics
{
    public class StatisticByUsernameDto
    {
        public long Total { get; set; }

        public List<StatusPoint> StatusPoints { get; set; }
    }

    public class StatusPoint
    {
        public string Name { get; set; }

        public long Y { get; set; }

        public bool Exploded { get; set; }
    }
}
