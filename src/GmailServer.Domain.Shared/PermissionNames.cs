namespace GmailServer
{
    public static class PermissionNames
    {
        public const string Gmail = "GmailGroup";
        public const string Gmail_Default = Gmail + ".Gmails";
        public const string Gmail_Download = Gmail + ".Download";
        public const string Gmail_Delete = Gmail + ".Delete";

        public const string FakeSetting = "FakeSettingGroup";
        public const string FakeSetting_Default = FakeSetting + ".FakeSettings";
        public const string FakeSetting_Create = FakeSetting + ".Create";
        public const string FakeSetting_Update = FakeSetting + ".Update";
        public const string FakeSetting_Delete = FakeSetting + ".Delete";

        public const string Decrypt = "DecryptGroup";
        public const string Decrypt_Default = Decrypt + ".Decrypt";

        public const string CheckMail = "CheckMailGroup";
        public const string CheckMail_Default = CheckMail + ".CheckMails";

        public const string Checker = "CheckerGroup";
        public const string Checker_Default = Checker + ".Checkers";
        //public const string Checker_Create = Checker + ".Create";
        //public const string Checker_Update = Checker + ".Update";
        public const string Checker_Delete = Checker + ".Delete";

        public const string TaskCheck = "TaskCheckGroup";
        public const string TaskCheck_Default = TaskCheck + ".TaskChecks";
        public const string TaskCheck_Create = TaskCheck + ".Create";
        public const string TaskCheck_Update = TaskCheck + ".Update";
        public const string TaskCheck_Delete = TaskCheck + ".Delete";

    }
}
