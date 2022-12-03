using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIds
{
    public class AppleIdGetOutputDto : EntityDto<long>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime TakenTime { get; set; }

        public AppleIdStatus Status { get; set; }

        public int? DownloadedAppCount { get; set; }
    }
}
