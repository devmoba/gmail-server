using GmailServer.AppleIds;
using GmailServer.Enums;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;

namespace GmailServer.Web.Pages.AppleIds
{
    [Authorize(GmailServerPermissions.AppleIds.ResetStatus)]
    public class ResetStatusModalModel : GmailServerPageModel
    {
        [BindProperty]
        public List<AppleIdStatus> Statuses { get; set; }

        [BindProperty]
        public int? UpdatedHours { get; set; }

        [BindProperty]
        [Required]
        public AppleIdStatus TargetStatus { get; set; }

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
                await this.appleIdAppService.ResetStatusAsync(Statuses, UpdatedHours, TargetStatus);
            }
            else
            {
                throw new UserFriendlyException("The status is required");
            }
            return NoContent();
        }
    }
}
