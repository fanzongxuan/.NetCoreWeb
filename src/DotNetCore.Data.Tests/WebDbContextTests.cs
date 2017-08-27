using DotNetCore.Data.Interface;
using DotNetCore.Framework.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Infrastructure;

namespace DotNetCore.Data.Tests
{
    [TestClass]
    public class WebDbContextTests : BaseTest
    {
        [TestMethod]
        public void ExecuteSqlCommandTest()
        {
            var _dbContext= EngineContext.Current.GetService<IDbContext>();
            string sql = "update userinfo set isdeleted=0";
            var res = _dbContext.ExecuteSqlCommand(sql);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SqlQueryTest()
        {
            var _dbContext = EngineContext.Current.GetService<IDbContext>();
            var sql = "select *from userinfo";
            var res= _dbContext.SqlQuery<UserInfo>(sql);
            Assert.IsTrue(true);
        }
    }
}
