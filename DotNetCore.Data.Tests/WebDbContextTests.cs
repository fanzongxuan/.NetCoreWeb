using DotNetCore.Data.Interface;
using DotNetCore.Framework.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.Data.Tests
{
    [TestClass]
    public class WebDbContextTests : BaseTest
    {
        [TestMethod]
        public void ExecuteSqlCommandTest()
        {
            var dbContext = ServiceProvider.GetService<IDbContext>();
            string sql = "update userinfo set isdeleted=0";
            var res= dbContext.ExecuteSqlCommand(sql);
            Assert.IsTrue(true);
        }
    }
}
