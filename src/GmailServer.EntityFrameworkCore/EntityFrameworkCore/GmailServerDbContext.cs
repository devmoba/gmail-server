using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;
using Volo.Payment.EntityFrameworkCore;
using GmailServer.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;
using GmailServer.Enums;

namespace GmailServer.EntityFrameworkCore
{
    [ReplaceDbContext(typeof(IIdentityProDbContext))]
    [ReplaceDbContext(typeof(ISaasDbContext))]
    [ConnectionStringName("Default")]
    public class GmailServerDbContext :
        AbpDbContext<GmailServerDbContext>,
        IIdentityProDbContext,
        ISaasDbContext
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here. */

        #region Entities from the modules

        /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
         * and replaced them for this DbContext. This allows you to perform JOIN
         * queries for the entities of these modules over the repositories easily. You
         * typically don't need that for other modules. But, if you need, you can
         * implement the DbContext interface of the needed module and use ReplaceDbContext
         * attribute just like IIdentityProDbContext and ISaasDbContext.
         *
         * More info: Replacing a DbContext of a module ensures that the related module
         * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
         */

        // Identity
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityClaimType> ClaimTypes { get; set; }
        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
        public DbSet<IdentityLinkUser> LinkUsers { get; set; }

        // SaaS
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        // Use

        public DbSet<Gmail> Gmails { get; set; }

        public DbSet<FakeSetting> FakeSettings { get; set; }

        public DbSet<Checker> Checkers { get; set; }

        public DbSet<TaskCheck> TaskChecks { get; set; }

        public DbSet<RecoveryEmail> RecoveryEmails { get; set; }

        public DbSet<GmailPremium> GmailPremiums { get; set; }

        public DbSet<AppleId> AppleIds { get; set; }

        public DbSet<GmailResource> GmailResources { get; set; }

        public DbSet<GmailType> GmailTypes { get; set; }

        public DbSet<MomoAccount> MomoAccounts { get; set; }

        public DbSet<AppleOrder> AppleOrders { get; set; }

        public DbSet<AppleIdNone> AppleIdNones { get; set; }

        public DbSet<AppleIdRaw> AppleIdRaws { get; set; }

        public DbSet<OwnerConfig> OwnerConfigs { get; set; }

        public DbSet<Statistic> Statistics { get; set; }

        #endregion

