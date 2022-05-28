using Volo.Abp.Identity;

namespace GmailServer
{
    public static class GmailServerConsts
    {
        public const string DbTablePrefix = "App";
        public const string DbSchema = null;
        public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
        public const string AdminPasswordDefaultValue = IdentityDataSeedContributor.AdminPasswordDefaultValue;
    }
}
