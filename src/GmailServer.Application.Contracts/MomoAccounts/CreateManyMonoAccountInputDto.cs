using System.ComponentModel.DataAnnotations;

namespace GmailServer.MomoAccounts
{
    public class CreateManyMonoAccountInputDto
    {
        [Required]
        public string Accounts { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
