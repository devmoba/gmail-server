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
            await SeedPermissionAsync();
        }

        private async Task SeedRoleAsync()
        {
            var checkMailToolRole = new IdentityRole(Guid.NewGuid(), RoleName.RoleNameCheckMailTool, null)
            {
                IsDefault = false,
                IsPublic = false
            };

            var appleIdMember = new IdentityRole(Guid.NewGuid(), RoleName.RoleNameAppleIdMember, null)
            {
                IsDefault = false,
                IsPublic = false
            };

            var appleIdManager = new IdentityRole(Guid.NewGuid(), RoleName.RoleNameAppleIdManager, null)
            {
                IsDefault = false,
                IsPublic = false
            };

            await _roleManager.CreateAsync(checkMailToolRole);
            await _roleManager.CreateAsync(appleIdMember);
            await _roleManager.CreateAsync(appleIdManager);

        }

        private async Task SeedPermissionAsync()
        {
            await _permissionManger.SetForRoleAsync(RoleName.RoleNameAppleIdMember, PermissionNames.AppleId_Default, isGranted: true);
            await _permissionManger.SetForRoleAsync(RoleName.RoleNameAppleIdMember, PermissionNames.AppleId_StatisticDaily, isGranted: true);

            await _permissionManger.SetForRoleAsync(RoleName.RoleNameAppleIdManager, PermissionNames.AppleId_Default, isGranted: true);
            await _permissionManger.SetForRoleAsync(RoleName.RoleNameAppleIdManager, PermissionNames.AppleId_Create, isGranted: true);
            await _permissionManger.SetForRoleAsync(RoleName.RoleNameAppleIdManager, PermissionNames.AppleId_Statistic, isGranted: true);
            await _permissionManger.SetForRoleAsync(RoleName.RoleNameAppleIdManager, PermissionNames.AppleId_StatisticDaily, isGranted: true);
        }
    }
}
