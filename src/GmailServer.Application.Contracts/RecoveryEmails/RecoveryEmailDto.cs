using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.RecoveryEmails
{
    public class RecoveryEmailDto : EntityDto<long>
    {
        public string Emails { get; set; }

        public string Username { get; set; }

        public DateTime Created { get; set; }

        public RecoveryEmailStatus Status { get; set; }
    }
}
