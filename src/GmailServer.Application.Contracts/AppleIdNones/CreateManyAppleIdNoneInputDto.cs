using System.ComponentModel.DataAnnotations;

namespace GmailServer.AppleIdNones
{
    public class CreateManyAppleIdNoneInputDto
    {
        [Required]
        public string Emails { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
