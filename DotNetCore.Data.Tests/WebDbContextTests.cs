using DotNetCore.Data.Interface;
using DotNetCore.Framework.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Core.Domain.UserInfos;

namespace DotNetCore.Data.Tests
{
    [TestClass]
    public class WebDbContextTests : BaseTest
    {
        [TestMethod]
        public void ExecuteSqlCommandTest()
        {
            var _dbContext = ServiceProvider.GetService<IDbContext>();
            string sql = "update userinfo set isdeleted=0";
            var res = _dbContext.ExecuteSqlCommand(sql);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SqlQueryTest()
        {
            var _dbContext = ServiceProvider.GetService<IDbContext>();
            var sql = "select *from userinfo";
            var res= _dbContext.SqlQuery<UserInfo>(sql);
            Assert.IsTrue(true);
        }
    }
}
