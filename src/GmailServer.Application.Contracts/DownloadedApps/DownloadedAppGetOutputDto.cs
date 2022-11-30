using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.DownloadedApps
{
    public class DownloadedAppGetOutputDto : EntityDto<long>
    {
        public string AppId { get; set; }

        public string ProductId { get; set; }

        public string Email { get; set; }

        public long? AppleIdFK { get; set; }

        public DateTime Created { get; set; }
    }
}
