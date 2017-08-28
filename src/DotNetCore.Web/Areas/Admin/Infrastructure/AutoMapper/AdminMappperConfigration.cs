using System;
using AutoMapper;
using DotNetCore.Core.Interface;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Web.Areas.Admin.Models.Setting;

namespace DotNetCore.Web.Areas.Admin.Infrastructure.AutoMapper
{
    public class AdminMappperConfigration : IMapperConfiguration
    {
        public int Order => 5;

        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            Action<IMapperConfigurationExpression> action = x =>
            {
                x.CreateMap<AuthorizeSettings, AuthorizeSettingsModel>();
                x.CreateMap<AuthorizeSettingsModel, AuthorizeSettings>();
            };

            return action;
        }
    }
}
