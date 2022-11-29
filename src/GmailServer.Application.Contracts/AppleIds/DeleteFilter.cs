using GmailServer.Enums;
using System;
using System.Collections.Generic;

namespace GmailServer.AppleIds
{
    public class DeleteFilter
    {
        public string Username { get; set; }

        public List<AppleIdStatus> Statuses { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public int? UpdatedHours { get; set; } = null;
    }
}
