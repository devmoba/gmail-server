using GmailServer.TaskChecks;
using System;
using System.Collections.Generic;
using System.Text;

namespace GmailServer.CheckerReports
{
    public class ReportResponseDto
    {
        public List<TaskCheckDto> TaskChecks { get; set; }
    }
}
