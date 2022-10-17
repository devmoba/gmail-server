using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIds.Statistics
{
    public class AppleStatisticFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }
    }
}
