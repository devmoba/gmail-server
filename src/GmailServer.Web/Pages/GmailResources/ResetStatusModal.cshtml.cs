using GmailServer.Enums;
using GmailServer.GmailResources;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.ResetStatus)]
    public class ResetStatusModalModel : GmailServerPageModel
    {
        [BindProperty]
        public List<GmailResourceStatus> Statuses { get; set; }

        [BindProperty]
        public int? UpdatedHours { get; set; }

        [BindProperty]
        [Required]
        public GmailResourceStatus TargetStatus { get; set; }

        private readonly IGmailResourceAppService gmailResourceAppService;

        public ResetStatusModalModel(IGmailResourceAppService gmailResourceAppService)
        {
            this.gmailResourceAppService = gmailResourceAppService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Statuses.Count > 0)
            {
                await this.gmailResourceAppService.ResetStatusAsync(Statuses, UpdatedHours, TargetStatus);
            }
            else
            {
                throw new UserFriendlyException("The status is required");
            }
            return NoContent();
        }
    }
}
