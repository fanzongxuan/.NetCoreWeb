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
        #region Account

        IdentityResult Register(Account entitiy, string password);

        Account GetById(string id);

        IdentityResult Update(Account entitiy);

        IdentityResult Delete(Account entitiy);

        IPagedList<Account> QueryPageable(string userName, string email, string keywords, int pageIndex = 0, int pageSize = int.MaxValue);

        void LoginOut();

        SignInResult LoginWithUserNameAndPwd(string userName, string pwd, bool remeberMe, bool lockOutOnFail);

        void Login(Account user, bool remeberMe);

        Account GetByName(string accountName);

        Account GetAuthenticationAccount();

        bool IsInRole(Account account, string roleName);

        AccountRole GetAccountRoleBySystemName(string systemName);

        void CreateAccountIfNotExist(Account account, string password);

        Account InsertGuestAccount();

        bool AccountIsExist(string userName);

        IdentityResult AddToRole(Account account, string roleName);

        bool IsInAnyRole(Account account, List<string> roleNames);
        #endregion

        #region Role

        void AddToRoleIfNotIn(string userName, string roleName);

        bool RoleExists(string roleName);

        IdentityResult CreateRole(AccountRole role);

        void CreateRoleIfNotExist(AccountRole role);

        AccountRole FindRoleByName(string name);

        IList<string> GetRoleNamesByAccount(Account account);

        void InstallRoles();
        #endregion
    }
}
