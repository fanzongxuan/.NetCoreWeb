using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using DotNetCore.Core.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace DotNetCore.Framework.Infrastructure
{
    public class Dependency : IDependency
    {
        public int Order => 5;

        public void Register(ContainerBuilder builder, AppDomainTypeFinder typeFinder, IConfiguration configuration)
        {
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
        }
    }
}
