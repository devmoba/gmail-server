using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.GmailPremiums
{
    public class GmailPremiumDto : EntityDto<long>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string RecoveryEmail { get; set; }

        public DateTime Created { get; set; }

        public GmailPremiumStatus Status { get; set; }
    }
}
