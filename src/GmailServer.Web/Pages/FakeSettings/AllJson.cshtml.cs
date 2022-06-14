using GmailServer.Entities;
using GmailServer.FakeSettings;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.FakeSettings
{
    public class AllJsonModel : GmailServerPageModel
    {
        private readonly IFakeSettingRepository _repository;

        public AllJsonModel(IFakeSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<JsonResult> OnGet()
        {
            var fakeSettings = await _repository.ToListAsync();
            var res = ObjectMapper.Map<List<FakeSetting>, List<FakeSettingDto>>(fakeSettings);

            return new JsonResult(res);
        }
    }
}
