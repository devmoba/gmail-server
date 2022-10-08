using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.GmailTypes
{
    public interface IGmailTypeAppService : ICrudAppService<
        GmailTypeDto, 
        long, 
        GmailTypeFilterDto, 
        CreateUpdateGmailTypeDto, 
        CreateUpdateGmailTypeDto>
    {
        Task<List<GmailTypeSelectionDto>> GetAllSelectionAsync();
    }
}
