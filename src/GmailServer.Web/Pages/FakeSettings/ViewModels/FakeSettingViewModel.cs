using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace GmailServer.Web.Pages.FakeSettings.ViewModels
{
    public class FakeSettingViewModel
    {
        [Required]
        [TextArea(Rows = 3)]
        public string DeviceType { get; set; }

        [Required]
        [TextArea(Rows = 7)]
        public string Version { get; set; }

        [Required]
        [TextArea(Rows = 15)]
        public string FakeVersion { get; set; }
    }
}
