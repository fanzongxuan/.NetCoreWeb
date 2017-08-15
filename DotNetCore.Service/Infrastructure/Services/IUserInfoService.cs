using DotNetCore.Core.Domain.UserInfos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Infrastructure.Services
{
    public interface IUserInfoService
    {
        UserInfo GetById(int id);
    }
}
