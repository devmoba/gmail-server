using GmailServer.Enums;
using GmailServer.GmailResources;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.ResetStatus)]
    public class ResetStatusModel : GmailServerPageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public List<GmailResourceStatus> Statuses { get; set; }

        [BindProperty]
        public int? UpdatedHours { get; set; }

        [BindProperty]
        [Required]
        public GmailResourceStatus TargetStatus { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTo { get; set; }

        [BindProperty]

        public bool CheckedOnDelete { get; set; }

        private readonly IGmailResourceAppService gmailResourceAppService;

        public ResetStatusModel(IGmailResourceAppService gmailResourceAppService)
        {
            this.gmailResourceAppService = gmailResourceAppService;
        }
        public async Task OnGetAsync()
        {
            var usernames = await this.gmailResourceAppService.GetUsernameSelectionAsync();
            var usernameSelections = usernames.Select(item => new SelectListItem()
            {
                Text = item,
                Value = item
            }).ToList();

            usernameSelections.AddFirst(new SelectListItem()
            {
                Text = "All Username",
                Value = string.Empty
            });

            var gmailResourceStatusSelections = Enum.GetValues(typeof(GmailResourceStatus)).Cast<GmailResourceStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();

            ViewData.Add("gmailResourceStatusSelections", SerializeObject(gmailResourceStatusSelections));
            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Statuses.Count > 0)
            {
                if (CheckedOnDelete)
                {
                    await this.gmailResourceAppService.DeleteAsync(new DeleteFilter()
                    {
                        Username = Username,
                        Statuses = Statuses,
                        CreatedFrom = CreatedFrom,
                        CreatedTo = CreatedTo,
                        UpdatedHours = UpdatedHours
                    });
                }
                else
                {
                    await this.gmailResourceAppService.ResetStatusAsync(new ResetStatusFilter()
                    {
                        Username = Username,
                        Statuses = Statuses,
                        TargetStatus = TargetStatus,
                        CreatedFrom = CreatedFrom,
                        CreatedTo = CreatedTo,
                        UpdatedHours = UpdatedHours
                    });
                }
            }
            else
            {
                throw new UserFriendlyException("The status is required");
            }

            return RedirectToPage("/GmailResources/ResetStatus");
        }
    }
}
