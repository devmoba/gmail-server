using GmailServer.CheckMails;
using GmailServer.EmailChecks;
using GmailServer.Enums;
using GmailServer.Extensions;
using GmailServer.Hubs;
using GmailServer.Permissions;
using GmailServer.TaskPools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GmailServer.Web.Pages.CheckMails
{
    [Authorize(GmailServerPermissions.CheckMails.Default)]
    public class IndexModel : GmailServerPageModel
    {
        private readonly IHubContext<CheckMailHub, ICheckMailHub> hubContext;

        [BindProperty]
        public string EmailCheckInput { get; set; }

        public string EmailResultOutput { get; set; }

        public IndexModel(IHubContext<CheckMailHub, ICheckMailHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            //if (!string.IsNullOrEmpty(EmailCheckInput))
            //{
            //    var connections = ConnectionMapping<string>
            //          .GetInstance()
            //          .GetConnections(CurrentUser.UserName)
            //          .ToList();

            //    var inputSplit = EmailCheckInput.Split("\r\n");
            //    var emailChecks = new List<EmailCheck>();
            //    for (int i = 0; i < inputSplit.Length; i++)
            //    {
            //        var email = inputSplit[i];

            //        if (!string.IsNullOrEmpty(email))
            //        {
            //            emailChecks.Add(new EmailCheck()
            //            {
            //                Email = email,
            //                Id = i,
            //            });
            //        }
            //    }

            //    if (emailChecks.Count > 70000)
            //    {
            //        var message = "Maxinum 70K emails!";
            //        await hubContext.Clients
            //            .Clients(connections)
            //            .ReceiveNotiAsync(message, "danger");
            //        return NoContent();
            //    }
            //    await hubContext.Clients.Clients(connections).ClearResultAsync();
            //    await hubContext.Clients
            //        .Clients(connections)
            //        .ReceiveTotalCheckAsync(emailChecks.Count);
            //    var emailCheckSplits = EnumerableExtension.Split<EmailCheck>(
            //            emailChecks, 
            //            (int)Math.Ceiling((decimal)emailChecks.Count / 5000)
            //        ).ToList();

            //    var cancelToken = new CancellationTokenSource();
            //    var taskPool = new TaskPool();

            //    var checkMailTask = new Task(() =>
            //        taskPool.StartThread(),
            //        cancelToken.Token
            //    );
            //    checkMailTask.Start();
            //    foreach (var ec in emailCheckSplits)
            //    {
            //        taskPool.EnqueueEmails(ec);
            //        var emailResults = new List<EmailResult>();
            //        var count = 0;

            //        while (count < ec.Count)
            //        {
            //            Thread.Sleep(700);
            //            var results = taskPool.GetResultAndClear();
            //            emailResults.AddRange(results);
            //            count += results.Count;
            //            await hubContext.Clients.Clients(connections)
            //                .ReceiveCountResultAsync(results.Count);
            //        }
            //        emailResults = emailResults.OrderBy(x => x.Id).ToList();
            //        var emailResultsString = emailResults
            //            .Select(x => $"{x.Email}|{Enum.GetName(typeof(Status), x.Status)}").ToList();
            //        var output = string.Join('\n', emailResultsString);

            //        await hubContext.Clients.Clients(connections)
            //            .ReceiveEmailResultOutputAsync(output);

            //        var emailResultGroups = emailResults
            //            .GroupBy(x => x.Status)
            //            .Select(group => new EmailResultGroup()
            //            {
            //                Status = group.Key,
            //                EmailResultOuput = string.Join('\n', group.Select(x => $"{x.Email}|{Enum.GetName(typeof(Status), x.Status)}").ToList()),
            //                Count = group.Count()
            //            }).ToList();
            //        foreach (var item in emailResultGroups)
            //        {
            //            await hubContext.Clients
            //                .Clients(connections)
            //                .ReceiveEmailResultGroupAsync(
            //                    item.EmailResultOuput,
            //                    item.Status,
            //                    item.Count
            //                );
            //        }
            //    }
            //    cancelToken.Cancel();
            //}
            return NoContent();
        }
    }
}
