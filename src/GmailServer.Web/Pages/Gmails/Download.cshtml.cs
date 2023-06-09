using GmailServer.Enums;
using GmailServer.Gmails;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.Gmails
{
    [Authorize(GmailServerPermissions.Gmails.Download)]
    public class DownloadModel : GmailServerPageModel
    {
        [BindProperty]
        public List<Status> Statuses { get; set; }

        [BindProperty]
        [Required]
        public string FileName { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTo { get; set; }

        private readonly IGmailAppService _appService;

        public DownloadModel(IGmailAppService appService)
        {
            _appService = appService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var appleIds = await _appService.GetGmailExcelModelsAsync(new GmailDownloadFilter()
                {
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
