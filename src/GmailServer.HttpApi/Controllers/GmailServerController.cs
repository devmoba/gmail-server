using GmailServer.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace GmailServer.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class GmailServerController : AbpController
    {
        protected GmailServerController()
        {
            LocalizationResource = typeof(GmailServerResource);
        }
    }
}