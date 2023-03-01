using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace GmailServer.Web.Pages.AppleIds.ViewModels
{
    public class AppleIdViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [TextArea(Rows = 35)]
        [Placeholder("email|password|ccv(optional)")]
        public string Emails { get; set; }
    }
}
