using GmailServer.Enums;
using System;
using System.Collections.Generic;

namespace GmailServer.Gmails
{
    public class GmailDownloadFilter
    {
        public List<Status> Statuses { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
