using GmailServer.Entities;
using GmailServer.FakeSettings;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GmailServer.Web.Pages.FakeSettings
{
    [Authorize(GmailServerPermissions.FakeSettings.Default)]
    public class JsonViewModel : GmailServerPageModel
    {
        private readonly IFakeSettingRepository _repository;

        public JsonViewModel(IFakeSettingRepository repository)
        {
            _repository = repository;
        }
        public async void OnGet()
        {
            var fakeSettings = await _repository.ToListAsync();
            var res = ObjectMapper.Map<List<FakeSetting>, List<FakeSettingDto>>(fakeSettings);
            ViewData.Add("fakeSettings", SerializeObject(res));
        }
    }
}
