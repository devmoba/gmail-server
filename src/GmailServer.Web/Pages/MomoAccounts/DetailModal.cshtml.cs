using GmailServer.MomoAccounts;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.MomoAccounts
{
    [Authorize(GmailServerPermissions.MomoAccounts.Default)]
    public class DetailModalModel : GmailServerPageModel
    {
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public MomoAccountDto MomoAccount { get; set; }

        private readonly IMomoAccountAppService _appService;

        public DetailModalModel(IMomoAccountAppService appService)
        {
            _appService = appService;
        }

        public async Task OnGetAsync()
        {
            MomoAccount = await _appService.GetAsync(Id);
        }
    }
}
