using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace GmailServer.TaskChecks
{
    public class TaskCheckDto : EntityDto<long>
    {
        public string Username { get; set; }

        public string EmailChecks { get; set; }

        public TaskCheckStatus Status { get; set; }

        public TypeCheck TypeCheck { get; set; }

        public DateTime Created { get; set; }

        public long CheckerId { get; set; }
    }
}
