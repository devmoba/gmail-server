using System;

namespace GmailServer.GmailResources
{
    public class GmailResourceExcelModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string RecoveryEmail { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime TakenTime { get; set; }
    }
}
