using GmailServer.EmailChecks;
using GmailServer.Enums;
using System.Collections.Generic;

namespace GmailServer.CheckMails
{
    public class EmailResultGroup
    {
        public string Status { get; set; }

        public List<string> EmailResults { get; set; }

        public int Count { get; set; }  
    }
}
