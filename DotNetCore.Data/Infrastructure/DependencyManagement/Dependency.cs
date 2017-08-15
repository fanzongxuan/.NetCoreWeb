using DotNetCore.Core.Interface;
using System;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Data.Interface;

namespace DotNetCore.Data.Infrastructure.DependencyManagement
{
    public class Dependency : IDependency
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IDbContext, WebDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }
    }
}
