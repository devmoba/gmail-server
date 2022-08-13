using GmailServer.CheckerReports;
using GmailServer.EmailChecks;
using GmailServer.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GmailServer.Hubs
{
    public interface ICheckMailHub
    {
        Task ReceiveEmailResultAsync(List<EmailResultDto> EmailResults);

        Task ReceiveNotiAsync(string message, string type);
    }
}
