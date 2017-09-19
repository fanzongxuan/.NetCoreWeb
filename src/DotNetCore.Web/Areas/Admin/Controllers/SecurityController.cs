using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCore.Core.Interface;

namespace DotNetCore.Web.Areas.Admin.Controllers
{
    public class SecurityController : BaseAdminController
    {
        #region Fileds

        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public SecurityController(ILogger<SecurityController> logger,
            IWorkContext workContext
            )
        {
            _logger = logger;
            _workContext = workContext;
        }
        #endregion

        #region Methods
        
        public IActionResult AccessDenied(string pageUrl)
        {
            var currentAccount = _workContext.CurrentAccount;
            _logger.LogInformation(string.Format($"Access denied to user #{currentAccount.Email} '{currentAccount.Email}' on {pageUrl}"));

            return View();
        }
        #endregion
    }
}