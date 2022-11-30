using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.DownloadedApps
{
    public class DownloadAppFilterDto : PagedAndSortedResultRequestDto
    {
        public string AppId { get; set; }

        public string ProductId { get; set; }

        public string Email { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public long? AppleIdFK { get; set; }
    }
}
