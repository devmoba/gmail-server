using GmailServer.Enums;

namespace GmailServer.EmailChecks
{
    public class EmailResult
    {
        public long Id { get; set; }

        public string Email { get; set; }   

        public Status Status { get; set; }
    }
}
