using DotNetCore.Core.Interface;
using System;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Service.Infrastructure.Services;

namespace DotNetCore.Service.Infrastructure.DependencyManagement
{
    public class Dependency : IDependency
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IUserInfoService, UserInfoService>();
        }
    }
}
