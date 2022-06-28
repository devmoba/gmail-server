using GmailServer.EmailChecks;
using GmailServer.Enums;
using System.Collections.Generic;

namespace GmailServer.CheckMails
{
    public class EmailResultGroup
    {
        public Status Status { get; set; }

        public string EmailResultOuput { get; set; }

        public int Count { get; set; }  
    }
}
