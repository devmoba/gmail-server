using System.Threading.Tasks;

namespace GmailServer.Data
{
    public interface IGmailServerDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}