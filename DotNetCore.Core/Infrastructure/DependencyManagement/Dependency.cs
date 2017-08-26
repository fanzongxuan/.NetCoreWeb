using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Core.Cache;

namespace DotNetCore.Core.Infrastructure.DependencyManagement
{
    public class Dependency : IDependency
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<ICacheManager, MemoryCacheManager>();
        }
    }
}
