using DotNetCore.Data.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Data
{
    public static class DbInitializer
    {
        public static void InitializeDatabase(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var dbContext = provider.GetService<IDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
