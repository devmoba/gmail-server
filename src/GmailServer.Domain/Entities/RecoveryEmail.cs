using GmailServer.Enums;
using System;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class RecoveryEmail : Entity<long>
    {
        public string Emails { get; set; }

        public string Username { get; set; }

        public DateTime Created { get; set; }

        public RecoveryEmailStatus Status { get; set; }
    }
}
