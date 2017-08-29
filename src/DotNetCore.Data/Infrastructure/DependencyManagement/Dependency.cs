using DotNetCore.Core.Interface;
using System;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Data.Interface;
using DotNetCore.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using Autofac;

namespace DotNetCore.Data.Infrastructure.DependencyManagement
{
    public class Dependency : IDependency
    {
        public int Order => 0;

        public void Register(ContainerBuilder builder, AppDomainTypeFinder typeFinder, IConfiguration configuration)
        {
            builder.RegisterType<WebDbContext>().As<IDbContext>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
  }
    }
}
