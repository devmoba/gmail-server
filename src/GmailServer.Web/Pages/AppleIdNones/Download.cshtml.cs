using GmailServer.AppleIdNones;
using GmailServer.Enums;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.AppleIdNones
{
    [Authorize(GmailServerPermissions.AppleIdNones.Download)]
    public class DownloadModel : GmailServerPageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public List<AppleIdNoneStatus> Statuses { get; set; }

        [BindProperty]
        [Required]
        public string FileName { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTo { get; set; }

        private readonly IAppleIdNoneAppService _appService;

        public DownloadModel(IAppleIdNoneAppService appService)
        {
            _appService = appService;
        }

        public async Task OnGetAsync()
        {
            var usernameSelections = await this._appService.GetUsernameSelectionAsync();

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
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var appleIds = await _appService.GetAppleIdNoneExcelModelsAsync(new AppleIdNoneDownloadFilter()
                {
                    Username = Username,
                    Statuses = Statuses,
                    CreatedFrom = CreatedFrom,
                    CreatedTo = CreatedTo
                });
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(appleIds, true);
                package.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{FileName}.xlsx");
        }
    }
}
