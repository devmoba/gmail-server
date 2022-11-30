using System;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class DownloadedApp : Entity<long>
    {
        public string AppId { get; set; }

        public string ProductId { get; set; }

        public string Email { get; set; }

        public long? AppleIdFK { get; set; }

        public DateTime Created { get; set; }

        public virtual AppleId AppleId { get; set; }
    }
}
