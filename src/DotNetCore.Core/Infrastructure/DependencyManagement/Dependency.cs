using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Core.Cache;
using Microsoft.Extensions.Configuration;
using Autofac;
using Autofac.Core;

namespace DotNetCore.Core.Infrastructure.DependencyManagement
{
    public class Dependency : IDependency
    {
        public int Order => 0;

        public void Register(ContainerBuilder builder, AppDomainTypeFinder typeFinder, IConfiguration configuration)
        {
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("cache_per_request");

            var redisEnable = configuration.GetValue<bool>("Redis:Enable");
            if (redisEnable)
            {
                builder.RegisterType<DistributeCacheManager>().As<ICacheManager>().Named<ICacheManager>("cache_static").WithParameter(ResolvedParameter.ForNamed<ICacheManager>("cache_per_request"));
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("cache_static");
            }
        }
    }
}
