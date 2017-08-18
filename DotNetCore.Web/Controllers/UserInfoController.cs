using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Service.Infrastructure.Services;

namespace DotNetCore.Web.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly IUserInfoService _userInfoService;

        public UserInfoController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}