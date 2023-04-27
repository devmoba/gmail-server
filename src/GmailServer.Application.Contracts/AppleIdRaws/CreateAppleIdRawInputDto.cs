using System;
using System.Collections.Generic;
using System.Text;

namespace GmailServer.AppleIdRaws
{
    public class CreateAppleIdRawInputDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string SecretAnswer1 { get; set; }

        public string SecretAnswer2 { get; set; }

        public string SecretAnswer3 { get; set; }

        public string DateOfBirth { get; set; }

        public string Country { get; set; }
    }
}
