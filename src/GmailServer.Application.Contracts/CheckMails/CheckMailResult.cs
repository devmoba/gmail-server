using System.Collections.Generic;

namespace GmailServer.CheckMails
{
    public class CheckMailResult
    {
        public List<string> EmailResults { get; set; }

        public List<EmailResultGroup> EmailResultGroups { get; set; }
    }
}
