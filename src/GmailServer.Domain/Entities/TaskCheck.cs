using GmailServer.Enums;
using System;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class TaskCheck : Entity<long>
    {
        public string Username { get; set; }

        public string EmailChecks { get; set; }

        public TaskCheckStatus Status { get; set; }

        public TypeCheck TypeCheck { get; set; }

        public DateTime Created { get; set; }

        public long CheckerId { get; set; }

        public virtual Checker Checker { get; set; }
    }
}
