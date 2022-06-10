using GmailServer.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GmailServer.Gmails
{
    public class CreateGmailDto
    {
        public DateTime Date { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [MaxLength(128)]
        public string Email { get; set; }

        [Required]
        [MaxLength(64)]
        public string Password { get; set; }

        [Required]
        [MaxLength(128)]
        public string RecoveryEmail { get; set; }

        public string DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Timezone { get; set; }

        public string FakeVersion { get; set; }

        public string SerialNumber { get; set; }

        public string DeviceType { get; set; }

        public string Version { get; set; }

        public string Country { get; set; }

        public string Arg1 { get; set; }

        public string Arg2 { get; set; }

        public string Arg3 { get; set; }
    }
}
