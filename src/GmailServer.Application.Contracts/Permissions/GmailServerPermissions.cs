namespace GmailServer.Permissions
{
    public static class GmailServerPermissions
    {
        public const string GroupName = "GmailServer";

        public static class Dashboard
        {
            public const string Home = GroupName + PermissionNames.Home;
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

        public const string GmailTypeGroup = PermissionNames.GmailType;
        public static class GmailTypes
        {
            public const string Default = PermissionNames.GmailType_Default;
            public const string Create = PermissionNames.GmailType_Create;
            public const string Update = PermissionNames.GmailType_Update;
            public const string Delete = PermissionNames.GmailType_Delete;
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
            public const string Config = PermissionNames.RecoveryEmail_Config;
        }

        public const string GmailPremiumGroup = PermissionNames.GmailPremium;
        public static class GmailPremiums
        {
            public const string Default = PermissionNames.GmailPremium_Default;
            public const string Create = PermissionNames.GmailPremium_Create;
            public const string Update = PermissionNames.GmailPremium_Update;
            public const string Delete = PermissionNames.GmailPremium_Delete;
        }

        public const string AppleIdGroup = PermissionNames.AppleId;
        public static class AppleIds
        {
            public const string Default = PermissionNames.AppleId_Default;  
            public const string Create = PermissionNames.AppleId_Create;  
            public const string Update = PermissionNames.AppleId_Update;  
            public const string Delete = PermissionNames.AppleId_Delete;  
            public const string DeleteAll = PermissionNames.AppleId_DeleteAll;  
            public const string DeleteFilter = PermissionNames.AppleId_DeleteFilter;  
            public const string Download = PermissionNames.AppleId_Download;
            public const string Statistic = PermissionNames.AppleId_Statistic;
            public const string StatisticDaily = PermissionNames.AppleId_StatisticDaily;
            public const string ResetStatus = PermissionNames.AppleId_ResetStatus;
        }

        public const string DownloadedAppGroup = PermissionNames.DownloadedApp;

        public static class DownloadedApps
        {
            public const string Default = PermissionNames.DownloadedApp_Default;
            public const string Create = PermissionNames.DownloadedApp_Create;
            public const string Delete = PermissionNames.DownloadedApp_Delete;
        }

        public const string GmailResourceGroup = PermissionNames.GmailResource;
        public static class GmailResources
        {
            public const string Default = PermissionNames.GmailResource_Default;
            public const string Create = PermissionNames.GmailResource_Create;
            public const string Update = PermissionNames.GmailResource_Update;
            public const string Delete = PermissionNames.GmailResource_Delete;
            public const string Download = PermissionNames.GmailResource_Download;
            public const string Statistic = PermissionNames.GmailResource_Statistic;
            public const string StatisticDaily = PermissionNames.GmailResource_StatisticDaily;
            public const string ResetStatus = PermissionNames.GmailResource_ResetStatus;
            public const string ReupEmail = PermissionNames.GmailResource_ReupEmail;
        }
    }
}
