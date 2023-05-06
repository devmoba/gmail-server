using Volo.Abp.Application.Dtos;

namespace GmailServer.OwnerConfigs
{
    public class OwnerConfigFilterDto : PagedAndSortedResultRequestDto
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
