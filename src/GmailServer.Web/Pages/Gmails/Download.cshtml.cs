using GmailServer.FileActions;
using GmailServer.Gmails;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.Gmails
{
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

        private readonly IGmailAppService gmailAppService;

        public DownloadModel(IGmailAppService gmailAppService)
        {
            this.gmailAppService = gmailAppService; 
        }

        public void OnGet()
        {

        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (CheckedAll)
            {
                var res = await this.gmailAppService.GetAll();
                return new GmailFileAction(res, $"Gmails_All.csv");
            }

            if (CheckedTimeRange)
            {
                var res = await this.gmailAppService.GetByTimeRange(DateFrom, DateTo);
                return new GmailFileAction(res, $"Gmails_TimeRange_{DateFrom.ToString("dd/MM/yyyy HH:mm")}-{DateTo.ToString("dd/MM/yyyy HH:mm")}.csv");
            }

            return NoContent();
        }
    }
}
