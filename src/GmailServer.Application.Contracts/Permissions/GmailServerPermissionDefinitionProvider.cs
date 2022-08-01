using GmailServer.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace GmailServer.Permissions
{
    public class GmailServerPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(GmailServerPermissions.GroupName);

            myGroup.AddPermission(GmailServerPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(GmailServerPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            var gmailGroup = context.AddGroup(GmailServerPermissions.GmailGroup, L("Permission:GmailGroup"));
            var gmailGroupManagement = gmailGroup.AddPermission(GmailServerPermissions.Gmails.Default, L("Permission:Gmails"));
            gmailGroupManagement.AddChild(GmailServerPermissions.Gmails.Download, L("Permission:GmailGroups.Download"));
            gmailGroupManagement.AddChild(GmailServerPermissions.Gmails.Delete, L("Permission:GmailGroups.Delete"));

            var fakeSettingGroup = context.AddGroup(GmailServerPermissions.FakeSettingGroup, L("Permission:FakeSettingGroup"));
            var fakeSettingManagement = fakeSettingGroup.AddPermission(GmailServerPermissions.FakeSettings.Default, L("Permission:FakeSettings"));
            fakeSettingManagement.AddChild(GmailServerPermissions.FakeSettings.Create, L("Permission:FakeSettings.Create"));
            fakeSettingManagement.AddChild(GmailServerPermissions.FakeSettings.Update, L("Permission:FakeSettings.Update"));
            fakeSettingManagement.AddChild(GmailServerPermissions.FakeSettings.Delete, L("Permission:FakeSettings.Delete"));

            var decryptGroup = context.AddGroup(GmailServerPermissions.DecryptGroup, L("Permission:DecryptGroup"));
            decryptGroup.AddPermission(GmailServerPermissions.Decrypts.Default, L("Permission:Decrypts"));

            var checkMailGroup = context.AddGroup(GmailServerPermissions.CheckMailGroup, L("Permission:CheckMailGroup"));
            checkMailGroup.AddPermission(GmailServerPermissions.CheckMails.Default, L("Permission:CheckMails"));

            var checkerGroup = context.AddGroup(GmailServerPermissions.CheckerGroup, L("Permission:CheckerGroup"));
            var checkerManagement = checkerGroup.AddPermission(GmailServerPermissions.Checkers.Default, L("Permission:Checkers"));
            checkerManagement.AddChild(GmailServerPermissions.Checkers.Delete, L("Permission:Checkers.Delete"));

            var taskCheckGroup = context.AddGroup(GmailServerPermissions.TaskCheckGroup, L("Permission:TaskCheckGroup"));
            var taskCheckManagement = taskCheckGroup.AddPermission(GmailServerPermissions.TaskChecks.Default, L("Permission:TaskChecks"));
            taskCheckManagement.AddChild(GmailServerPermissions.TaskChecks.Create, L("Permission:TaskChecks.Create"));
            taskCheckManagement.AddChild(GmailServerPermissions.TaskChecks.Update, L("Permission:TaskChecks.Update"));
            taskCheckManagement.AddChild(GmailServerPermissions.TaskChecks.Delete, L("Permission:TaskChecks.Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<GmailServerResource>(name);
        }
    }
}