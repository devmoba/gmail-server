using GmailServer.Enums;
using System.Collections.Generic;

namespace GmailServer.CheckerReports
{
    public class TaskCheckResultDto
    {
        public long Id { get; set; }    

        public string Username { get; set; }

        public List<EmailResultDto> EmailResults { get; set; }

        public TaskCheckStatus Status { get; set; }

        public TypeCheck TypeCheck { get; set; }

        public long CheckerId { get; set; }
    }
}
