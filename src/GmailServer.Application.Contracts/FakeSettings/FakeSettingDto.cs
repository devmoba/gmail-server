using Volo.Abp.Application.Dtos;

namespace GmailServer.FakeSettings
{
    public class FakeSettingDto : EntityDto<long>
    {
        public string DeviceType { get; set; }

        public string Version { get; set; }

        public string FakeVersion { get; set; }
    }
}
