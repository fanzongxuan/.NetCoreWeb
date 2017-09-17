using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Core.Interface;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Linq;
using DotNetCore.Core;
using Microsoft.AspNetCore.Http;
using DotNetCore.Core.Cache;
using DotNetCore.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Service.Accounts
{
    public class AccountService : IAccountService
    {
        #region Const

        private const string ACCOUNT_BY_SYSTEMNAME_KEY = "Web.accountrole.systemname-{0}";
        #endregion

        #region Ctor

        private readonly SignInManager<Account> _signManager;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<AccountRole> _accountRoleManager;
        private readonly ICacheManager _cacheManager;
        private Account _cachedAccount;
        #endregion

        #region Methods

        public AccountService(UserManager<Account> userManager,
            SignInManager<Account> signManager,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<AccountRole> accountRoleManager,
            ICacheManager cacheManager)
        {
            _userManager = userManager;
            _signManager = signManager;
            _httpContextAccessor = httpContextAccessor;
            _cacheManager = cacheManager;
            _accountRoleManager = accountRoleManager;
        }

        public IdentityResult Delete(Account entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("Account");
            return _userManager.DeleteAsync(entitiy).Result;
        }

        public Account GetById(string id)
        {
            return _userManager.Users.FirstOrDefault(x => x.Id == id);
        }

        public IPagedList<Account> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _userManager.Users;
            return new PagedList<Account>(query, pageIndex, pageSize);
        }

        public IdentityResult Register(Account entitiy, string pwd)
        {
            if (entitiy == null)
                throw new ArgumentNullException("entitiy");

            entitiy.CreateOnUtc = DateTime.UtcNow;
            var res = _userManager.CreateAsync(entitiy, pwd);

            if (res.Result.Succeeded)
            {
                var resRole = _userManager.AddToRoleAsync(entitiy, AccountRoleNames.Register).Result;
                if (!resRole.Succeeded)
                {
                    throw new Exception($"Errors:{string.Join(";", resRole.ErrorDescrirtions())}");
                }

            }

            return res.Result;
        }

        public IdentityResult Update(Account entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("entitiy");

            return _userManager.UpdateAsync(entitiy).Result;
        }

        public SignInResult LoginWithUserNameAndPwd(string userName, string pwd, bool remeberMe, bool lockOutOnFail)
        {
            return _signManager.PasswordSignInAsync(userName, pwd, remeberMe, lockOutOnFail).Result;

        }

        public void Login(Account user, bool remeberMe)
        {
            user.LastActivityDateUtc = DateTime.UtcNow;
            _signManager.SignInAsync(user, remeberMe);
        }

        public void LoginOut()
        {
            _signManager.SignOutAsync();
        }

        public Account GetByName(string accountName)
        {
            if (string.IsNullOrEmpty(accountName))
                return null;
            return _userManager.FindByNameAsync(accountName).Result;
        }

        public Account GetAuthenticationAccount()
        {
            //TODO Due to dependency have some issues,so cancel account cache
            //if (_cachedAccount != null)
            //    return _cachedAccount;

            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Request == null || !_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated || _httpContextAccessor.HttpContext.User == null)
                return null;

            var identityPrincipal = _httpContextAccessor.HttpContext.User;
            var account = _userManager.GetUserAsync(identityPrincipal).Result;

            if (account != null)
                _cachedAccount = account;
            return _cachedAccount;
        }

        public bool IsInRole(Account account, string roleName)
        {
            if (account == null || string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("account or roleName");
            return _userManager.IsInRoleAsync(account, roleName).Result;
        }

        public virtual AccountRole GetAccountRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            string key = string.Format(ACCOUNT_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _accountRoleManager.Roles
                            orderby cr.Id
                            where cr.Name == systemName
                            select cr;
                var accountRole = query.FirstOrDefault();
                return accountRole;
            });
        }

        public Account InsertGuestAccount()
        {

            var account = new Account()
            {
                UserName = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid().ToString(),
                CreateOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow
            };
            var res = _userManager.CreateAsync(account).Result;
            if (res.Succeeded)
            {
                var guestRole = GetAccountRoleBySystemName(AccountRoleNames.Guest);
                if (guestRole == null)
                    throw new ArgumentException("'Guests' role could not be loaded");
                var roleRes = _userManager.AddToRoleAsync(account, guestRole.Name).Result;
                if (roleRes.Succeeded)
                {
                    return account;
                }
                else
                {
                    throw new Exception("attach guest to guest role error!");
                }
            }
            else
            {
                throw new Exception("Insert a guest error!");
            }
        }

        public bool AccountIsExist(string userName)
        {
            return _userManager.Users.Any(x => x.UserName == userName);
        }

        public void CreateAccountIfNotExist(Account account, string password)
        {
            if (!AccountIsExist(account.UserName))
            {
                var res = Register(account, password);
                if (!res.Succeeded)
                    throw new Exception($"create role error:{string.Join(";", res.ErrorDescrirtions())}");

            }
        }

        public IdentityResult AddToRole(Account account, string roleName)
        {
            return _userManager.AddToRoleAsync(account, roleName).Result;
        }

        public bool RoleExists(string roleName)
        {
            return _accountRoleManager.RoleExistsAsync(roleName).Result;
        }

        public IdentityResult CreateRole(AccountRole role)
        {
            return _accountRoleManager.CreateAsync(role).Result;
        }

        public AccountRole FindRoleByName(string name)
        {
            return _accountRoleManager.Roles.Where(x => x.Name == name)
                    .Include("RolePermissionMaps.PermissionRecord")
                    .FirstOrDefault();
        }

        public IList<string> GetRoleNamesByAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            return _userManager.GetRolesAsync(account).Result;
        }

        public void CreateRoleIfNotExist(AccountRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            if (!RoleExists(role.Name))
            {
                var res = CreateRole(role);
                if (!res.Succeeded)
                {
                    throw new Exception($"create role error:{string.Join(";", res.ErrorDescrirtions())}");
                }
            }
        }

        public void InstallRoles()
        {
            var roles = RoleProvider.GetRoles();
            foreach (var role in roles)
            {
                CreateRoleIfNotExist(role);
            }
        }

        public void AddToRoleIfNotIn(string userName, string roleName)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("userName or roleName!");

            var account = _userManager.FindByNameAsync(userName).Result;
            if (account == null)
                throw new Exception("account is not exist!");

            var res = IsInRole(account, roleName);
            if (!res)
            {
                var identityRes = AddToRole(account, roleName);
                if (!identityRes.Succeeded)
                    throw new Exception($"Errors:{string.Join(";", identityRes.ErrorDescrirtions())}");
            }
        }
        #endregion
    }
}
