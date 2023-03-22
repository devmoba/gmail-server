using GmailServer.AppleIds;
using GmailServer.Enums;
using GmailServer.GmailResources;
using GmailServer.Hubs;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OfficeOpenXml;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace GmailServer.Web.Pages.GmailResources
{
    [Authorize(GmailServerPermissions.GmailResources.ReupEmail)]
    public class ReupModalModel : GmailServerPageModel
    {
        [BindProperty]
        public ReupFormModel ReupForm { get; set; }

        public string AlertMessage { get; set; }

        private readonly IGmailResourceAppService gmailResourceAppService;
        private readonly IHubContext<ReupGmailResourceHub, IReupGmailResourceHub> hubContext;

        public ReupModalModel(IGmailResourceAppService gmailResourceAppService,
            IHubContext<ReupGmailResourceHub, IReupGmailResourceHub> hubContext)
        {
            this.gmailResourceAppService = gmailResourceAppService;
            this.hubContext = hubContext;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<ReupFormModel, ReupGmailResourceInputDto>(ReupForm);
            try
            {
                var reuptOutputs = await this.gmailResourceAppService.ReupAsync(input);
                var connections = ConnectionMapping<string>.GetInstance().GetConnections(CurrentUser.UserName).ToList();
                if (reuptOutputs.Count > 0)
                {
                    var stream = new MemoryStream();
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                        workSheet.Cells.LoadFromCollection(reuptOutputs, true);
                        package.Save();
                    }
                    stream.Position = 0;
                    
                    await this.hubContext.Clients.Clients(connections).ReceiveNotiAsync(
                        $"Reup Successfully! There are some emails duplicated or non-existen in the Database. Pls, Open file to check!", 
                        "warning");
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Reup_Output.xlsx");
                }
                await this.hubContext.Clients.Clients(connections).ReceiveNotiAsync(
                        $"Reup Successfully!",
                        "success");
                return NoContent();
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            
        }
    }


    public class ReupFormModel
    {
        [Required]
        [DisplayName("New username")]
        public string Username { get; set; }

        [Required]
        [TextArea(Rows = 35)]
        [Placeholder("email|password|recoveryEmail(optional)|country(optional)")]
        public string Emails { get; set; }
    }
}
