using System.ComponentModel.DataAnnotations;

namespace GmailServer.GmailPremiums
{
    public class CreateManyGmailPremiumInputDto
    {
        [Required]
        public string Emails { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
