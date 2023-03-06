using System.ComponentModel.DataAnnotations;

namespace GmailServer.GmailResources
{
    public class ReupGmailResourceInputDto
    {
        [Required]
        public string Emails { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
