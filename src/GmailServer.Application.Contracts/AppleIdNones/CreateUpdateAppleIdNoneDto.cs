using System.ComponentModel.DataAnnotations;

namespace GmailServer.AppleIdNones
{
    public class CreateUpdateAppleIdNoneDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }

        public string Ccv { get; set; }

        public string SecretAnswer1 { get; set; }

        public string SecretAnswer2 { get; set; }

        public string SecretAnswer3 { get; set; }

        public string DateOfBirth { get; set; }
    }
}
