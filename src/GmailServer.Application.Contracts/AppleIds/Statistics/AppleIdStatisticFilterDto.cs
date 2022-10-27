using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIds.Statistics
{
    public class AppleIdStatisticFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }
    }
}
