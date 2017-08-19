using AutoMapper;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Service.UserInfoService;
using DotNetCore.Web.Models.UserInfos;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Web.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly IUserinfoService _userInfoService;

        public UserInfoController(IUserinfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        public IActionResult Index()
        {
            var models = _userInfoService.GetListPageable();
            return Json(new { code = 1, mes = "Success" });
        }

        public IActionResult Insert()
        {
            var model = new UserInfoModel()
            {
                LoginName="test",
                Password="123456",
                RealName="我是你鸭哥",
                PhoneNumber="13814063516",
                Email="1027300882@qq.com",
                LastLoginIpAddress="192.168.0.0",
                Sex= Sex.Man,
            };
            var entity = AutoMapperConfiguration.Mapper.Map<UserInfoModel, UserInfo>(model);
            _userInfoService.Insert(entity);
            _userInfoService.Delete(entity);
            return Json(new { code = 1, mes = "Success" });
        }
    }
}