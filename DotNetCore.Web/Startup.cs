using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Data;
using Microsoft.EntityFrameworkCore;
using DotNetCore.Framework.Mvc.Config;
using DotNetCore.Core.Infrastructure;

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
            services.AddDbContextPool<WebDbContext>(options => options.UseSqlServer(conn));
            services.AddMvc(options => { options.Config(); });
            
            //web site service and some config
            EngineContext.Initialize(services, false);

            //database Migrate
            services.InitializeDatabase();
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
