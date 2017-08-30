using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Data;
using DotNetCore.Framework.Mvc.Config;
using DotNetCore.Framework.WebSiteConfig;
using DotNetCore.Service.ScheduleTasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DotNetCore.Framework.Infrastructure
{
    public class BaseTest
    {
        public BaseTest()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

            var config = builder.Build();

            IServiceCollection services = new ServiceCollection();
            // config services
            services.ConfigServices(config); // Create the container builder.
            
        }
    }
}
