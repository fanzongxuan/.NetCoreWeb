using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Web.Models.UserInfos;

namespace DotNetCore.Web.Infrastructure.AutoMapper
{
    public class WebMapperConfigration : IMapperConfiguration
    {
        public int Order => 10;

        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            Action<IMapperConfigurationExpression> action = x =>
            {
                x.CreateMap<UserInfo, UserInfoModel>();
                x.CreateMap<Address, AddressModel>();
                x.CreateMap<UserInfoModel, UserInfo>();
                x.CreateMap<AddressModel, Address>();
            };

            return action;
        }
    }
}
