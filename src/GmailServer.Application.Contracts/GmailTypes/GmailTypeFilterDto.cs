using Volo.Abp.Application.Dtos;

namespace GmailServer.GmailTypes
{
    public class GmailTypeFilterDto : PagedAndSortedResultRequestDto
    {
        public string Name { get; set; }

        public string DeviceType { get; set; }

        public string FakeVersion { get; set; }

        public string Version { get; set; }

        public string Country { get; set; }
    }
}
