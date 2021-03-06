﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using DotNetCore.Framework.WebSiteConfig;
using System;
using DotNetCore.Core.Infrastructure;
using Microsoft.Extensions.Logging;
using DotNetCore.Core.Logger;
using DotNetCore.Core.ElasticSearch;
using Microsoft.Extensions.Options;
using DotNetCore.Framework.Mvc.Config;

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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // config services
            return services.ConfigMyWebServices(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            //enable elastic search logger provier
            var opts = EngineContext.Current.GetService<IOptions<ESOptions>>();
            if (opts != null && opts.Value.Enable)
            {
                loggerFactory.AddESLogger(Configuration.GetSection("Logging"));
            }

            if (env.IsDevelopment())
                loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.Config();
            });

        }
    }
}
