using DotNetCore.Core.Domain.UserInfos;
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
    }
}
