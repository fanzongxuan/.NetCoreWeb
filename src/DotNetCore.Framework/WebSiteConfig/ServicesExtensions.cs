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
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DotNetCore.Framework.WebSiteConfig
{
    public static class ServicesExtensions
    {
        private static void ConfigAuthorize(IServiceCollection services)
        {
            var _settingService = EngineContext.Current.GetService<ISettingService>();
            var authorizeSettings = _settingService.LoadSetting<AuthorizeSettings>();
            var identityOpt = EngineContext.Current.GetService<IOptions<IdentityOptions>>();

            // Password settings
            identityOpt.Value.Password.RequireDigit = authorizeSettings.RequirePasswordDigit;
            identityOpt.Value.Password.RequiredLength = authorizeSettings.RequirePasswordLength;
            identityOpt.Value.Password.RequireNonAlphanumeric = authorizeSettings.RequirePasswordNonAlphanumeric;
            identityOpt.Value.Password.RequireUppercase = authorizeSettings.RequirePasswordUppercase;
            identityOpt.Value.Password.RequireLowercase = authorizeSettings.RequirePasswordLowercase;
            identityOpt.Value.Password.RequiredUniqueChars = authorizeSettings.RequiredPasswordUniqueChars;

            // Lockout settings
            identityOpt.Value.Lockout.DefaultLockoutTimeSpan = authorizeSettings.DefaultLockoutTimeSpan;
            identityOpt.Value.Lockout.MaxFailedAccessAttempts = authorizeSettings.MaxFailedAccessAttempts;

            // User settings
            identityOpt.Value.User.RequireUniqueEmail = authorizeSettings.RequireUniqueEmail;

            //sign in
            identityOpt.Value.SignIn.RequireConfirmedEmail = authorizeSettings.RequireConfirmedEmail;
            identityOpt.Value.SignIn.RequireConfirmedPhoneNumber = authorizeSettings.RequireConfirmedPhoneNumber;
        }

        public static IServiceProvider ConfigMyWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Due to identity config,add setting service to default service collection
            services.AddScoped<ISettingService, SettingService>();

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
            if (redisEnable)
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

            //web site services and some configs,
            //Due to use autofac,so can't use defalut service collection any more
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
