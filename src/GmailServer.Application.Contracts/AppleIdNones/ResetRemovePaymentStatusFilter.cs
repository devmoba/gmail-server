using GmailServer.Enums;
using System;
using System.Collections.Generic;

namespace GmailServer.AppleIdNones
{
    public class ResetRemovePaymentStatusFilter
    {
        public string Username { get; set; }

        public List<RemovePaymentStatus> Statuses { get; set; }

        public RemovePaymentStatus TargetStatus { get; set; } = RemovePaymentStatus.Ready;

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public DateTime? RemoveTakenTimeFrom { get; set; }

        public DateTime? RemoveTakenTimeTo { get; set; }
    }
}
