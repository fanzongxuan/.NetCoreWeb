using DotNetCore.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Accounts
{
    public class RolePermissionMap : BaseEntity
    {
        public string AccountRoleId { get; set; }

        public int PermissionRecordId { get; set; }

        public AccountRole AccountRole { get; set; }

        public PermissionRecord PermissionRecord { get; set; }
    }
}
