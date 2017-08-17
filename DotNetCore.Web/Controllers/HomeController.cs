using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Web.Models;
using DotNetCore.Core.Interface;
using DotNetCore.Core.Domain.UserInfos;
using Microsoft.EntityFrameworkCore;
using DotNetCore.Service.Infrastructure.Services;
using DotNetCore.Data.Interface;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserInfoService _userInfoService;
        private readonly IDbContext _dbContext;
        private readonly ILogger _logger;

        public HomeController(IUserInfoService userInfoService,
            IDbContext dbContext,
            ILogger<HomeController> logger)
        {
            _userInfoService = userInfoService;
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            throw new Exception("error!");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
