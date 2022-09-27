using GmailServer.Enums;
using System;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class GmailPremium : Entity<long>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string RecoveryEmail { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime TakenTime { get; set; }

        public GmailPremiumStatus Status { get; set; }
    }
}
