using GmailServer.FakeSettings;
using GmailServer.Permissions;
using GmailServer.Web.Pages.FakeSettings.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.FakeSettings
{
    [Authorize(GmailServerPermissions.FakeSettings.Update)]
    public class EditModalModel : GmailServerPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty]
        public FakeSettingViewModel FakeSetting { get; set; }

        private readonly IFakeSettingAppService fakeSettingAppService;

        public EditModalModel(IFakeSettingAppService fakeSettingAppService)
        {
            this.fakeSettingAppService = fakeSettingAppService;
        }

        public async void OnGet()
        {
            var fakeSettingDto = await this.fakeSettingAppService.GetAsync(Id);
            FakeSetting = ObjectMapper.Map<FakeSettingDto, FakeSettingViewModel>(fakeSettingDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var fakeSetting = ObjectMapper.Map<FakeSettingViewModel, CreateUpdateFakeSettingDto>(FakeSetting);
            await this.fakeSettingAppService.UpdateAsync(Id, fakeSetting);
            return NoContent();
        }
    }
}
