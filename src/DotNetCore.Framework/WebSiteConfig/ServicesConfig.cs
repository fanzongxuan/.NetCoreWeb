using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Data;
using DotNetCore.Framework.Mvc.Config;
using DotNetCore.Service.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DotNetCore.Service.ScheduleTasks;

namespace DotNetCore.Framework.WebSiteConfig
{
    public static class ServicesConfig
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

        public static void ConfigServices(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DotNetCoreWeb");
            services.AddDbContextPool<WebDbContext>(options => options.UseSqlServer(conn, t => t.UseRowNumberForPaging()));

            services.AddMvc(options => { options.Config(); });

            services.AddIdentity<Account, IdentityRole>()
            .AddEntityFrameworkStores<WebDbContext>()
            .AddDefaultTokenProviders();

            //cach
            var redisEnable = configuration.GetValue<bool>("Redis:Enable");
            if(redisEnable)
            {

                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = configuration.GetValue<string>("Redis:Configration");
                    options.InstanceName = configuration.GetValue<string>("Redis:Instance");
                });

            }
            else
            {
                services.AddMemoryCache();
            }

            //web site services and some configs
            EngineContext.Initialize(services, configuration,false);

            ConfigAuthorize(services);

            //start schedule task
            if (!string.IsNullOrWhiteSpace(conn))
            {
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();
            }
        }
    }
}
