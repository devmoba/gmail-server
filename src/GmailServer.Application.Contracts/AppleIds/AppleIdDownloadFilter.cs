using GmailServer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GmailServer.AppleIds
{
    public class AppleIdDownloadFilter
    {
        public string Username { get; set; }

        public List<AppleIdStatus> Statuses { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
