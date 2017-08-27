using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Interface;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Linq;
using DotNetCore.Core;

namespace DotNetCore.Service.Accounts
{
    public class AccountService : IAccountService
    {
        private SignInManager<Account> _signManager;
        private UserManager<Account> _userManager;
        public AccountService(UserManager<Account> userManager,
            SignInManager<Account> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
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

        public IdentityResult Register(Account entitiy,string pwd)
        {
            if (entitiy == null)
                throw new ArgumentNullException("entitiy");
            var res = _userManager.CreateAsync(entitiy, pwd);
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

        public void Login(Account user,bool remeberMe)
        {
            _signManager.SignInAsync(user, remeberMe);
        }

        public void LoginOut()
        {
            _signManager.SignOutAsync();
        }
    }
}
