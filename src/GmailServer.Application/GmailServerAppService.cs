using GmailServer.Localization;
using Volo.Abp.Application.Services;

namespace GmailServer
{
    /* Inherit your application services from this class.
     */
    public abstract class GmailServerAppService : ApplicationService
    {
        protected GmailServerAppService()
        {
            LocalizationResource = typeof(GmailServerResource);
        }
    }
}
