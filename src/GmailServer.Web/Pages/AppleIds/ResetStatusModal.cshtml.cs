using GmailServer.AppleIds;
using GmailServer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace GmailServer.Web.Pages.AppleIds
{
    public class ResetStatusModalModel : GmailServerPageModel
    {
        [BindProperty]
        public List<AppleIdStatus> Statuses { get; set; }

        [BindProperty]
        public int? UpdatedHours { get; set; }

        private readonly IAppleIdAppService appleIdAppService;

        public ResetStatusModalModel(IAppleIdAppService appleIdAppService)
        {
            this.appleIdAppService = appleIdAppService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Statuses.Count > 0)
            {
                await this.appleIdAppService.ResetStatusAsync(Statuses, UpdatedHours);
            }
            else
            {
                throw new UserFriendlyException("The status is required");
            }
            return NoContent();
        }
    }
}
