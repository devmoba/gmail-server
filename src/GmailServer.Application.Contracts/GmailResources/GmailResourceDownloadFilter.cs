using GmailServer.Enums;
using System;
using System.Collections.Generic;

namespace GmailServer.GmailResources
{
    public class GmailResourceDownloadFilter
    {
        public string Username { get; set; }

        public List<GmailResourceStatus> Statuses { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
