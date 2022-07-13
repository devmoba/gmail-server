using GmailServer.CheckerReports;
using GmailServer.CheckMails;
using GmailServer.EmailChecks;
using GmailServer.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GmailServer.Hubs
{
    public interface ICheckMailHub
    {
        Task ReceiveEmailCheckAsync(List<EmailCheck> emailChecks);

        Task ReceiveEmailResultAsync(CheckMailResult emailResults);

        Task ReceiveEmailResultAsync(List<EmailResultDto> EmailResults);

        Task ReceiveEmailResultOutputAsync(string output);

        Task ReceiveEmailResultGroupAsync(string emailResult, Status status, int count);

        Task ReceiveCountResultAsync(int count);

        Task ReceiveTotalCheckAsync(int count);

        Task ReceiveNotiAsync(string message, string type);

        Task ClearResultAsync();
    }
}
