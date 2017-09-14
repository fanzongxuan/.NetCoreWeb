using DotNetCore.Core.Domain.Security;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Accounts
{
    public class AccountRole : IdentityRole
    {
        private ICollection<RolePermissionMap> _rolePermissionMap;

        public AccountRole() : base()
        {

        }

        public AccountRole(string roleName) : base(roleName)
        {

        }


        public virtual ICollection<RolePermissionMap> RolePermissionMaps
        {
            get { return _rolePermissionMap ?? (_rolePermissionMap = new List<RolePermissionMap>()); }
            protected set { _rolePermissionMap = value; }
        }
    }
}
