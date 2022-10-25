using System.ComponentModel.DataAnnotations;

namespace GmailServer.AppleIds
{
    public class CreateUpdateAppleIdDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
