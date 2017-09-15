using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Accounts
{
    public static class RoleProvider
    {
        public static IEnumerable<AccountRole> GetRoles()
        {
            return new AccountRole[] {
                new AccountRole(AccountRoleNames.Administrators),
                new AccountRole(AccountRoleNames.Register),
                new AccountRole(AccountRoleNames.Guest),
                new AccountRole(AccountRoleNames.SystemRole)
        };
        }
    }
}
