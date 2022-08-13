using System.ComponentModel.DataAnnotations;

namespace GmailServer.RecoveryEmails
{
    public class CreateUpdateRecoveryEmailDto
    {
        [Required]
        public string Emails { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
