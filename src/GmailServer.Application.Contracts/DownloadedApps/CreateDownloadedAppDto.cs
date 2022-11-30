using System.ComponentModel.DataAnnotations;

namespace GmailServer.DownloadedApps
{
    public class CreateDownloadedAppDto
    {
        [Required]
        public string AppId { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public string Email { get; set; } // AppleId
    }
}
