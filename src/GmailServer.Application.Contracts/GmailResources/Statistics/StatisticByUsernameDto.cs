using System.Collections.Generic;

namespace GmailServer.GmailResources.Statistics
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
