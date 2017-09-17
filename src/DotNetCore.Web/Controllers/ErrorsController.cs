using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Framework.Mvc.Controllers;
using System.Net;

namespace DotNetCore.Web.Controllers
{
    public class ErrorsController : BaseController
    {
        [Route("errors/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            switch (statusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    return View("401");
                case (int)HttpStatusCode.Forbidden:
                    return View("403");
                case (int)HttpStatusCode.NotFound:
                    return View("404");
                default:
                    return View("errors");
            }
        }
    }
}