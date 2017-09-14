using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.Security;
using DotNetCore.Core.Domain.Accounts;

namespace DotNetCore.Service.Security
{
    public class StandardPermissionProvider : IPermissionProvider
    {
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };

        public IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[]
            {
                new DefaultPermissionRecord
                {
                    AccountRoleName=AccountRoleNames.Administrators,
                    PermissionRecords=new[]
                    {
                        AccessAdminPanel
                    }
                }
            };
        }

        public IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel
            };
        }
    }
}
