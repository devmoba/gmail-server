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

        public const string DecryptGroup = PermissionNames.Decrypt;
        public static class Decrypts
        {
            public const string Default = PermissionNames.Decrypt_Default;
        }

        public const string CheckMailGroup = PermissionNames.CheckMail;
        public static class CheckMails
        {
            public const string Default = PermissionNames.CheckMail_Default;
        }

        public const string CheckerGroup = PermissionNames.Checker;
        public static class Checkers
        {
            public const string Default = PermissionNames.Checker_Default;  
            public const string Delete = PermissionNames.Checker_Delete;  
        }

        public const string TaskCheckGroup = PermissionNames.TaskCheck;
        public static class TaskChecks
        {
            public const string Default = PermissionNames.TaskCheck_Default;
            public const string Create = PermissionNames.TaskCheck_Create;
            public const string Update = PermissionNames.TaskCheck_Update;
            public const string Delete = PermissionNames.TaskCheck_Delete;
        }

        public const string RecoveryEmailGroup = PermissionNames.RecoveryEmail;
        public static class RecoveryEmails
        {
            public const string Default = PermissionNames.RecoveryEmail_Default;
            public const string Create = PermissionNames.RecoveryEmail_Create;
            public const string Update = PermissionNames.RecoveryEmail_Update;
            public const string Delete = PermissionNames.RecoveryEmail_Delete;
        }
    }
}
