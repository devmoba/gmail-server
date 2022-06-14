namespace GmailServer.Permissions
{
    public static class GmailServerPermissions
    {
        public const string GroupName = "GmailServer";

        public static class Dashboard
        {
            public const string DashboardGroup = GroupName + ".Dashboard";
            public const string Host = DashboardGroup + ".Host";
            public const string Tenant = DashboardGroup + ".Tenant";
        }

        public const string GmailGroup = PermissionNames.Gmail;
        public static class Gmails
        {
            public const string Default = PermissionNames.Gmail_Default;
            public const string Download = PermissionNames.Gmail_Download;
            public const string Delete = PermissionNames.Gmail_Delete;
        }

        public const string FakeSettingGroup = PermissionNames.FakeSetting;
        public static class FakeSettings
        {
            public const string Default = PermissionNames.FakeSetting_Default;
            public const string Create = PermissionNames.FakeSetting_Create;
            public const string Update = PermissionNames.FakeSetting_Update;
            public const string Delete = PermissionNames.FakeSetting_Delete;
        }
    }
}
