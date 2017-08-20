using DotNetCore.Core.Infrastructure;
using DotNetCore.Data;
using DotNetCore.Framework.Mvc.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNetCore.Framework.Infrastructure
{
    public class BaseTest
    {
        public BaseTest()
        {
            IServiceCollection services = new ServiceCollection();
            var conn = "Data Source=.;Initial Catalog=DotNetCoreWeb;Integrated Security=True;Persist Security Info=False;MultipleActiveResultSets=True";
            services.AddDbContextPool<WebDbContext>(options => options.UseSqlServer(conn));
            services.AddMvc(options => { options.Config(); });
            EngineContext.Initialize(services, false);
        }
    }
}
