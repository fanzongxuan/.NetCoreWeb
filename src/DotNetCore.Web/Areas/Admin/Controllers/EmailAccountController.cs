using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Service.Messages;
using DotNetCore.Service.Security;
using DotNetCore.Service.Settings;
using DotNetCore.Framework.Attributes;
using DotNetCore.Framework.UI;
using DotNetCore.Web.Areas.Admin.Infrastructure.AutoMapper;
using DotNetCore.Core.Domain.Messages;
using System.Net;
using DotNetCore.Web.Areas.Admin.Models.EmailAccounts;
using DotNetCore.Core.Extensions;
using DotNetCore.Core;
using DotNetCore.Framework.Mvc.ActionFilter;

namespace DotNetCore.Web.Areas.Admin.Controllers
{
    public class EmailAccountController : BaseAdminController
    {
        #region Fileds

        private readonly IEmailAccountService _emailAccountService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IEmailSender _emailSender;
        private readonly EmailAccountSettings _emailAccountSettings;

        #endregion

        #region Ctor

        public EmailAccountController(IEmailAccountService emailAccountService,
            IPermissionService permissionService,
            ISettingService settingService,
            IEmailSender emailSender,
            EmailAccountSettings emailAccountSettings
            )
        {
            _emailAccountService = emailAccountService;
            _permissionService = permissionService;
            _settingService = settingService;
            _emailSender = emailSender;
            _emailAccountSettings = emailAccountSettings;
        }

        #endregion

        #region List
        public IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageEmailAccounts))
                return AccessDeniedView();

            return View();
        }

        [HttpPost]
        [AjaxRequest]
        public IActionResult List([FromBody]DataSourceRequest query)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageEmailAccounts))
                return AccessDeniedView();

            var emailAccounts = _emailAccountService
                                .GetListPageable(query.Page - 1, query.PageSize);

            var gridModel = new DataSourceResult
            {
                rows = emailAccounts.Select(x =>
                {
                    var model = x.ToModel();
                    model.IsDefaultEmailAccount = x.Id == _emailAccountSettings.DefaultEmailAccountId;
                    return model;
                }),
                total = emailAccounts.TotalCount
            };

            return Json(gridModel);
        }
        #endregion

        #region Update

        public IActionResult Update(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageEmailAccounts))
                return AccessDeniedView();

            var email = _emailAccountService.GetById(id);
            if (email == null)
                return new StatusCodeResult((int)HttpStatusCode.NotFound);

            var model = email.ToModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [FormValueRequired("save")]
        public IActionResult Update([FromForm]EmailAccountModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageEmailAccounts))
                return AccessDeniedView();

            var entity = _emailAccountService.GetById(model.Id);
            if (entity == null)
                return new StatusCodeResult((int)HttpStatusCode.NotFound);

            if (ModelState.IsValid)
            {
                entity = model.ToEntity(entity);
                _emailAccountService.Update(entity);

                SuccessNotification("Email Account has been updated!");
                return RedirectToAction("List");
            }
            else
            {
                ErrorNotification(string.Join(";", ModelState.Errors()));
                return View(model);
            }

        }
        #endregion

        #region Create

        public IActionResult Create(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageEmailAccounts))
                return AccessDeniedView();

            var model = new EmailAccountModel()
            {
                Port = 25
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm]EmailAccountModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageEmailAccounts))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                _emailAccountService.Insert(entity);

                SuccessNotification("Email Account has been create!");
                return RedirectToAction("List");
            }
            else
            {
                ErrorNotification(string.Join(";", ModelState.Errors()));
                return View(model);
            }

        }
        #endregion

        #region MarkAsDefaultEmail

        public IActionResult MarkAsDefaultEmail(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageEmailAccounts))
                return AccessDeniedView();

            var defaultEmailAccount = _emailAccountService.GetById(id);
            if (defaultEmailAccount == null)
                return new StatusCodeResult((int)HttpStatusCode.NotFound);

            _emailAccountSettings.DefaultEmailAccountId = defaultEmailAccount.Id;
            _settingService.SaveSetting(_emailAccountSettings);
            return RedirectToAction("List");
        }
        #endregion

        #region Delete

        [HttpPost]
        [AjaxRequest]
        public IActionResult Delete(int id)
        {
            var res = new AjaxResult();
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageEmailAccounts))
            {
                res = DefalutAjaxResultProvider.AccessDenied;
            }

            var emailAccount = _emailAccountService.GetById(id);
            if (emailAccount == null)
            {
                res = DefalutAjaxResultProvider.NotFound;
            }

            _emailAccountService.Delete(emailAccount);

            return Json(res);
        }

        #endregion

        #region SendTestEmail

        [HttpPost, ActionName("Update")]
        [FormValueRequired("sendtestemail")]
        public virtual ActionResult SendTestEmail(EmailAccountModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.MangageEmailAccounts))
                return AccessDeniedView();

            var emailAccount = _emailAccountService.GetById(model.Id);
            if (emailAccount == null)
                //No email account found with the specified id
                return RedirectToAction("List");

            if (!CommonHelper.IsValidEmail(model.SendTestEmailTo))
            {
                ErrorNotification("Wrong Email!", false);
                return View(model);
            }

            try
            {
                if (String.IsNullOrWhiteSpace(model.SendTestEmailTo))
                    throw new Exception("Enter test email address");

                string subject = "Testing email functionality.";
                string body = "Email works fine.";
                _emailSender.SendEmail(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName, model.SendTestEmailTo, null);
                SuccessNotification("Send test email success!", false);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message, false);
            }

            return View(model);
        }

        #endregion
    }
}