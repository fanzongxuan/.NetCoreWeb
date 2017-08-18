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
using AutoMapper;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Web.Models.UserInfos;

namespace DotNetCore.Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
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
