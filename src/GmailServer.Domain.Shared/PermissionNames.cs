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
    }
}
