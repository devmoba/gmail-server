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

            myGroup.AddPermission(GmailServerPermissions.Dashboard.Home, L("Permission:Home"));
            myGroup.AddPermission(GmailServerPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(GmailServerPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            var gmailGroup = context.AddGroup(GmailServerPermissions.GmailGroup, L("Permission:GmailGroup"));
            var gmailGroupManagement = gmailGroup.AddPermission(GmailServerPermissions.Gmails.Default, L("Permission:Gmails"));
            gmailGroupManagement.AddChild(GmailServerPermissions.Gmails.Download, L("Permission:GmailGroups.Download"));
            gmailGroupManagement.AddChild(GmailServerPermissions.Gmails.Delete, L("Permission:GmailGroups.Delete"));

            var gmailTypeGroup = context.AddGroup(GmailServerPermissions.GmailTypeGroup, L("Permission:GmailTypeGroup"));
            var gmailTypeManagement = gmailTypeGroup.AddPermission(GmailServerPermissions.GmailTypes.Default, L("Permission:GmailTypes"));
            gmailTypeManagement.AddChild(GmailServerPermissions.GmailTypes.Create, L("Permission:GmailTypes.Create"));
            gmailTypeManagement.AddChild(GmailServerPermissions.GmailTypes.Update, L("Permission:GmailTypes.Update"));
            gmailTypeManagement.AddChild(GmailServerPermissions.GmailTypes.Delete, L("Permission:GmailTypes.Delete"));

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

            var recoveryEmailGroup = context.AddGroup(GmailServerPermissions.RecoveryEmailGroup, L("Permission:RecoveryEmailGroup"));
            var recoveryEmailManagement = recoveryEmailGroup.AddPermission(GmailServerPermissions.RecoveryEmails.Default, L("Permission:RecoveryEmails"));
            recoveryEmailManagement.AddChild(GmailServerPermissions.RecoveryEmails.Create, L("Permission:RecoveryEmails.Create"));
            recoveryEmailManagement.AddChild(GmailServerPermissions.RecoveryEmails.Update, L("Permission:RecoveryEmails.Update"));
            recoveryEmailManagement.AddChild(GmailServerPermissions.RecoveryEmails.Delete, L("Permission:RecoveryEmails.Delete"));
            recoveryEmailManagement.AddChild(GmailServerPermissions.RecoveryEmails.Config, L("Permission:RecoveryEmails.Config"));

            var gmailPremiumGroup = context.AddGroup(GmailServerPermissions.GmailPremiumGroup, L("Permission:GmailPremiumGroup "));
            var gmailPremiumManagement = gmailPremiumGroup.AddPermission(GmailServerPermissions.GmailPremiums.Default, L("Permission:GmailPremiums"));
            gmailPremiumManagement.AddChild(GmailServerPermissions.GmailPremiums.Create, L("Permission:GmailPremiums.Create"));
            gmailPremiumManagement.AddChild(GmailServerPermissions.GmailPremiums.Update, L("Permission:GmailPremiums.Update"));
            gmailPremiumManagement.AddChild(GmailServerPermissions.GmailPremiums.Delete, L("Permission:GmailPremiums.Delete"));

            var appleIdGroup = context.AddGroup(GmailServerPermissions.AppleIdGroup, L("Permission:AppleIdGroup "));
            var appleIdManagement = appleIdGroup.AddPermission(GmailServerPermissions.AppleIds.Default, L("Permission:AppleIds"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.Create, L("Permission:AppleIds.Create"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.Update, L("Permission:AppleIds.Update"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.Delete, L("Permission:AppleIds.Delete"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.DeleteFilter, L("Permission:AppleIds.DeleteFilter"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.DeleteAll, L("Permission:AppleIds.DeleteAll"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.Download, L("Permission:AppleIds.Download"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.Statistic, L("Permission:AppleIds.Statistic"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.StatisticDaily, L("Permission:AppleIds.StatisticDaily"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.ResetStatus, L("Permission:AppleIds.ResetStatus"));
            appleIdManagement.AddChild(GmailServerPermissions.AppleIds.PurchaseNumber, L("Permission:AppleIds.PurchaseNumber"));

            var downloadedAppGroup = context.AddGroup(GmailServerPermissions.DownloadedAppGroup, L("Permission:DownloadedAppGroup "));
            var downloadedAppManagement = downloadedAppGroup.AddPermission(GmailServerPermissions.DownloadedApps.Default, L("Permission:DownloadedApps"));
            downloadedAppManagement.AddChild(GmailServerPermissions.DownloadedApps.Create, L("Permission:DownloadedApps.Create"));
            downloadedAppManagement.AddChild(GmailServerPermissions.DownloadedApps.Delete, L("Permission:DownloadedApps.Delete"));

            var gmailResourceGroup = context.AddGroup(GmailServerPermissions.GmailResourceGroup, L("Permission:GmailResourceGroup "));
            var gmailResourceManagement = gmailResourceGroup.AddPermission(GmailServerPermissions.GmailResources.Default, L("Permission:GmailResources"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.Create, L("Permission:GmailResource.Create"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.Update, L("Permission:GmailResource.Update"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.Delete, L("Permission:GmailResource.Delete"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.DeleteAll, L("Permission:GmailResource.DeleteAll"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.DeleteFilter, L("Permission:GmailResource.DeleteFilter"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.Download, L("Permission:GmailResource.Download"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.Statistic, L("Permission:GmailResource.Statistic"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.StatisticDaily, L("Permission:GmailResource.StatisticDaily"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.ResetStatus, L("Permission:GmailResource.ResetStatus"));
            gmailResourceManagement.AddChild(GmailServerPermissions.GmailResources.ReupEmail, L("Permission:GmailResource.ReupEmail"));

            var momoAccountGroup = context.AddGroup(GmailServerPermissions.MomoAccountGroup, L("Permission:MomoAccountGroup "));
            var momoAccountManagement = momoAccountGroup.AddPermission(GmailServerPermissions.MomoAccounts.Default, L("Permission:MomoAccounts"));
            momoAccountManagement.AddChild(GmailServerPermissions.MomoAccounts.Create, L("Permission:MomoAccount.Create"));
            momoAccountManagement.AddChild(GmailServerPermissions.MomoAccounts.CreateMany, L("Permission:MomoAccount.CreateMany"));
            momoAccountManagement.AddChild(GmailServerPermissions.MomoAccounts.Update, L("Permission:MomoAccount.Update"));
            momoAccountManagement.AddChild(GmailServerPermissions.MomoAccounts.Delete, L("Permission:MomoAccount.Delete"));
            momoAccountManagement.AddChild(GmailServerPermissions.MomoAccounts.DeleteAll, L("Permission:MomoAccount.DeleteAll"));
            momoAccountManagement.AddChild(GmailServerPermissions.MomoAccounts.Statistic, L("Permission:MomoAccount.Statistic"));
            momoAccountManagement.AddChild(GmailServerPermissions.MomoAccounts.ResetStatus, L("Permission:MomoAccount.ResetStatus"));

            var appleOrderGroup = context.AddGroup(GmailServerPermissions.AppleOrderGroup, L("Permissions:AppleOrderGroup"));
            var appleOrderManagement = appleOrderGroup.AddPermission(GmailServerPermissions.AppleOrders.Default, L("Permissions:AppleOrders"));
            appleOrderManagement.AddChild(GmailServerPermissions.AppleOrders.Create, L("Permission:AppleOrders.Create"));
            appleOrderManagement.AddChild(GmailServerPermissions.AppleOrders.Update, L("Permission:AppleOrders.Update"));
            appleOrderManagement.AddChild(GmailServerPermissions.AppleOrders.Delete, L("Permission:AppleOrders.Update"));
            appleOrderManagement.AddChild(GmailServerPermissions.AppleOrders.Statistic, L("Permission:AppleOrders.Statistic"));
            appleOrderManagement.AddChild(GmailServerPermissions.AppleOrders.ResetLinkStatus, L("Permission:AppleOrders.ResetLinkStatus"));

            var appleIdNoneGroup = context.AddGroup(GmailServerPermissions.AppleIdNoneGroup, L("Permission:AppleIdNoneGroup "));
            var appleIdNoneManagement = appleIdNoneGroup.AddPermission(GmailServerPermissions.AppleIdNones.Default, L("Permission:AppleIdNones"));
            appleIdNoneManagement.AddChild(GmailServerPermissions.AppleIdNones.Create, L("Permission:AppleIdNones.Create"));
            appleIdNoneManagement.AddChild(GmailServerPermissions.AppleIdNones.Update, L("Permission:AppleIdNones.Update"));
            appleIdNoneManagement.AddChild(GmailServerPermissions.AppleIdNones.Delete, L("Permission:AppleIdNones.Delete"));
            appleIdNoneManagement.AddChild(GmailServerPermissions.AppleIdNones.DeleteFilter, L("Permission:AppleIdNones.DeleteFilter"));
            appleIdNoneManagement.AddChild(GmailServerPermissions.AppleIdNones.DeleteAll, L("Permission:AppleIdNones.DeleteAll"));
            appleIdNoneManagement.AddChild(GmailServerPermissions.AppleIdNones.Download, L("Permission:AppleIdNones.Download"));
            appleIdNoneManagement.AddChild(GmailServerPermissions.AppleIdNones.Statistic, L("Permission:AppleIdNones.Statistic"));
            appleIdNoneManagement.AddChild(GmailServerPermissions.AppleIdNones.ResetStatus, L("Permission:AppleIdNones.ResetStatus"));
            appleIdNoneManagement.AddChild(GmailServerPermissions.AppleIdNones.PurchaseNumber, L("Permission:AppleIdNones.PurchaseNumber"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<GmailServerResource>(name);
        }
    }
}