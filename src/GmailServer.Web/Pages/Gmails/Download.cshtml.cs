using GmailServer.Entities;
using GmailServer.Gmails;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.Gmails
{
    [Authorize(GmailServerPermissions.Gmails.Download)]
    public class DownloadModel : GmailServerPageModel
    {
        [BindProperty]
        public bool CheckedAll { get; set; }

        [BindProperty]
        public bool CheckedTimeRange { get; set; }

        [BindProperty]
        public DateTime DateFrom { get; set; }

        [BindProperty]
        public DateTime DateTo { get; set; }

        private readonly IGmailRepository gmailRepository;

        public DownloadModel(IGmailRepository gmailRepository)
        {
            this.gmailRepository = gmailRepository;
        }

        public void OnGet()
        {
            DateTo = DateTime.Now.Date.AddDays(-1);
            DateFrom = DateTo.Date.AddDays(-1);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var res = new List<Gmail>();
            var fileName = string.Empty;
            if (CheckedAll)
            {
                res = await this.gmailRepository.GetAllAsync();
                fileName = "Gmails_All.xlsx";
            }

            if (CheckedTimeRange)
            {
                res = await this.gmailRepository.GetByTimeRangeAsync(DateFrom, DateTo);
                fileName = $"Gmails_TimeRange_{DateFrom.ToString("dd/MM/yyyy HH:mm")}-{DateTo.ToString("dd/MM/yyyy HH:mm")}.xlsx";
            }
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var gmailExcelModels = ObjectMapper.Map<List<Gmail>, List<GmailExcelModel>>(res);
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(gmailExcelModels, true);

                package.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
