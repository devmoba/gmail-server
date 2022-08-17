using System.Threading.Tasks;
using GmailServer.Localization;
using GmailServer.Permissions;
using Volo.Abp.AuditLogging.Web.Navigation;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.IdentityServer.Web.Navigation;
using Volo.Abp.LanguageManagement.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TextTemplateManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using Volo.Saas.Host.Navigation;
using GmailServer.MultiTenancy;

namespace GmailServer.Web.Menus
{
    public class GmailServerMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<GmailServerResource>();
            var administration = context.Menu.GetAdministration();
            if (!MultiTenancyConsts.IsEnabled)
            {
                administration.TryRemoveMenuItem(SaasHostMenuNames.GroupName);
            }
            //Home
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    GmailServerMenus.Home,
                    l["Menu:Home"],
                    "~/",
                    icon: "fa fa-home",
                    order: 1
                )
            );

            if (await context.IsGrantedAsync(GmailServerPermissions.Gmails.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.Gmail,
                       "Gmail",
                       "/Gmails",
                       icon: "fa fa-list-ul",
                       order: 2
                   )
               );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.FakeSettings.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.FakeSetting,
                       "Fake Setting",
                       "/FakeSettings",
                       icon: "fa fa-cogs",
                       order: 3
                   )
               );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.Decrypts.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.Decrypt,
                       "Decrypt",
                       "/Decrypt",
                       icon: "fa fa-object-ungroup",
                       order: 4
                   )
               );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.CheckMails.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.CheckMail,
                       "Check Mail",
                       "/CheckMails",
                       icon: "fa-check-square",
                       order: 5
                   )
               );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.Checkers.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.Checker,
                       "Checker",
                       "/Checkers",
                       icon: "fa fa-cog",
                       order: 6
                   )
               );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.RecoveryEmails.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.RecoveryEmail,
                       "Recovery Email",
                       "/RecoveryEmails",
                       icon: "fa fa-registered",
                       order: 7
                   )
               );
            }
            //HostDashboard
            //context.Menu.AddItem(
            //    new ApplicationMenuItem(
            //        GmailServerMenus.HostDashboard,
            //        l["Menu:Dashboard"],
            //        "~/HostDashboard",
            //        icon: "fa fa-line-chart",
            //        order: 2
            //    ).RequirePermissions(GmailServerPermissions.Dashboard.Host)
            //);

            ////TenantDashboard
            //context.Menu.AddItem(
            //    new ApplicationMenuItem(
            //        GmailServerMenus.TenantDashboard,
            //        l["Menu:Dashboard"],
            //        "~/Dashboard",
            //        icon: "fa fa-line-chart",
            //        order: 2
            //    ).RequirePermissions(GmailServerPermissions.Dashboard.Tenant)
            //);

            context.Menu.SetSubItemOrder(SaasHostMenuNames.GroupName, 8);

            //Administration
            //var administration = context.Menu.GetAdministration();
            administration.Order = 9;

            //Administration->Identity
            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);

            //Administration->Identity Server
            administration.SetSubItemOrder(AbpIdentityServerMenuNames.GroupName, 2);

            //Administration->Language Management
            administration.SetSubItemOrder(LanguageManagementMenuNames.GroupName, 3);

            //Administration->Text Template Management
            administration.SetSubItemOrder(TextTemplateManagementMainMenuNames.GroupName, 4);

            //Administration->Audit Logs
            administration.SetSubItemOrder(AbpAuditLoggingMainMenuNames.GroupName, 5);

            //Administration->Settings
            administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 6);

        }
    }
}
