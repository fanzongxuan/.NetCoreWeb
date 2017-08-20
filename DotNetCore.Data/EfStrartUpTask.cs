using DotNetCore.Core.Infrastructure;
using DotNetCore.Core.Interface;
using DotNetCore.Data.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Data
{
    public class EfStrartUpTask : IStartupTask
    {
        public int Order => 1;

        public void Execute()
        {
            var dbContext = EngineContext.Current.GetService<IDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
