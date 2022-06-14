using System.ComponentModel.DataAnnotations;

namespace GmailServer.FakeSettings
{
    public class CreateUpdateFakeSettingDto
    {
        [Required]
        public string DeviceType { get; set; }

        [Required]
        public string Version { get; set; }

        [Required]
        public string FakeVersion { get; set; }
    }
}
