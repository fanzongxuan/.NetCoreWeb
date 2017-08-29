using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Data;
using DotNetCore.Framework.Mvc.Config;
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
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var config = builder.Build();

            IServiceCollection services = new ServiceCollection();
            var conn = "Data Source=.;Initial Catalog=DotNetCoreWeb;Integrated Security=True;Persist Security Info=False;MultipleActiveResultSets=True";
            services.AddDbContextPool<WebDbContext>(options => options.UseSqlServer(conn));
            services.AddMemoryCache();
            services.AddIdentity<Account, IdentityRole>()
            .AddEntityFrameworkStores<WebDbContext>()
            .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = false;
            });
            services.AddMvc(options => { options.Config(); });
            EngineContext.Initialize(services,config, false);
            //start schedule task
            if (!string.IsNullOrWhiteSpace(conn))
            {
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();
            }
        }
    }
}
