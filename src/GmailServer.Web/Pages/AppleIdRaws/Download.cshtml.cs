using GmailServer.AppleIdRaws;
using GmailServer.Entities;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Web.Pages.AppleIdRaws
{
    [Authorize(GmailServerPermissions.AppleIdRaws.Download)]
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

        private readonly IRepository<AppleIdRaw, long> _repository;
         
        public DownloadModel(IRepository<AppleIdRaw, long> repository)
        {
            _repository = repository;
        }

        public async void OnGet()
        {
            DateTo = DateTime.Now;
            DateFrom = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var fileName = "AppleIdRaws_All";
            var query = _repository.AsQueryable();

            if (CheckedTimeRange)
            {
                query = query.Where(x => x.Created.Date >= DateFrom.Date);
                query = query.Where(x => x.Created.Date <= DateTo.Date);
                fileName = $"AppleIdRaws_TimeRange_{DateFrom.ToString("dd/MM/yyyy")}-{DateTo.ToString("dd/MM/yyyy")}";
            }

            fileName += ".xlsx";
            var res = await query.ToListAsync();
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var excelModels = ObjectMapper.Map<List<AppleIdRaw>, List<AppleIdRawDto>>(res);
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(excelModels, true);
                package.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
