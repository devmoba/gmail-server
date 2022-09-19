using GmailServer.Enums;
using Volo.Abp.Application.Dtos;

namespace GmailServer.GmailPremiums
{
    public class GmailPremiumFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }

        public GmailPremiumStatus? Status { get; set; }
    }
}
