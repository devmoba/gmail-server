using GmailServer.Enums;
using GmailServer.MomoAccounts;
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

namespace GmailServer.Web.Pages.MomoAccounts
{
    [Authorize(GmailServerPermissions.MomoAccounts.ResetStatus)]
    public class ResetStatusModel : GmailServerPageModel
    {
        [BindProperty]
        public string UploadGroup { get; set; }

        [BindProperty]
        [Required]
        public List<MomoAccountStatus> Statuses { get; set; }

        [BindProperty]
        [Required]
        public MomoAccountStatus TargetStatus { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTimeFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTimeTo { get; set; }

        [BindProperty]
        public bool CheckedOnDelete { get; set; }

        private readonly IMomoAccountAppService _appService;
        public ResetStatusModel(IMomoAccountAppService appService)
        {
            _appService = appService;
        }

        public async Task OnGetAsync()
        {
            var uploadGroupSelections = await _appService.GetUploadGroupSelectionAsync();
            uploadGroupSelections.AddFirst(new UploadGroupSelectionDto()
            {
                Text = "All Username",
                Value = string.Empty
            });

            var momoAccountStatusSelections = Enum.GetValues(typeof(MomoAccountStatus)).Cast<MomoAccountStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();

            ViewData.Add("uploadGroupSelections", SerializeObject(uploadGroupSelections));
            ViewData.Add("momoAccountStatusSelections", SerializeObject(momoAccountStatusSelections));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Statuses.Count > 0)
            {
                if (CheckedOnDelete)
                {
                    await _appService.DeleteFilterAsync(new DeleteFilterInput()
                    {
                        UploadGroup = UploadGroup,
                        Statuses = Statuses,
                        CreatedTimeFrom = CreatedTimeFrom,
                        CreatedTimeTo = CreatedTimeTo
                    });
                }
                else
                {
                    await _appService.ResetStatusAsync(new ResetStatusFilterInput()
                    {
                        UploadGroup = UploadGroup,
                        Statuses = Statuses,
                        TargetStatus = TargetStatus,
                        CreatedTimeFrom = CreatedTimeFrom,
                        CreatedTimeTo = CreatedTimeTo
                    });
                }
            } 
            else
            {
                throw new UserFriendlyException("The status is required");
            }
            return RedirectToPage("/MomoAccounts/ResetStatus");
        }
    }
}
