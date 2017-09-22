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
    }
}