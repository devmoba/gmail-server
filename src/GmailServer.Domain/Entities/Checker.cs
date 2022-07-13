using GmailServer.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class Checker : Entity<long>
    {
        public Guid CheckerId { get; set; }

        public string CheckerIP { get; set; }

        public CheckerStatus Status { get; set; }

        public double FreeRam { get; set; }

        public double TotalRam { get; set; }

        public int UsingThread { get; set; }

        public int MaxThread { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastCheck { get; set; }

        public virtual ICollection<TaskCheck> TaskChecks { get; set; }
    }
}
