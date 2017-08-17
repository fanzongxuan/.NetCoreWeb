using DotNetCore.Core.Extensions;
using DotNetCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNetCore.Framework.Infrastructure
{
    public class BaseTest
    {
        public IServiceProvider ServiceProvider;
        public BaseTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddAllServices();
            var conn = "Data Source=.;Initial Catalog=DotNetCoreWeb;Integrated Security=True;Persist Security Info=False;MultipleActiveResultSets=True";
            services.AddDbContextPool<WebDbContext>(options => options.UseSqlServer(conn));
            var serviceProvider = services.BuildServiceProvider();
            ServiceProvider = serviceProvider;
        }
    }
}
