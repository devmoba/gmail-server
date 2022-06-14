using GmailServer.FakeSettings;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace GmailServer.ControllerInterfaces
{
    public interface IFakeSettingController
    {
        Task<PagedResultDto<FakeSettingDto>> GetListAsync(FakeSettingFilterDto input);

        Task DeleteAsync(long id);

        //Task<IActionResult> GetAll();
    }
}
