using System.Threading.Tasks;

namespace GmailServer.Hubs
{
    public interface IReupGmailResourceHub
    {
        Task ReceiveNotiAsync(string message, string type);
    }
}
