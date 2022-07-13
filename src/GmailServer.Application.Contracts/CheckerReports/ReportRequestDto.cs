using System;
using System.Collections.Generic;

namespace GmailServer.CheckerReports
{
    public class ReportRequestDto
    {
        public Guid CheckerId { get; set; }

        public string CheckerIP { get; set; }

        public double FreeRam { get; set; }

        public double TotalRam { get; set; }

        public int UsingThread { get; set; }

        public int MaxThread { get; set; }

        public List<TaskCheckResultDto> TaskCheckResults { get; set; }
    }
}
