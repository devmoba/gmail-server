using Volo.Abp.Application.Dtos;

namespace GmailServer.GmailResources.Statistics
{
    public class GmailResourceStatisticFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }
    }
}
