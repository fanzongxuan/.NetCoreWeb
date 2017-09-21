using DotNetCore.Core.Interface;
using System;
using AutoMapper;

namespace DotNetCore.Web.Infrastructure.AutoMapper
{
    public class WebMapperConfigration : IMapperConfiguration
    {
        public int Order => 10;

        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            Action<IMapperConfigurationExpression> action = x =>
            {
            };

            return action;
        }
    }
}
