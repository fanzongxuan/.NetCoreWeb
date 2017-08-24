using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Data;
using Microsoft.EntityFrameworkCore;
using DotNetCore.Framework.Mvc.Config;
using DotNetCore.Core.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using DotNetCore.Core.Domain.UserInfos;

namespace DotNetCore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //default service
            var conn = Configuration.GetConnectionString("DotNetCoreWeb");
            //sqlserver version below 2012,don't support 'Featch Next'
            services.AddDbContextPool<WebDbContext>(options => options.UseSqlServer(conn, t => t.UseRowNumberForPaging()));
            services.AddMvc(options => { options.Config(); });

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

            //web site service and some config
            EngineContext.Initialize(services, false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.Config();
            });

        }
    }
}
