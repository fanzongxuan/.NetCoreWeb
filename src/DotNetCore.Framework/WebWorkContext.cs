using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.Accounts;
using Microsoft.AspNetCore.Http;
using DotNetCore.Service.Accounts;
using System.Net;

namespace DotNetCore.Framework
{
    public class WebWorkContext : IWorkContext
    {
        #region Const

        private const string CustomerCookieName = "Web.Account";
        #endregion

        #region Fileds

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _accountService;
        private Account _account;
        #endregion

        #region Ctor

        public WebWorkContext(IHttpContextAccessor httpContextAccessor,
            IAccountService accountService)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
        }
        #endregion

        #region Uitites

        protected virtual string GetAccountCookie()
        {
            string cookieValue = "";
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Request == null)
                return null;

            _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CustomerCookieName, out cookieValue);
            return cookieValue;
        }

        protected virtual void SetCustomerCookie(string accounId)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext.Response != null)
            {
                CookieOptions option = new CookieOptions();
                option.HttpOnly = true;

                if (string.IsNullOrEmpty(accounId))
                {
                    option.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    option.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContextAccessor.HttpContext.Response.Cookies.Delete(CustomerCookieName);
                _httpContextAccessor.HttpContext.Response.Cookies.Append(CustomerCookieName, accounId, option);
            }
        }
        #endregion

        #region Prop

        public Account CurrentAccount
        {
            get
            {
                if (_account != null)
                    return _account;

                Account account = null;

                if (_httpContextAccessor.HttpContext == null)
                    account = _accountService.GetByName(SystemAccountNames.BackgroundTask);

                if (account == null)
                    account = _accountService.GetAuthenticationAccount();

                if (account == null)
                {
                    var accountCookieValue = GetAccountCookie();
                    if (!String.IsNullOrEmpty(accountCookieValue))
                    {
                        var accountByCookie = _accountService.GetById(accountCookieValue);
                        if (accountByCookie != null)
                        {
                            if (!_accountService.IsInRole(accountByCookie, AccountRoleNames.Register))
                                account = accountByCookie;
                        }
                    }
                }

                if (account == null)
                {
                    account = _accountService.InsertGuestAccount();
                }

                SetCustomerCookie(account.Id);
                _account = account;
                _account.LastActivityDateUtc = DateTime.UtcNow;
                _accountService.Update(_account);

                return _account;
            }
            set
            {
                SetCustomerCookie(value.Id);
                _account = value;
            }
        }
        #endregion

    }
}
