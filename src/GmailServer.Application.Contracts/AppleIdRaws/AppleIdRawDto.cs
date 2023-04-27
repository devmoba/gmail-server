using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIdRaws
{
    public class AppleIdRawDto : EntityDto<long>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string SecretAnswer1 { get; set; }

        public string SecretAnswer2 { get; set; }

        public string SecretAnswer3 { get; set; }

        public string DateOfBirth { get; set; }

        public string Country { get; set; }

        public DateTime Created { get; set; }
    }
}
