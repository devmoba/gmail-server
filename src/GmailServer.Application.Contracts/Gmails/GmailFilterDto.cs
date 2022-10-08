using GmailServer.Enums;
using Volo.Abp.Application.Dtos;

namespace GmailServer.Gmails
{
    public class GmailFilterDto : PagedAndSortedResultRequestDto
    {
        public string Email { get; set; }

        public string Country { get; set; }

        public string RecoveryEmail { get; set; }   

        public Status? Status { get; set; }

        public long? GmailTypeId { get; set; }
    }
}
