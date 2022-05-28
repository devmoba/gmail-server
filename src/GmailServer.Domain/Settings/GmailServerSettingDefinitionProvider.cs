using Volo.Abp.Settings;

namespace GmailServer.Settings
{
    public class GmailServerSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(GmailServerSettings.MySetting1));
        }
    }
}
