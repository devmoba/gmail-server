using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class OwnerConfig : Entity<long>
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
