using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class FakeSetting : Entity<long>
    {
        public string DeviceType { get; set; }

        public string Version { get; set; }

        public string FakeVersion { get; set; }
    }
}
