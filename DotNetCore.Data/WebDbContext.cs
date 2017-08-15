using System.Collections.Generic;
using DotNetCore.Core;
using DotNetCore.Data.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Infrastructure;
using System;

namespace DotNetCore.Data
{
    public class WebDbContext : DbContext, IDbContext
    {

        public WebDbContext(DbContextOptions<WebDbContext> options) : base(options)
        {

        }

        #region Utilities

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typeFinder = new AppDomainTypeFinder();
            var typesToRegister = typeFinder.FindClassesOfType(typeof(IEntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }

        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }

        #endregion

        public bool ProxyCreationEnabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool AutoDetectChangesEnabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Detach(object entity)
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
