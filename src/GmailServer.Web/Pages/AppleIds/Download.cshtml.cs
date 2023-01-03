using GmailServer.AppleIds;
using GmailServer.Enums;
using GmailServer.Permissions;
using GmailServer.RecoveryEmails;
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

namespace GmailServer.Web.Pages.AppleIds
{
    [Authorize(GmailServerPermissions.AppleIds.Download)]
    public class DownloadModel : GmailServerPageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public List<AppleIdStatus> Statuses { get; set; }

        [BindProperty]
        [Required]
        public string FileName { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTo { get; set; }

        private readonly IAppleIdAppService appleIdAppService;

        public DownloadModel(IAppleIdAppService appleIdAppService)
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
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var appleIds = await this.appleIdAppService.GetAppleIdExcelModelsAsync(new AppleIdDownloadFilter()
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
