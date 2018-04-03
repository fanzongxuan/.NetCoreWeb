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
using DotNetCore.Service.Helpers;
using System.Net;
using DotNetCore.Web.Areas.Admin.Infrastructure.AutoMapper;
using DotNetCore.Web.Areas.Admin.Models.EmailAccounts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetCore.Web.Areas.Admin.Controllers
{
    public class AccountController : BaseAdminController
    {
        #region Fileds

        private readonly IAccountService _accountService;
        private readonly IPermissionService _permissionService;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region Ctor

        public AccountController(IAccountService accountService,
            IPermissionService permissionService,
            IDateTimeHelper dateTimeHelper
            )
        {
            _accountService = accountService;
            _permissionService = permissionService;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Uitites

        [NonAction]
        private AccountModel PrepareAccountModel(Account account)
        {
            var accountRoles = _accountService.GetRoleNamesByAccount(account);
            var allRoles = _accountService.SearchRoles().ToList();

            var model = account.ToModel();
            model.Roles = accountRoles;
            allRoles.ForEach(x =>
            {
                var selectList = new SelectListItem();
                if (accountRoles.Contains(x.Name))
                {
                    selectList.Text = x.Name;
                    selectList.Value = x.Id;
                    selectList.Selected = true;
                }
                else
                {
                    selectList.Text = x.Name;
                    selectList.Value = x.Id;
                }
                model.AvailaleRoles.Add(selectList);
            });

            return model;
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
                        CreateOn = _dateTimeHelper.ConvertToWebSiteTime(x.CreateOnUtc, DateTimeKind.Utc),
                        LastActivityDate = _dateTimeHelper.ConvertToWebSiteTime(x.LastActivityDateUtc, DateTimeKind.Utc),
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

            if (!_permissionService.Authorize(StandardPermissionProvider.MangageAccounts))
            {
                res = DefalutAjaxResultProvider.AccessDenied;
            }

            var account = _accountService.GetById(id);

            if (account == null)
            {
                res = DefalutAjaxResultProvider.NotFound;
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

        public IActionResult Update(string id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageAccounts))
                return AccessDeniedView();

            var account = _accountService.GetById(id);
            if (account == null)
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            var model = PrepareAccountModel(account);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(AccountModel model)
        {
            return View();
        }

        #endregion

        #endregion
    }
}