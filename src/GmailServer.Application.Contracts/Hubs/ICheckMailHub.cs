using GmailServer.EmailChecks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GmailServer.Hubs
{
    public interface ICheckMailHub
    {
        Task ReceiveEmailCheckAsync(List<EmailCheck> emailChecks);
    }
}
