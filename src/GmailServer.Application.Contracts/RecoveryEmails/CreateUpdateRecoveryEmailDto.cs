using System.ComponentModel.DataAnnotations;

namespace GmailServer.RecoveryEmails
{
    public class CreateUpdateRecoveryEmailDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }    
    }
}
