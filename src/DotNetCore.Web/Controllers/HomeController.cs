using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using DotNetCore.Framework.Mvc.Controllers;

namespace DotNetCore.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
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
