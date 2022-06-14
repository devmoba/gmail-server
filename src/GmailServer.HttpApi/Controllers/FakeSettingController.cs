using GmailServer.ControllerInterfaces;
using GmailServer.FakeSettings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    [RemoteService(Name = GmailServerHttpApiModule.RemoteServiceName)]
    [Route("/api/fakeSettings")]
    public class FakeSettingController : AbpController, IFakeSettingController
    {
        private readonly IFakeSettingAppService fakeSettingAppService;
        //private readonly JsonSerializerSettings CamelCaseSerializerSettings = new JsonSerializerSettings()
        //{
        //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
        //    NullValueHandling = NullValueHandling.Include,
        //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //    DateFormatHandling = DateFormatHandling.IsoDateFormat
        //};


        public FakeSettingController(IFakeSettingAppService fakeSettingAppService)
        {
            this.fakeSettingAppService = fakeSettingAppService;
        }

        [HttpGet]
        public async Task<PagedResultDto<FakeSettingDto>> GetListAsync(FakeSettingFilterDto input)
        {
            return await this.fakeSettingAppService.GetListAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(long id)
        {
            await this.fakeSettingAppService.DeleteAsync(id);
        }

        //[HttpGet]
        //[Route("all")]
        //public async Task<IActionResult> GetAll()
        //{
        //   var fakeSettings = await this.fakeSettingAppService.GetAll();
        //   var jsonString = JsonConvert.SerializeObject(fakeSettings, CamelCaseSerializerSettings);
        //   byte[] byteArray = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonString);

        //   return File(byteArray, "application/force-download", "fake-setting.json");
        //}
    }
}
