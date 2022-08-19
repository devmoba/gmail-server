using System.ComponentModel.DataAnnotations;

namespace GmailServer.RecoveryEmails
{
    public class CreateManyRecoveryEmailInputDto
    {
        [Required]
        public string Emails { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
