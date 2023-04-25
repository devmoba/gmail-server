using GmailServer.Enums;
using System;
using System.Collections.Generic;

namespace GmailServer.AppleIdNones
{
    public class DeleteFilter
    {
        public string Username { get; set; }

        public List<AppleIdNoneStatus> Statuses { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
