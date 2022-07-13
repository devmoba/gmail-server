using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.Checkers
{
    public class CheckerFilterDto : PagedAndSortedResultRequestDto
    {
        public string CheckerId { get; set; }

        public string CheckerIP { get; set; }
    }
}
