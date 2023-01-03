using GmailServer.AppleIds;
using GmailServer.Enums;
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

namespace GmailServer.Web.Pages.AppleIds
{
    [Authorize(GmailServerPermissions.AppleIds.ResetStatus)]
    public class ResetStatusModel : GmailServerPageModel
    {

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public List<AppleIdStatus> Statuses { get; set; }

        [BindProperty]
        public int? UpdatedHours { get; set; }

        [BindProperty]
        [Required]
        public AppleIdStatus TargetStatus { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTo { get; set; }

        [BindProperty]

        public bool CheckedOnDelete { get; set; }

        private readonly IAppleIdAppService appleIdAppService;

        public ResetStatusModel(IAppleIdAppService appleIdAppService)
        {
            this.appleIdAppService = appleIdAppService;
        }
        public async Task OnGetAsync()
        {
            var usernameSelections = await this.appleIdAppService.GetUsernameSelectionAsync();
            //var usernameSelections = usernames.Select(item => new SelectListItem()
            //{
            //    Text = item,
            //    Value = item
            //}).ToList();

            usernameSelections.AddFirst(new UsernameSelectionDto()
            {
                Text = "All Username",
                Value = string.Empty
            });

            var appleIdStatusSelections = Enum.GetValues(typeof(AppleIdStatus)).Cast<AppleIdStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();

            ViewData.Add("appleIdStatusSelections", SerializeObject(appleIdStatusSelections));
            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Statuses.Count > 0)
            {
                if (CheckedOnDelete)
                {
                    await this.appleIdAppService.DeleteAsync(new DeleteFilter()
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
                    await this.appleIdAppService.ResetStatusAsync(new ResetStatusFilter()
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
            return RedirectToPage("/AppleIds/ResetStatus");
        }
    }
}
