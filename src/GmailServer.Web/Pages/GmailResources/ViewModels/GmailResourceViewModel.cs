using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace GmailServer.Web.Pages.GmailResources.ViewModels
{
    public class GmailResourceViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [TextArea(Rows = 35)]
        [Placeholder("email|password|recoveryEmail(optional)|country(optional)")]
        public string Emails { get; set; }
    }
}
