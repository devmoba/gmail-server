using GmailServer.FakeSettings;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.FakeSettings
{
    public class CreateModalModel : GmailServerPageModel
    {
        [BindProperty]
        public CreateUpdateFakeSettingDto FakeSetting { get; set; }

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
            await this.fakeSettingAppService.CreateAsync(FakeSetting);
            return NoContent();
        }
    }
}
