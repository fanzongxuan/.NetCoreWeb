using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Framework.Infrastructure;
using DotNetCore.Service.UserInfoService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Tests.UserInfos
{
    [TestClass]
    public class UserInfoServiceTest:BaseTest
    {
        [TestMethod]
        public void CURDUserInfoTest()
        {
            var userinfoService = EngineContext.Current.GetService<IUserinfoService>();
            var entity = new UserInfo()
            {
                LoginName = "test",
                Password = "123456",
                RealName = "我是你鸭哥",
                LastLoginIpAddress = "192.168.0.0",
                Sex = Sex.Man,
            };
            userinfoService.Insert(entity);
            var userinfo= userinfoService.GetById(entity.Id);
            var userinfoFromCach = userinfoService.GetById(entity.Id);
            userinfo.RealName = "6666";
            userinfoService.Update(userinfo);
            userinfoService.Delete(userinfo);
        }
    }
}
