using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.Security;
using DotNetCore.Core.Domain.Accounts;

namespace DotNetCore.Service.Security
{
    public class StandardPermissionProvider : IPermissionProvider
    {
        #region Fileds
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
        public static readonly PermissionRecord MangageAccount = new PermissionRecord { Name = "Access mangage account", SystemName = "MangageAccount", Category = "Standard" };
        #endregion

        #region Methods

        public IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[]
            {
                new DefaultPermissionRecord
                {
                    AccountRoleName=AccountRoleNames.Administrators,
                    PermissionRecords=new[]
                    {
                        AccessAdminPanel,
                        MangageAccount
                    }
                }
            };
        }

        public IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel,
                MangageAccount
            };
        } 
        #endregion
    }
}
