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
        private readonly IHubContext<CheckMailHub, ICheckMailHub> hubContext;
        private readonly IConfiguration configuration;

        [BindProperty]
        public string EmailCheckInput { get; set; }

        public string EmailResultOutput { get; set; }

        public IndexModel(IHubContext<CheckMailHub, ICheckMailHub> hubContext,
            IConfiguration configuration)
        {
            this.hubContext = hubContext;
            this.configuration = configuration;
        }

        public void OnGet()
        {
            var emailLimitRequest = this.configuration.GetValue<int>("CheckMail:MailPerRequest");
            ViewData.Add("emailLimitRequest", SerializeObject(emailLimitRequest));
        }

        public async Task<IActionResult> OnPost()
        {
          
            return NoContent();
        }
    }
}
