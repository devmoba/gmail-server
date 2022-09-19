using System.ComponentModel.DataAnnotations;

namespace GmailServer.AppleIds
{
    public class CreateManyAppleIdInputDto
    {
        [Required]
        public string Emails { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
