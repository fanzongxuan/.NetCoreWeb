using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Core.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Accounts
{
    public interface IAccountService
    {
        IdentityResult Register(Account entitiy, string password);

        Account GetById(string id);

        IdentityResult Update(Account entitiy);

        IdentityResult Delete(Account entitiy);

        IPagedList<Account> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue);

        void LoginOut();

        SignInResult LoginWithUserNameAndPwd(string userName, string pwd, bool remeberMe, bool lockOutOnFail);

        void Login(Account user, bool remeberMe);

        Account GetByName(string accountName);

        Account GetAuthenticationAccount();

        bool IsInRole(Account account, string roleName);

        AccountRole GetAccountRoleBySystemName(string systemName);

        Account InsertGuestAccount();

        bool AccountIsExist(string userName);

        IdentityResult AddToRole(Account account, string roleName);

        bool RoleExists(string roleName);

        IdentityResult CreateRole(AccountRole role);

        AccountRole FindRoleByName(string name);

        IList<string> GetRoleNamesByAccount(Account account);
    }
}
