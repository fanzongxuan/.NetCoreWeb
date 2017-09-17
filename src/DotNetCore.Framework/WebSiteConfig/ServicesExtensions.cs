using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Data;
using DotNetCore.Service.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DotNetCore.Service.ScheduleTasks;
using System;
using DotNetCore.Core.ElasticSearch;
using DotNetCore.Framework.Mvc.Config;

namespace DotNetCore.Framework.WebSiteConfig
{
    public static class ServicesExtensions
    {
        private static void ConfigAuthorize(IServiceCollection services)
        {
            var _settingService = EngineContext.Current.GetService<ISettingService>();
            var authorizeSettings = _settingService.LoadSetting<AuthorizeSettings>();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = authorizeSettings.RequirePasswordDigit;
                options.Password.RequiredLength = authorizeSettings.RequirePasswordLength;
                options.Password.RequireNonAlphanumeric = authorizeSettings.RequirePasswordNonAlphanumeric;
                options.Password.RequireUppercase = authorizeSettings.RequirePasswordUppercase;
                options.Password.RequireLowercase = authorizeSettings.RequirePasswordLowercase;
                options.Password.RequiredUniqueChars = authorizeSettings.RequiredPasswordUniqueChars;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = authorizeSettings.DefaultLockoutTimeSpan;
                options.Lockout.MaxFailedAccessAttempts = authorizeSettings.MaxFailedAccessAttempts;

                // User settings
                options.User.RequireUniqueEmail = authorizeSettings.RequireUniqueEmail;

                //sign in
                options.SignIn.RequireConfirmedEmail = authorizeSettings.RequireConfirmedEmail;
                options.SignIn.RequireConfirmedPhoneNumber = authorizeSettings.RequireConfirmedPhoneNumber;
            });
        }

        public static IServiceProvider ConfigMyWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            //default container config
            var conn = configuration.GetConnectionString("DotNetCoreWeb");
            services.AddDbContextPool<WebDbContext>(options => options.UseSqlServer(conn, t => t.UseRowNumberForPaging()));

            //config mvc
            services.AddMvc(options => { options.Config(); });

            //get ElasticSearch options
            services.Configure<ESOptions>(configuration.GetSection("ElasticSearch"));

            // add IdentityRole service
            services.AddIdentity<Account, AccountRole>()
            .AddEntityFrameworkStores<WebDbContext>()
            .AddDefaultTokenProviders();

            //cache
            var redisEnable = configuration.GetValue<bool>("Redis:Enable");
            if(redisEnable)
            {

                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = configuration.GetValue<string>("Redis:Configration");
                });

            }
            else
            {
                services.AddMemoryCache();
            }

            //web site services and some configs
            EngineContext.Initialize(services, configuration, false);

            //config  authorize
            ConfigAuthorize(services);
            
            //start schedule task
            if (!string.IsNullOrWhiteSpace(conn))
            {
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();
            }
            
            return EngineContext.Current.ServiceProvider;
        }
    }
}
