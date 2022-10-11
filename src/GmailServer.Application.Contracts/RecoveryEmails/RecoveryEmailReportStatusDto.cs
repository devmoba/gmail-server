using GmailServer.Enums;

namespace GmailServer.RecoveryEmails
{
    public class RecoveryEmailReportStatusDto
    {
        public RecoveryEmailStatus Status { get; set; }

        public int Count { get; set; }
    }
}
