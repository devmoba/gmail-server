using GmailServer.Enums;
using System;

namespace GmailServer.TaskChecks
{
    public class CreateUpdateTaskCheckDto
    {
        public string Username { get; set; }

        public string EmailChecks { get; set; }

        public TaskCheckStatus Status { get; set; }

        public TypeCheck TypeCheck { get; set; }

        public DateTime Created { get; set; }

        public long CheckerId { get; set; }
    }
}
