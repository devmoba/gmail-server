using GmailServer.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GmailServer.Gmails
{
    public class CreateGmailDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
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

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public string Timezone { get; set; }

        public string FakeVersion { get; set; }

        public string SerialNumber { get; set; }

        public string DeviceType { get; set; }

        public string Version { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public Status Status { get; set; }

        public string Arg1 { get; set; }

        public string Arg2 { get; set; }

        public string Arg3 { get; set; }
    }
}
