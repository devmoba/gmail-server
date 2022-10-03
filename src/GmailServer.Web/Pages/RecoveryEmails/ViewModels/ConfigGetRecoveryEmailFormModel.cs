using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace GmailServer.Web.Pages.RecoveryEmails.ViewModels
{
    public class ConfigGetRecoveryEmailFormModel
    {
        [Required]
        public int ReserveQuantity { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string ApiUrl { get; set; }

        [Required]
        public string ApiKey { get; set; }

        [Placeholder("mailcode1|mailcode2|mailcode3")]
        [Required]
        public string MailCodes { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
