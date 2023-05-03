using GmailServer.Enums;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using GmailServer.AppleIdNones;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;

namespace GmailServer.Web.Pages.AppleIdNones
{
    [Authorize(GmailServerPermissions.AppleIdNones.ResetStatus)]
    public class ResetStatusModel : GmailServerPageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public List<AppleIdNoneStatus> Statuses { get; set; }

        [BindProperty]
        [Required]
        public AppleIdNoneStatus TargetStatus { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTo { get; set; }

        [BindProperty]
        public bool CheckedOnDelete { get; set; }

        private readonly IAppleIdNoneAppService _appService;
        public ResetStatusModel(IAppleIdNoneAppService appService)
        {
            _appService = appService;
        }
        public async Task OnGetAsync()
        {
            var usernameSelections = await _appService.GetUsernameSelectionAsync();
            usernameSelections.AddFirst(new UsernameSelectionDto()
            {
                Text = "All Username",
                Value = string.Empty
            });
            var appleIdNoneStatusSelections = Enum.GetValues(typeof(AppleIdNoneStatus)).Cast<AppleIdNoneStatus>()
             .Select(item => new SelectListItem()
             {
                 Text = item.ToString(),
                 Value = $"{(int)item}"
             }).ToList();

            ViewData.Add("appleIdNoneStatusSelections", SerializeObject(appleIdNoneStatusSelections));
            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Statuses.Count > 0)
            {
                if (CheckedOnDelete)
                {
                    await _appService.DeleteAsync(new DeleteFilter()
                    {
                        Username = Username,
                        Statuses = Statuses,
                        CreatedFrom = CreatedFrom,
                        CreatedTo = CreatedTo
                    });
                }
                else
                {
                    await _appService.ResetStatusAsync(new ResetStatusFilter()
                    {
                        Username = Username,
                        Statuses = Statuses,
                        TargetStatus = TargetStatus,
                        CreatedFrom = CreatedFrom,
                        CreatedTo = CreatedTo
                    });
                }
            }
            else
            {
                throw new UserFriendlyException("The status is required");
            }
            return RedirectToPage("/AppleIdNones/ResetStatus");
        }
    }
}
