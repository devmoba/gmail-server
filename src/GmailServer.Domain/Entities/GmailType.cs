using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class GmailType : Entity<long>
    {
        public string Name { get; set; }

        public string DeviceType { get; set; }

        public string FakeVersion { get; set; }

        public string Version { get; set; }

        public string Country { get; set; }

        public virtual ICollection<Gmail> Gmails { get; set; }
    }
}
