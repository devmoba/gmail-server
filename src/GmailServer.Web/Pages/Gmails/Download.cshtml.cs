using GmailServer.Entities;
using GmailServer.Gmails;
using GmailServer.GmailTypes;
using GmailServer.Permissions;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public bool CheckedGmailType { get; set; }

        [BindProperty]
        public long? GmailTypeId { get; set; }

        [BindProperty]
        public DateTime DateFrom { get; set; }

        [BindProperty]
        public DateTime DateTo { get; set; }

        private readonly IGmailRepository gmailRepository;
        private readonly IGmailTypeAppService gmailTypeAppService;

        public DownloadModel(IGmailRepository gmailRepository, IGmailTypeAppService gmailTypeAppService)
        {
            this.gmailRepository = gmailRepository;
            this.gmailTypeAppService = gmailTypeAppService;
        }

        public async void OnGet()
        {
            DateTo = DateTime.Now.Date.AddDays(-1);
            DateFrom = DateTo.Date.AddDays(-1);

            var gmailTypes = await gmailTypeAppService.GetAllSelectionAsync();
            var gmailTypeSelections = gmailTypes.Select(item => new SelectListItem()
            {
                Text = item.Name,
                Value = $"{item.Id}"
            }).ToList();
            gmailTypeSelections.AddFirst(new SelectListItem()
            {
                Text = "Non of Gmail Type",
                Value = "null"
            });

            ViewData.Add("gmailTypeSelections", SerializeObject(gmailTypeSelections));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var res = new List<Gmail>();
            var fileName = "Gmails_All";
            var query = this.gmailRepository.AsQueryable();
           
            if (CheckedTimeRange)
            {
                query = query.Where(x => x.Created >= DateFrom);
                query = query.Where(x => x.Created <= DateTo);
                fileName = $"Gmails_TimeRange_{DateFrom.ToString("dd/MM/yyyy HH:mm")}-{DateTo.ToString("dd/MM/yyyy HH:mm")}";
            }

            if (CheckedGmailType)
            {
                query = query.Where(x => x.GmailTypeId == GmailTypeId);
                fileName += $"_{GmailTypeId}";
            }

            fileName += ".xlsx";
            res = await query.ToListAsync();
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
