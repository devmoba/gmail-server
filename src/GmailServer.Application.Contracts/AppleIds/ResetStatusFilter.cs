using GmailServer.Enums;
using System;
using System.Collections.Generic;

namespace GmailServer.AppleIds
{
    public class ResetStatusFilter
    {
        public string Username { get; set; }

        public List<AppleIdStatus> Statuses { get; set; }

        public AppleIdStatus TargetStatus { get; set; } = AppleIdStatus.Ready;

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public int? UpdatedHours { get; set; } = null;
    }
}
