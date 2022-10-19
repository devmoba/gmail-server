using GmailServer.Background;
using GmailServer.EntityFrameworkCore;
using GmailServer.Localization;
using GmailServer.MultiTenancy;
using GmailServer.Permissions;
using GmailServer.Web.BundleContributors;
using GmailServer.Web.HealthChecks;
using GmailServer.Web.Menus;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Admin.Web;
using Volo.Abp.Account.Public.Web.ExternalProviders;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AuditLogging.Web;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.IdentityServer.Web;
using Volo.Abp.LanguageManagement;
using Volo.Abp.LeptonTheme.Management;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TextTemplateManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Saas.Host;

namespace GmailServer.Web
{
    [DependsOn(
        typeof(GmailServerHttpApiModule),
        typeof(GmailServerApplicationModule),
        typeof(GmailServerEntityFrameworkCoreModule),
        typeof(GmailServerBackgroundModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpAccountPublicWebIdentityServerModule),
        typeof(AbpAuditLoggingWebModule),
        typeof(LeptonThemeManagementWebModule),
        typeof(SaasHostWebModule),
        typeof(AbpAccountAdminWebModule),
        typeof(AbpIdentityServerWebModule),
        typeof(LanguageManagementWebModule),
        typeof(AbpAspNetCoreMvcUiLeptonThemeModule),
        typeof(TextTemplateManagementWebModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreSignalRModule)
        )]
    public class GmailServerWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(GmailServerResource),
                    typeof(GmailServerDomainModule).Assembly,
                    typeof(GmailServerDomainSharedModule).Assembly,
                    typeof(GmailServerApplicationModule).Assembly,
                    typeof(GmailServerApplicationContractsModule).Assembly,
                    typeof(GmailServerWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureBundles();
            ConfigureUrls(configuration);
            ConfigurePages(configuration);
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureNavigationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);
            ConfigureExternalProviders(context);
            ConfigureHealthChecks(context);

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Gmails", GmailServerPermissions.Gmails.Default);
                options.Conventions.AuthorizePage("/AppleIds", GmailServerPermissions.AppleIds.Default);
                options.Conventions.AuthorizePage("/GmailPremiums", GmailServerPermissions.GmailPremiums.Default);
                options.Conventions.AuthorizePage("/GmailResources", GmailServerPermissions.GmailResources.Default);
                options.Conventions.AuthorizePage("/RecoveryEmails", GmailServerPermissions.RecoveryEmails.Default);
                options.Conventions.AuthorizePage("/GmailTypes", GmailServerPermissions.GmailTypes.Default);
                options.Conventions.AuthorizePage("/Gmails/Download", GmailServerPermissions.Gmails.Download);
                options.Conventions.AuthorizePage("/FakeSettings", GmailServerPermissions.FakeSettings.Default);
                options.Conventions.AuthorizePage("/Decrypt", GmailServerPermissions.Decrypts.Default);
                options.Conventions.AuthorizePage("/CheckMails", GmailServerPermissions.CheckMails.Default);
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options.ScriptBundles.Configure(
                   StandardBundles.Scripts.Global,
                   bundleConfig =>
                   {
                       bundleConfig.AddContributors(typeof(GlobalScriptBundleContributor));
                   });

                options.ScriptBundles.Add("knockout",
                  bundle => bundle.AddFiles(
                      "/libs/knockout-js/knockout.js",
                      "/libs/knockout-js/number-formatting.js"
                  ));

                options.ScriptBundles.Add("chosen",
                   bundle => bundle.AddFiles(
                       "/libs/chosen/chosen.jquery.js",
                       "/libs/chosen/chosen.ko.js"
                   ));


                options.ScriptBundles.Add("pretty-json",
                  bundle => bundle.AddFiles(
                      "/libs/pretty-json/backbone-min.js",
                      "/libs/pretty-json/underscore-min.js",
                      "/libs/pretty-json/pretty-json-min.js"
                  ));

                options.ScriptBundles.Add("crypto-js",
                  bundle => bundle.AddFiles(
                      "/libs/crypto-js/decrypt.js"
                  ));

                options.ScriptBundles.Add("canvasjs",
                 bundle => bundle.AddFiles(
                     "/libs/canvasjs/canvasjs.min.js"
                 ));

                options.ScriptBundles.Add("codemirror",
                 bundle => bundle.AddFiles(
                     "/libs/codemirror/codemirror.js"
                 ));

                options.StyleBundles.Add("chosen", bundle => bundle.AddFiles("/styles/chosen/component-chosen.css"));

                options.StyleBundles.Add("pretty-json",
                    bundle => bundle.AddFiles(
                       "/libs/pretty-json/css/pretty-json.css"
                    ));

                options.StyleBundles.Add("codemirror",
                    bundle => bundle.AddFiles(
                       "/libs/codemirror/codemirror.css"
                    ));
            });

        }

        private void ConfigureHealthChecks(ServiceConfigurationContext context)
        {
            context.Services.AddGmailServerHealthChecks();
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    LeptonThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );
            });
        }

        private void ConfigurePages(IConfiguration configuration)
        {
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/HostDashboard", GmailServerPermissions.Dashboard.Host);
                options.Conventions.AuthorizePage("/TenantDashboard", GmailServerPermissions.Dashboard.Tenant);
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]); ;
                    options.Audience = "GmailServer";

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/signalr-hubs/check-mail")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GmailServerWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GmailServerWebModule>();

                if (hostingEnvironment.IsDevelopment())
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<GmailServerDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}GmailServer.Domain.Shared", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<GmailServerDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}GmailServer.Domain", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<GmailServerApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}GmailServer.Application.Contracts", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<GmailServerApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}GmailServer.Application", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<GmailServerHttpApiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}GmailServer.HttpApi", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<GmailServerWebModule>(hostingEnvironment.ContentRootPath);
                }
            });
        }

        private void ConfigureNavigationServices()
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new GmailServerMenuContributor());
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new GmailServerToolbarContributor());
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(GmailServerApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddAbpSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "GmailServer API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        private void ConfigureExternalProviders(ServiceConfigurationContext context)
        {
            context.Services.AddAuthentication()
                .AddGoogle(GoogleDefaults.AuthenticationScheme, _ => { })
                .WithDynamicOptions<GoogleOptions, GoogleHandler>(
                    GoogleDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.WithProperty(x => x.ClientId);
                        options.WithProperty(x => x.ClientSecret, isSecret: true);
                    }
                )
                .AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options =>
                {
                    //Personal Microsoft accounts as an example.
                    options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                    options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
                })
                .WithDynamicOptions<MicrosoftAccountOptions, MicrosoftAccountHandler>(
                    MicrosoftAccountDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.WithProperty(x => x.ClientId);
                        options.WithProperty(x => x.ClientSecret, isSecret: true);
                    }
                )
                .AddTwitter(TwitterDefaults.AuthenticationScheme, options => options.RetrieveUserDetails = true)
                .WithDynamicOptions<TwitterOptions, TwitterHandler>(
                    TwitterDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.WithProperty(x => x.ConsumerKey);
                        options.WithProperty(x => x.ConsumerSecret, isSecret: true);
                    }
                );
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "GmailServer API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
