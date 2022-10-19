using GmailServer.FakeSettings;
using GmailServer.Permissions;
using GmailServer.Web.Pages.FakeSettings.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.FakeSettings
{
    [Authorize(GmailServerPermissions.FakeSettings.Create)]
    public class CreateModalModel : GmailServerPageModel
    {
        [BindProperty]
        public FakeSettingViewModel FakeSetting { get; set; }

        private readonly IFakeSettingAppService fakeSettingAppService;

        public CreateModalModel(IFakeSettingAppService fakeSettingAppService)
        {
            this.fakeSettingAppService = fakeSettingAppService; 
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var fakeSetting = ObjectMapper.Map<FakeSettingViewModel, CreateUpdateFakeSettingDto>(FakeSetting);
            await this.fakeSettingAppService.CreateAsync(fakeSetting);
            return NoContent();
        }
    }
}
