using AutoMapper;
using System;

namespace DotNetCore.Core.Interface
{
    public interface IMapperConfiguration
    {
        Action<IMapperConfigurationExpression> GetConfiguration();

        int Order { get; }
    }
}
