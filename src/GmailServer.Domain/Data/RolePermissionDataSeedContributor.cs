using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace GmailServer.Data
{
    public class RolePermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private const string RoleNameCheckMailTool = "check-mail-tool";

        private readonly IdentityRoleManager _roleManager;
        private readonly IPermissionManager _permissionManger;

        public RolePermissionDataSeedContributor(IdentityRoleManager roleManager, IPermissionManager permissionManager)
        {
            _roleManager = roleManager;
            _permissionManger = permissionManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedRoleAsync();
        }

        private async Task SeedRoleAsync()
        {
            var checkMailToolRole = new IdentityRole(Guid.NewGuid(), RoleNameCheckMailTool, null)
            {
                IsDefault = false,
                IsPublic = false
            };
            await _roleManager.CreateAsync(checkMailToolRole);
        }
    }
}
