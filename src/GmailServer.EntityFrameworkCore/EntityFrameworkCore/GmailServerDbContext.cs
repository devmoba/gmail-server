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
                b.Property(x => x.Emails).IsRequired();
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.Created).IsRequired();
            });
        }
    }
}
