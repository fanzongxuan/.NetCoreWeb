using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using DotNetCore.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using DotNetCore.Web.Controllers;
using Autofac.Core;
using DotNetCore.Core.Cache;

namespace DotNetCore.Web.Infrastructure.DependencyManangement
{
    public class DependencyRegistrar : IDependency
    {
        public int Order => 10;

        public void Register(ContainerBuilder builder, AppDomainTypeFinder typeFinder, IConfiguration configuration)
        {
            builder.RegisterType<UserInfoController>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("cache_static"));
        }
    }
}
