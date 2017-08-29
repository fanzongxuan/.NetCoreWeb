using Autofac;
using DotNetCore.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Interface
{
    public interface IDependency
    {
        void Register(ContainerBuilder builder, AppDomainTypeFinder typeFinder, IConfiguration configuration);

        int Order { get; }
    }
}
