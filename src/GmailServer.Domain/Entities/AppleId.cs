using GmailServer.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class AppleId : Entity<long>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime TakenTime { get; set; }

        public AppleIdStatus Status { get; set; }

        public virtual ICollection<DownloadedApp> DownloadedApps { get; set; }
    }
}
