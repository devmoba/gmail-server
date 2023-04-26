using GmailServer.Enums;
using System;
using System.Collections.Generic;

namespace GmailServer.MomoAccounts
{
    public class DeleteFilterInput
    {
        public string UploadGroup { get; set; }

        public List<MomoAccountStatus> Statuses { get; set; }

        public MomoAccountStatus TargetStatus { get; set; }

        public DateTime? CreatedTimeFrom { get; set; }

        public DateTime? CreatedTimeTo { get; set; }
    }
}
