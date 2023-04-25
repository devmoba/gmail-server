using GmailServer.Enums;
using System;
using System.Collections.Generic;

namespace GmailServer.AppleOrders
{
    public class ResetLinkStatusFilterInput
    {
        public string Username { get; set; }

        public List<LinkStatus> Statuses { get; set; }

        public LinkStatus TargetStatus { get; set; }

        public DateTime? CreatedTimeFrom { get; set; }

        public DateTime? CreatedTimeTo { get; set; }
    }
}
