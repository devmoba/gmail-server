using GmailServer.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace GmailServer.Web.Pages
{
    public abstract class GmailServerPageModel : AbpPageModel
    {
        private static readonly JsonSerializerSettings CamelCaseSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };

        protected GmailServerPageModel()
        {
            LocalizationResourceType = typeof(GmailServerResource);
        }

        protected string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, CamelCaseSerializerSettings);
        }
    }
}