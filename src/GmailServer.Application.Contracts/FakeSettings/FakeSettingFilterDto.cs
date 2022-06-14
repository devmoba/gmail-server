using Volo.Abp.Application.Dtos;

namespace GmailServer.FakeSettings
{
    public class FakeSettingFilterDto : PagedAndSortedResultRequestDto
    {
        public string DeviceType { get; set; }

        public string Version { get; set; }

        public string FakeVersion { get; set; }
    }
}
