using GmailServer.Enums;

namespace GmailServer.CheckerReports
{
    public class EmailResultDto
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public Status Status { get; set; }
    }
}
