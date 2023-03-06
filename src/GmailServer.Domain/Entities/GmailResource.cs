using GmailServer.Enums;
using System;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class GmailResource : Entity<long>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string RecoveryEmail { get; set; }

        public string Country { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime TakenTime { get; set; }

        public GmailResourceStatus Status { get; set; }

        public PremiumType PremiumType { get; set; }

        public DateTime UpdatedPremium { get; set; }
    }
}
