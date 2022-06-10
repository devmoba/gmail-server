using GmailServer.Enums;
using System;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class Gmail : Entity<long>
    {
        public DateTime Date { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RecoveryEmail { get; set; }

        public string DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Timezone { get; set; }

        public string FakeVersion { get; set; }

        public string SerialNumber { get; set; }

        public string DeviceType { get; set; }

        public string Version { get; set; }

        public string Country { get; set; }

        public Status Status { get; set; }

        public string Arg1 { get; set; }

        public string Arg2 { get; set; }

        public string Arg3 { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
