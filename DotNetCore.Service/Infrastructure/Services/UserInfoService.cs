using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Interface;

namespace DotNetCore.Service.Infrastructure.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IRepository<UserInfo> _addressRepository;

        public UserInfoService(IRepository<UserInfo> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public UserInfo GetById(int id)
        {
            return _addressRepository.GetById(id);
        }
    }
}
