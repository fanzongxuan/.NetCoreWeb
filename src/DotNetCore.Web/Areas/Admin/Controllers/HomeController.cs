using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Core.Interface;

namespace DotNetCore.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        private readonly IWorkContext _workContext;

        public HomeController(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        public IActionResult Index()
        {
            var currentAccount= _workContext.CurrentAccount;
            return View();
        }
    }
}