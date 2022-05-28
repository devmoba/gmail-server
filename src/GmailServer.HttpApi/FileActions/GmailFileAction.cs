using GmailServer.Gmails;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GmailServer.FileActions
{
    public class GmailFileAction : FileResult
    {
        private readonly List<GmailDto> gmailCsvResults;
        public GmailFileAction(List<GmailDto> gmails, string fileDownloadName) : base("text/csv")
        {
            FileDownloadName = fileDownloadName;
            this.gmailCsvResults = gmails;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { $"attachment; filename={FileDownloadName}" });
            context.HttpContext.Response.ContentType = "application/octet-stream";

            using (var streamWriter = new StreamWriter(response.Body))
            {
                var header = "Date,FirstName,LastName,Email,Password,RecoveryEmail,DateOfBirth,Gender,Timezone," +
                    "FakeVersion,SerialNumber,DeviceType,Version,Country,Status,Arg1,Arg2,Arg3";
                await streamWriter.WriteLineAsync(header);
                this.gmailCsvResults.ForEach(async rs =>
                {
                    var line = $"{rs.Date.ToString("dd/MM/yyyy HH:mm")},{rs.FirstName},{rs.LastName},{rs.Email},{rs.Password},{rs.RecoveryEmail}" +
                    $",{rs.DateOfBirth.ToString("dd/MM/yyyy")},{(int)rs.Gender},{rs.Timezone},{rs.FakeVersion},{rs.SerialNumber},{rs.DeviceType}" +
                    $",{rs.Version},{rs.Country},{(int)rs.Status},{rs.Arg1},{rs.Arg2},{rs.Arg3}";
                    await streamWriter.WriteLineAsync(line);
                    await streamWriter.FlushAsync();
                });
                await streamWriter.FlushAsync();
            }
        }
    }
}
