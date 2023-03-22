namespace GmailServer.GmailResources
{
    public class ReupOutputDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string RecoveryEmail { get; set; }

        public string Country { get; set; }

        public string OutputType { get; set; } //Duplicate //NotInDB

        public string ReupStatus { get; set; } // Done // NA
    }
}
