using System.ComponentModel.DataAnnotations;

namespace GmailServer.GmailPremiums
{
    public class CreateUpdateGmailPremiumDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string RecoveryEmail { get; set; }
    }
}
