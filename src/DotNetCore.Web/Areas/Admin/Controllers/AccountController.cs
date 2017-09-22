using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Service.Accounts;
using DotNetCore.Web.Areas.Admin.Models.Accounts;
using DotNetCore.Framework.UI;
using DotNetCore.Core.Extensions;
using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Service.Security;
using DotNetCore.Framework.Attributes;

namespace DotNetCore.Web.Areas.Admin.Controllers
{
    public class AccountController : BaseAdminController
    {
        #region Fileds

        private readonly IAccountService _accountService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public AccountController(IAccountService accountService,
            IPermissionService permissionService
            )
        {
            _accountService = accountService;
            _permissionService = permissionService;
        }

        #endregion

        #region Methods

        #region List
        public IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageAccounts))
                return AccessDeniedView();

            return View();
        }

        [HttpPost]
        [AjaxRequest]
        public IActionResult GetListJson([FromBody] AccountQuery query)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageAccounts))
                return AccessDeniedView();

            var accounts = _accountService.QueryPageable(query.UserName, query.Email, query.Keywords, query.Page - 1, query.PageSize);

            var gridModel = new DataSourceResult
            {
                rows = accounts.Select(x =>
                {
                    return new
                    {
                        Id = x.Id,
                        Email = x.Email,
                        UserName = x.UserName,
                        PhoneNumber = x.PhoneNumber,
                        CreateOn = x.CreateOnUtc,
                        LastActivityDate = x.LastActivityDateUtc,
                        Roles = string.Join("|", _accountService.GetRoleNamesByAccount(x))
                    };
                }),
                total = accounts.TotalCount
            };

            return Json(gridModel);
        }
        #endregion

        #region Delete

        [AjaxRequest]
        public IActionResult Delete(string id)
        {
            var res = new AjaxResult();

            //access
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageAccounts))
            {
                res = DefalutAjaxResultProvider.AccessDenied;
                return Json(res);
            }

            var account = _accountService.GetById(id);

            if (account == null)
            {
                res.Message = "account not exsit!";
                res.Code = ReturnCode.Error;
            }

            var rolesCantBeDelete = new List<string>();
            rolesCantBeDelete.Add(AccountRoleNames.Administrators);
            rolesCantBeDelete.Add(AccountRoleNames.SystemRole);

            if (_accountService.IsInAnyRole(account, rolesCantBeDelete))
            {
                res.Code = ReturnCode.Error;
                res.Message = string.Join(';', "account contain the role which can't be deleted");
            }
            else
            {
                var identityRes = _accountService.Delete(account);
                if (identityRes.Succeeded)
                {
                    res = DefalutAjaxResultProvider.Success;
                }
                else
                {
                    res.Code = ReturnCode.Error;
                    res.Message = string.Join(';', identityRes.ErrorDescrirtions());
                }
            }

            return Json(res);
        }

        #endregion

        #region Update

        #endregion

        #endregion
    }
}