        public GmailServerDbContext(DbContextOptions<GmailServerDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentityPro();
            builder.ConfigureIdentityServer();
            builder.ConfigureFeatureManagement();
            builder.ConfigureLanguageManagement();
            builder.ConfigurePayment();
            builder.ConfigureSaas();
            builder.ConfigureTextTemplateManagement();
            builder.ConfigureBlobStoring();

            builder.Entity<Gmail>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "Gmails", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Email).IsUnicode(false).HasMaxLength(128).IsRequired();
                b.Property(x => x.Password).IsUnicode(false).HasMaxLength(64).IsRequired();
                b.Property(x => x.RecoveryEmail).IsUnicode(false).HasMaxLength(128).IsRequired();
                b.Property(x => x.FirstName).HasMaxLength(128);
                b.Property(x => x.LastName).HasMaxLength(128);
                b.Property(x => x.Gender).HasMaxLength(26);
                b.Property(x => x.DateOfBirth).HasMaxLength(128);
                b.Property(x => x.Country).HasMaxLength(26);
                b.Property(x => x.Timezone).HasMaxLength(128);
                b.Property(x => x.SerialNumber).HasMaxLength(128);
                b.Property(x => x.DeviceType).HasMaxLength(128);
                b.Property(x => x.Version).HasMaxLength(128);
                b.Property(x => x.Created).IsRequired();
                b.Property(x => x.Updated).IsRequired();
                b.Property(x => x.LastCheck).IsRequired();  
                b.Property(x => x.TimeDiff).IsRequired();
                b.HasIndex(x => x.Id).IncludeProperties<Gmail>(g => new
                {
                    g.Status,
                    g.Created,
                    g.Updated,
                    g.LastCheck,
                    g.RecoveryEmail,
                    g.Country
                });
            });

            builder.Entity<GmailType>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "GmailTypes", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Name).IsUnique();
                b.Property(x => x.Name).HasMaxLength(128).IsRequired();
                b.Property(x => x.FakeVersion).HasMaxLength(128);
                b.Property(x => x.Country).HasMaxLength(26);
                b.Property(x => x.DeviceType).HasMaxLength(128);
                b.Property(x => x.Version).HasMaxLength(128);

                b.HasMany(x => x.Gmails).WithOne(x => x.GmailType).HasForeignKey(x => x.GmailTypeId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<FakeSetting>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "FakeSettings", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Version);
                b.Property(x => x.FakeVersion);
                b.Property(x => x.DeviceType);
            });

            builder.Entity<Checker>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "Checkers", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasIndex(x => x.CheckerId).IsUnique();
                b.Property(x => x.CheckerId).IsRequired();
                b.Property(x => x.CheckerIP).HasMaxLength(16);
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.UsingThread).IsRequired();
                b.Property(x => x.MaxThread).IsRequired();
                b.Property(x => x.Created).IsRequired();
                b.Property(x => x.LastCheck).IsRequired();
            });

            builder.Entity<TaskCheck>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "TaskChecks", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Id).IncludeProperties<TaskCheck>(tc => new
                {
                    tc.Username,
                    tc.EmailChecks,
                    tc.Status,
                    tc.TypeCheck,
                    tc.CheckerId,
                    tc.Created
                });
                b.Property(x => x.Username).IsRequired();
                b.Property(x => x.EmailChecks).IsRequired();
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.TypeCheck).IsRequired();
                b.Property(x => x.Created).IsRequired();
                b.HasOne(x => x.Checker).WithMany(x => x.TaskChecks).HasForeignKey(x => x.CheckerId);
                
            });

            builder.Entity<RecoveryEmail>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "RecoveryEmails", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Id).IncludeProperties<RecoveryEmail>(re => new
                {
                    re.Username,
                    re.Status
                });
                b.Property(x => x.Username).IsUnicode(false).HasMaxLength(256).IsRequired();
                b.Property(x => x.Email).IsUnicode(false).HasMaxLength(128).IsRequired();
                b.Property(x => x.Password).IsUnicode(false).HasMaxLength(64).IsRequired();
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.Created).IsRequired();
            });

            builder.Entity<GmailPremium>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "GmailPremiums", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Email).IsUnique();
                b.HasIndex(x => x.Id).IncludeProperties<GmailPremium>(re => new
                {
                    re.Username,
                    re.Status
                });
                b.Property(x => x.Username).IsUnicode(false).HasMaxLength(256).IsRequired();
                b.Property(x => x.Email).IsUnicode(false).HasMaxLength(128).IsRequired();
                b.Property(x => x.Password).IsUnicode(false).HasMaxLength(64).IsRequired();
                b.Property(x => x.RecoveryEmail).IsUnicode(false).HasMaxLength(128).IsRequired();
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.Created).IsRequired();
                b.Property(x => x.Updated).IsRequired();
                b.Property(x => x.TakenTime).IsRequired();
            });

            builder.Entity<AppleId>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "AppleIds", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Email).IsUnique();
                b.HasIndex(x => x.Id).IncludeProperties<AppleId>(re => new
                {
                    re.Username,
                    re.Status
                });
                b.Property(x => x.Username).IsUnicode(false).HasMaxLength(256).IsRequired();
                b.Property(x => x.Email).IsUnicode(false).HasMaxLength(128).IsRequired();
                b.Property(x => x.Password).IsUnicode(false).HasMaxLength(64).IsRequired();
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.Created).IsRequired();
                b.Property(x => x.Updated).IsRequired();
                b.Property(x => x.TakenTime).IsRequired();
                b.Property(x => x.PurchaseNumber).IsRequired();
                b.Property(x => x.TakenOutNumber).IsRequired();
                b.Property(x => x.Ccv).HasMaxLength(64).IsRequired(false);
                b.Property(x => x.SecretAnswer1).IsRequired(false);
                b.Property(x => x.SecretAnswer2).IsRequired(false);
                b.Property(x => x.SecretAnswer3).IsRequired(false);
                b.Property(x => x.DateOfBirth).HasMaxLength(128).IsRequired(false);
            });

            builder.Entity<DownloadedApp>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "DownloadedApps", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.AppId).IsRequired();
                b.Property(x => x.ProductId).IsRequired();
                b.Property(x => x.Email).IsUnicode(false).HasMaxLength(128);
                b.Property(x => x.Created).IsRequired();
                b.HasIndex(x => x.Id).IncludeProperties<DownloadedApp>(da => new
                {
                    da.AppId,
                    da.ProductId,
                    da.Email,
                    da.Created
                });
                b.HasOne(x => x.AppleId).WithMany(x => x.DownloadedApps).HasForeignKey(x => x.AppleIdFK).OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<GmailResource>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "GmailResources", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Email).IsUnique();
                b.HasIndex(x => x.Id).IncludeProperties<GmailResource>(re => new
                {
                    re.Username,
                    re.Status
                });
                b.Property(x => x.Username).IsUnicode(false).HasMaxLength(256).IsRequired();
                b.Property(x => x.Email).IsUnicode(false).HasMaxLength(128).IsRequired();
                b.Property(x => x.Password).IsUnicode(false).HasMaxLength(64).IsRequired();
                b.Property(x => x.RecoveryEmail).IsUnicode(false).HasMaxLength(128);
                b.Property(x => x.Country).HasMaxLength(128).IsRequired(false);
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.PremiumType).IsRequired().HasDefaultValue(PremiumType.Unset);
                b.Property(x => x.Created).IsRequired();
                b.Property(x => x.Updated).IsRequired();
                b.Property(x => x.UpdatedPremium).IsRequired();
                b.Property(x => x.TakenTime).IsRequired();
            });

            builder.Entity<MomoAccount>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "MomoAccounts", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Username).IsUnique();
                b.HasIndex(x => x.Id).IncludeProperties<MomoAccount>(re => new
                {
                    re.UploadGroup,
                    re.CreatedTime,
                    re.Email,
                    re.Status,
                    re.CurrentLinkCount,
                    re.TotalLinkCount,
                    re.LastUpdateTime,
                    re.LastTakenTime
                });
                b.Property(x => x.UploadGroup).HasMaxLength(1024).IsRequired();
                b.Property(x => x.Username).IsUnicode(false).HasMaxLength(256).IsRequired();
                b.Property(x => x.Email).IsUnicode(false).HasMaxLength(128).IsRequired();
                b.Property(x => x.Password).IsUnicode(false).HasMaxLength(64).IsRequired();
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.CreatedTime).IsRequired();
            });

            builder.Entity<AppleOrder>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "AppleOrders", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasIndex(x => x.Id).IncludeProperties<AppleOrder>(re => new
                {
                    re.OrderID,
                    re.URLPayment,
                    re.LinkStatus,
                    re.AddPaymentStatus,
                    re.LinkTakenTime,
                    re.LinkCompletedTime,
                    re.AddPaymentTakenTime, 
                    re.AddPaymentCompletedTime
                });
                b.Property(x => x.OrderID).IsRequired();
                b.Property(x => x.URLPayment).IsRequired();
                b.Property(x => x.CreatedTime).IsRequired();
                b.Property(x => x.LinkStatus).IsRequired();
                b.Property(x => x.AddPaymentCompletedTime).IsRequired();
            });

            builder.Entity<AppleIdNone>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "AppleIdNones", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Email).IsUnique();
                b.HasIndex(x => x.Id).IncludeProperties<AppleIdNone>(re => new
                {
                    re.Username,
                    re.Created,
                    re.Status,
                    re.AddPaymentCompleted,
                    re.RemovePaymentStatus,
                    re.RemoveTakenTime,
                    re.RemoveUpdateTime
                });
                b.Property(x => x.Username).IsUnicode(false).HasMaxLength(256).IsRequired();
                b.Property(x => x.Email).IsUnicode(false).HasMaxLength(128).IsRequired();
                b.Property(x => x.Password).IsUnicode(false).HasMaxLength(64).IsRequired();
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.Created).IsRequired();
                b.Property(x => x.PurchaseNumber).IsRequired();
                b.Property(x => x.TakenOutNumber).IsRequired();
                b.Property(x => x.RemovePaymentStatus).IsRequired();
                b.Property(x => x.AddPaymentCompleted).HasDefaultValue(false);
            });

            builder.Entity<AppleIdRaw>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "AppleIdRaws", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Username).IsUnique();
                b.Property(x => x.Password).IsUnicode(false).HasMaxLength(64).IsRequired();
                b.Property(x => x.Created).IsRequired();

                b.HasIndex(x => x.Id).IncludeProperties<AppleIdRaw>(re => new
                {
                    re.Created
                });
            });

            builder.Entity<OwnerConfig>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "OwnerConfigs", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => x.Key).IsUnique();
                b.Property(x => x.Key).IsRequired();
                b.Property(x => x.Value).IsRequired();
            });

            builder.Entity<Statistic>(b =>
            {
                b.ToTable(GmailServerConsts.DbTablePrefix + "Statistics", GmailServerConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasIndex(x => x.HashCode).IsUnique();
                b.Property(x => x.HashCode).HasMaxLength(128).IsRequired();
                b.Property(x => x.EntityName).HasMaxLength(128).IsRequired();
                b.Property(x => x.Type).IsRequired();
                b.Property(x => x.Data).IsRequired();
                b.HasIndex(x => x.Id).IncludeProperties<Statistic>(re => new
                {
                    re.Date,
                    re.EntityName,
                    re.Type
                });
            });
        }
    }
}
