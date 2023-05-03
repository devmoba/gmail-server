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
using GmailServer.Entities;
using static GmailServer.Permissions.GmailServerPermissions;

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

            if (await context.IsGrantedAsync(GmailServerPermissions.GmailTypes.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.GmailType,
                       "Gmail Type",
                       "/GmailTypes",
                       icon: "fa fa-server",
                       order: 2
                   )
               );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.Gmails.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.Gmail,
                       "Gmail",
                       "/Gmails",
                       icon: "fa fa-google",
                       order: 3
                   )
               );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.GmailPremiums.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.GmailPremium,
                       "Gmail Premium",
                       "/GmailPremiums",
                       icon: "fa fa-glide",
                       order: 4
                   )
               );
            }

            var gmailResource = new ApplicationMenuItem(GmailServerMenus.GmailResource, "Gmail Resource", order: 5, icon: "fa fa-google-plus-official");

            if (await context.IsGrantedAsync(GmailServerPermissions.GmailResources.Default))
            {
                gmailResource.AddItem(
                  new ApplicationMenuItem(
                      GmailServerMenus.GmailResource,
                      "Gmail Resource",
                      "/GmailResources",
                      order: 1
                  )
              );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.GmailResources.Statistic))
            {
                gmailResource.AddItem(
                  new ApplicationMenuItem(
                      GmailServerMenus.GmailResource,
                      "Statistic",
                      "/GmailResources/Statistic",
                      order: 2
                  )
              );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.GmailResources.Download))
            {
                gmailResource.AddItem(
                  new ApplicationMenuItem(
                      GmailServerMenus.GmailResource,
                      "Download",
                      "/GmailResources/Download",
                      order: 3
                  )
              );
            }

            context.Menu.AddItem(gmailResource);

            var apple = new ApplicationMenuItem(GmailServerMenus.Apple, "Apple", order: 6, icon: "fa fa-apple");
            if (await context.IsGrantedAsync(GmailServerPermissions.AppleIds.Default))
            {
                var appleId = new ApplicationMenuItem(GmailServerMenus.AppleId, "Apple ID", order: 1);
                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIds.Default))
                {
                    appleId.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleId,
                           "Apple ID",
                           "/AppleIds",
                           order: 1
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIds.Statistic))
                {
                    appleId.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleId,
                           "Statistic",
                           "/AppleIds/Statistic",
                           order: 2
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIds.ResetStatus))
                {
                    appleId.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleIdResetStatus,
                           "Reset Status",
                           "/AppleIds/ResetStatus",
                           order: 3
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIds.Download))
                {
                    appleId.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleIdDownload,
                           "Download",
                           "/AppleIds/Download",
                           order: 4
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.DownloadedApps.Default))
                {
                    appleId.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.DownloadedApp,
                           "Apps",
                           "/DownloadedApps",
                           order: 5
                       )
                   );
                }
                apple.AddItem(appleId);
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.AppleIdNones.Default))
            {
                var appleIdNone = new ApplicationMenuItem(GmailServerMenus.AppleIdNone, "AppleID-None", order: 2);
                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIdNones.Default))
                {
                    appleIdNone.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleIdNone,
                           "AppleID-None",
                           "/AppleIdNones",
                           order: 1
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIdNones.Statistic))
                {
                    appleIdNone.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleIdNoneStatistic,
                           "Statistic",
                           "/AppleIdNones/Statistic",
                           order: 2
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIdNones.ResetStatus))
                {
                    appleIdNone.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleIdNoneResetStatus,
                           "Reset Status",
                           "/AppleIdNones/ResetStatus",
                           order: 3
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIdNones.ResetRemovePaymentStatus))
                {
                    appleIdNone.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleIdNoneResetRemovePaymentStatus,
                           "Reset Remove Payment",
                           "/AppleIdNones/ResetRemovePaymentStatus",
                           order: 4
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIdNones.Download))
                {
                    appleIdNone.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleIdNoneDownload,
                           "Download",
                           "/AppleIdNones/Download",
                           order: 5
                       )
                   );
                }
                apple.AddItem(appleIdNone);
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.AppleOrders.Default))
            {
                var appleOrder = new ApplicationMenuItem(GmailServerMenus.AppleOrder, "Apple Order", order: 3);
                if (await context.IsGrantedAsync(GmailServerPermissions.AppleOrders.Default))
                {
                    appleOrder.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleOrder,
                           "Apple Order",
                           "/AppleOrders",
                           order: 1
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.AppleOrders.Statistic))
                {
                    var statistic = new ApplicationMenuItem(GmailServerMenus.AppleOrderStatistic, "Statistic", order: 2);

                    statistic.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleOrderStatistic,
                           "Link Status",
                           "/AppleOrders/StatisticByLinkStatus",
                           order: 2
                       )
                    );

                    statistic.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleOrderStatistic,
                           "Add Payment Status",
                           "/AppleOrders/StatisticByAddPaymentStatus",
                           order: 3
                       )
                    );

                    appleOrder.AddItem(statistic);
                }

                apple.AddItem(appleOrder);
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.AppleIdRaws.Default))
            {
                var appleIdRaw = new ApplicationMenuItem(GmailServerMenus.AppleIdRaw, "AppleID-Raw", order: 3);
                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIdRaws.Statistic))
                {
                    appleIdRaw.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleIdRawStatistic,
                           "Statistic",
                           "/AppleIdRaws/Statistic",
                           order: 1
                       )
                   );
                }

                if (await context.IsGrantedAsync(GmailServerPermissions.AppleIdRaws.Download))
                {
                    appleIdRaw.AddItem(
                       new ApplicationMenuItem(
                           GmailServerMenus.AppleIdRawDownload,
                           "Download",
                           "/AppleIdRaws/Download",
                           order: 2
                       )
                   );
                }

                apple.AddItem(appleIdRaw);
            }

            context.Menu.AddItem(apple);
            var momoAccount = new ApplicationMenuItem(GmailServerMenus.MomoAccount, "Momo Account", order: 7, icon: "fa fa-credit-card");
            if (await context.IsGrantedAsync(GmailServerPermissions.MomoAccounts.Default))
            {
                momoAccount.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.MomoAccount,
                       "Momo Account",
                       "/MomoAccounts",
                       order: 1
                   )
               );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.MomoAccounts.Statistic))
            {
                momoAccount.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.MomoAccount,
                       "Statistic",
                       "/MomoAccounts/Statistic",
                       order: 1
                   )
               );
            }

            if (await context.IsGrantedAsync(GmailServerPermissions.MomoAccounts.ResetStatus))
            {
                momoAccount.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.MomoAccount,
                       "Reset Status",
                       "/MomoAccounts/ResetStatus",
                       order: 2
                   )
               );
            }
            context.Menu.AddItem(momoAccount);
            //var appleId = new ApplicationMenuItem(GmailServerMenus.AppleId, "Apple ID", order: 6, icon: "fa fa-apple");

            //if (await context.IsGrantedAsync(GmailServerPermissions.AppleIds.Default))
            //{
            //    appleId.AddItem(
            //       new ApplicationMenuItem(
            //           GmailServerMenus.AppleId,
            //           "Apple ID",
            //           "/AppleIds",
            //           order: 1
            //       )
            //   );
            //}

            //if (await context.IsGrantedAsync(GmailServerPermissions.AppleIds.Statistic))
            //{
            //    appleId.AddItem(
            //       new ApplicationMenuItem(
            //           GmailServerMenus.AppleId,
            //           "Statistics",
            //           "/AppleIds/Statistic",
            //           order: 2
            //       )
            //   );
            //}

            //if (await context.IsGrantedAsync(GmailServerPermissions.AppleIds.Download))
            //{
            //    appleId.AddItem(
            //       new ApplicationMenuItem(
            //           GmailServerMenus.AppleId,
            //           "Download",
            //           "/AppleIds/Download",
            //           order: 3
            //       )
            //   );
            //}

            //if (await context.IsGrantedAsync(GmailServerPermissions.DownloadedApps.Default))
            //{
            //    appleId.AddItem(
            //       new ApplicationMenuItem(
            //           GmailServerMenus.DownloadedApp,
            //           "Apps",
            //           "/DownloadedApps",
            //           order: 4
            //       )
            //   );
            //}

            //context.Menu.AddItem(appleId);

            if (await context.IsGrantedAsync(GmailServerPermissions.RecoveryEmails.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       GmailServerMenus.RecoveryEmail,
                       "Recovery Email",
                       "/RecoveryEmails",
                       icon: "fa fa-registered",
                       order: 8
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
                       order: 9
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
                       order: 10
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
                       order: 11
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
                       order: 12
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

            context.Menu.SetSubItemOrder(SaasHostMenuNames.GroupName, 13);

            //Administration
            //var administration = context.Menu.GetAdministration();
            administration.Order = 14;

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
