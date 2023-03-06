using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GmailServer.GmailResources
{
    public class CreateUpdateGmailResourceDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string RecoveryEmail { get; set; }

        public string Country { get; set; }
    }
}
