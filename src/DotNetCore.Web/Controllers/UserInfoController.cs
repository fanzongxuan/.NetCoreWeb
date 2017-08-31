using AutoMapper;
using DotNetCore.Core.Cache;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Service.UserInfoService;
using DotNetCore.Web.Models.UserInfos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetCore.Web.Controllers
{
    [Authorize]
    public class UserInfoController : Controller
    {
        private readonly IUserinfoService _userInfoService;
        private readonly ICacheManager _cacheManager;

        public UserInfoController(IUserinfoService userInfoService,
            ICacheManager cacheManager)
        {
            _userInfoService = userInfoService;
            _cacheManager = cacheManager;
        }

        public IActionResult Index()
        {
            var entities = _userInfoService.GetListPageable();
            return Json(entities);
        }

        public IActionResult Insert()
        {
            throw new System.Exception("报错啦！");
            var entity = new UserInfo()
            {
                LoginName = "test",
                Password = "123456",
                RealName = "我是你鸭哥",
                LastLoginIpAddress = "192.168.0.0",
                Sex = Sex.Man,
            };
            _userInfoService.Insert(entity);
            var userinfo = _userInfoService.GetById(entity.Id);
            var userinfoFromCach = _userInfoService.GetById(entity.Id);
            
            userinfo.RealName = "6666";
            _userInfoService.Update(userinfo);
            _userInfoService.Delete(userinfo);
            return View();
        }

    }
}