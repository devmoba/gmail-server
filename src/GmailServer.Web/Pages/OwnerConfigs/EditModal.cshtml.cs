using GmailServer.OwnerConfigs;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.OwnerConfigs
{
    [Authorize(GmailServerPermissions.OwnerConfigs.Update)]
    public class EditModalModel : GmailServerPageModel
    {
        [BindProperty(SupportsGet = true)]
        [HiddenInput]
        public long Id { get; set; }

        [BindProperty]
        public EditFormModel ConfigModel { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Key { get; set; }

        private readonly IOwnerConfigAppService _appService;

        public EditModalModel(IOwnerConfigAppService appService)
        {
            _appService = appService;
        }

        public async void OnGet()
        {
            var config = await _appService.GetAsync(Id);
            ConfigModel = new EditFormModel()
            {
                Value = config.Value,
            };
            Key = config.Key;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _appService.UpdateAsync(Id, new CreateUpdateOwnerConfigDto()
            {
                Key = Key,
                Value = ConfigModel.Value
            });
            return NoContent();
        }
    }

    public class EditFormModel
    {
        public string Value { get; set; }
    }
}
