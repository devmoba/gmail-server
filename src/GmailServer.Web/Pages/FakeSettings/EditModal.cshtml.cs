using GmailServer.FakeSettings;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.FakeSettings
{
    public class EditModalModel : GmailServerPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty]
        public CreateUpdateFakeSettingDto FakeSetting { get; set; }

        private readonly IFakeSettingAppService fakeSettingAppService;

        public EditModalModel(IFakeSettingAppService fakeSettingAppService)
        {
            this.fakeSettingAppService = fakeSettingAppService;
        }

        public async void OnGet()
        {
            var fakeSettingDto = await this.fakeSettingAppService.GetAsync(Id);
            FakeSetting = ObjectMapper.Map<FakeSettingDto, CreateUpdateFakeSettingDto>(fakeSettingDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await this.fakeSettingAppService.UpdateAsync(Id, FakeSetting);
            return NoContent();
        }
    }
}
