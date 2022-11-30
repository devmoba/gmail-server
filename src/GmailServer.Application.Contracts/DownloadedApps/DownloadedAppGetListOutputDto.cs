using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace GmailServer.DownloadedApps
{
    public class DownloadedAppGetListOutputDto : EntityDto<long>
    {
        public string AppId { get; set; }

        public string ProductId { get; set; }

        public string Email { get; set; }

        public long? AppleIdFK { get; set; }

        public DateTime Created { get; set; }

        public string AppleId { get; set; }
    }
}
