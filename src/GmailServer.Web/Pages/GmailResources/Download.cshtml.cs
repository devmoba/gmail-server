using GmailServer.Enums;
using GmailServer.GmailResources;
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

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.Download)]
    public class DownloadModel : GmailServerPageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public List<GmailResourceStatus> Statuses { get; set; }

        [BindProperty]
        [Required]
        public string FileName { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? CreatedTo { get; set; }

        private readonly IGmailResourceAppService gmailResourceAppService;

        public DownloadModel(IGmailResourceAppService gmailResourceAppService)
        {
            this.gmailResourceAppService = gmailResourceAppService;
        }

        public async Task OnGetAsync()
        {
            var usernameSelections = await this.gmailResourceAppService.GetUsernameSelectionAsync();
           
            usernameSelections.AddFirst(new UsernameSelectionDto()
            {
                Text = "All Username",
                Value = string.Empty
            });

            var gmailResourceStatusSelections = Enum.GetValues(typeof(GmailResourceStatus)).Cast<GmailResourceStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();

            ViewData.Add("gmailResourceStatusSelections", SerializeObject(gmailResourceStatusSelections));
            ViewData.Add("usernameSelections", SerializeObject(usernameSelections));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var gmailResources = await this.gmailResourceAppService.GetGmailResourceExcelModelsAsync(new GmailResourceDownloadFilter()
                {
                    Username = Username,
                    Statuses = Statuses,
                    CreatedFrom = CreatedFrom,
                    CreatedTo = CreatedTo
                });
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(gmailResources, true);
                package.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{FileName}.xlsx");
        }
    }
}
