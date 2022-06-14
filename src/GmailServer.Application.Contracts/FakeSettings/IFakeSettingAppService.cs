using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GmailServer.FakeSettings
{
    public interface IFakeSettingAppService : ICrudAppService<
        FakeSettingDto, 
        long, 
        FakeSettingFilterDto,
        CreateUpdateFakeSettingDto, 
        CreateUpdateFakeSettingDto>
    {
        Task<List<FakeSettingDto>> GetAll();
    }
}
