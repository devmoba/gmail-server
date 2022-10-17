using GmailServer.Enums;

namespace GmailServer.RecoveryEmails
{
    public class RecoveryEmailReportStatusDto
    {
        public int ReadyCount { get; set; }

        public int CompletedCount { get; set; }
    }
}
