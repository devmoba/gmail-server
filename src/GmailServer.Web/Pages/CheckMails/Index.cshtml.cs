using GmailServer.Hubs;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.CheckMails
{
    [Authorize(GmailServerPermissions.CheckMails.Default)]
    public class IndexModel : GmailServerPageModel
    {
        private readonly IConfiguration configuration;

        public IndexModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet()
        {
            var emailLimitRequest = this.configuration.GetValue<int>("CheckMail:MailPerRequest");
            ViewData.Add("emailLimitRequest", SerializeObject(emailLimitRequest));
        }
    }
}
