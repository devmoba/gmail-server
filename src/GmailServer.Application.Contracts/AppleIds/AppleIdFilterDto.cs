using GmailServer.Enums;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIds
{
    public class AppleIdFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }

        public AppleIdStatus? Status { get; set; }
    }
}
