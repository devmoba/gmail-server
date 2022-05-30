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
            var gmailGroupManagement = gmailGroup.AddPermission(GmailServerPermissions.Gmails.Default, L("Permission:GmailGroups"));
            gmailGroupManagement.AddChild(GmailServerPermissions.Gmails.Download, L("Permission:GmailGroups.Download"));
            gmailGroupManagement.AddChild(GmailServerPermissions.Gmails.Delete, L("Permission:GmailGroups.Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<GmailServerResource>(name);
        }
    }
}