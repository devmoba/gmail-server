using Volo.Abp.Application.Dtos;

namespace GmailServer.GmailTypes
{
    public class GmailTypeSelectionDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}
