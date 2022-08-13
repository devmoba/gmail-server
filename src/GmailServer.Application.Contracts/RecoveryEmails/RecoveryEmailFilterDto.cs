using GmailServer.Enums;
using Volo.Abp.Application.Dtos;

namespace GmailServer.RecoveryEmails
{
    public class RecoveryEmailFilterDto : PagedAndSortedResultRequestDto
    {
        public string Username { get; set; }

        public RecoveryEmailStatus? Status { get; set; }
    }
}
