using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GmailServer.GmailTypes
{
    public class CreateUpdateGmailTypeDto
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [DisplayName("Device Type")]
        [MaxLength(128)]
        public string DeviceType { get; set; }

        [DisplayName("Fake Version")]
        [MaxLength(128)]
        public string FakeVersion { get; set; }

        [MaxLength(128)]
        public string Version { get; set; }

        [MaxLength(26)]
        public string Country { get; set; }
    }
}
