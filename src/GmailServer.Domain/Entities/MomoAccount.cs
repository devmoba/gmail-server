using GmailServer.Enums;
using System;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class MomoAccount : Entity<long>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public MomoAccountStatus Status { get; set; }

        public string UDid1 { get; set; }

        public string UDid2 { get; set; }

        public string RefreshToken { get; set; }

        public string AuthenticateToken { get; set; }

        public string SessionKey { get; set; }

        public string SessionKey2 { get; set; }

        public string SetupKey { get; set; }

        public int CurrentLinkCount { get; set; }

        public int TotalLinkCount { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime LastTakenTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public string CustmArg1 { get; set; }

        public string CustmArg2 { get; set; }

        public string CustmArg3 { get; set; }

        public string InUseDevice { get; set; }

    }
}
