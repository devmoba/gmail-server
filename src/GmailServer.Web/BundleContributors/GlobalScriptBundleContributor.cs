using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace GmailServer.Web.BundleContributors
{
    public class GlobalScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/moment/moment.js");
            context.Files.Add("/libs/devmoba/core/devmoba.js");
           
        }
    }
}
