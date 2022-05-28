using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace GmailServer.Web
{
    [Dependency(ReplaceServices = true)]
    public class GmailServerBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "GmailServer";
    }
}
