using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GmailServer.GmailResources
{
    public class ResetStatusFilter
    {
        public string Username { get; set; }

        public List<GmailResourceStatus> Statuses { get; set; }

        public GmailResourceStatus TargetStatus { get; set; } = GmailResourceStatus.Ready;

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public int? UpdatedHours { get; set; } = null;
    }
}
