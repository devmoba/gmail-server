using GmailServer.Enums;
using Volo.Abp.Application.Dtos;

namespace GmailServer.GmailResources
{
    public class GmailResourceFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }

        public GmailResourceStatus? Status { get; set; }
    }
}
