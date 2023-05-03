using GmailServer.AppleIdNones;
using GmailServer.Enums;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System;
using Volo.Abp;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GmailServer.Web.Pages.AppleIdNones
{
    [Authorize(GmailServerPermissions.AppleIdNones.ResetRemovePaymentStatus)]
    public class ResetRemovePaymentStatusModel : GmailServerPageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public List<RemovePaymentStatus> Statuses { get; set; }

        [BindProperty]
        [Required]
        public RemovePaymentStatus TargetStatus { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTo { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? RemoveTakenTimeFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? RemoveTakenTimeTo { get; set; }

        private readonly IAppleIdNoneAppService _appService;
        public ResetRemovePaymentStatusModel(IAppleIdNoneAppService appService)
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
            var removePaymentStatusSelections = Enum.GetValues(typeof(RemovePaymentStatus)).Cast<RemovePaymentStatus>()
             .Select(item => new SelectListItem()
             {
                 Text = item.ToString(),
                 Value = $"{(int)item}"
             }).ToList();

            ViewData.Add("removePaymentStatusSelections", SerializeObject(removePaymentStatusSelections));
            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Statuses.Count > 0)
            {
                await _appService.ResetRemovePaymentStatusAsync(new ResetRemovePaymentStatusFilter()
                {
                    Username = Username,
                    Statuses = Statuses,
                    TargetStatus = TargetStatus,
                    CreatedFrom = CreatedFrom,
                    CreatedTo = CreatedTo,
                    RemoveTakenTimeFrom = RemoveTakenTimeFrom,
                    RemoveTakenTimeTo = RemoveTakenTimeTo
                });
            }
            else
            {
                throw new UserFriendlyException("The status is required");
            }
            return RedirectToPage("/AppleIdNones/ResetRemovePaymentStatus");
        }
    }
}
