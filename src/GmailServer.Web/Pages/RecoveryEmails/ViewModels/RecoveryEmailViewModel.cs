using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace GmailServer.Web.Pages.RecoveryEmails.ViewModels
{
    public class RecoveryEmailViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [TextArea(Rows = 35)]
        public string Emails { get; set; }
    }
}
