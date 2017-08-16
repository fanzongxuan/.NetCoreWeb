using System.Collections.Generic;
using DotNetCore.Core;
using DotNetCore.Data.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Infrastructure;
using System;
using System.Data.Common;
using System.Data;

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
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                Set<TEntity>().Attach(entity);
                return entity;
            }
            
            return alreadyAttached;
        }

        #endregion

        public bool AutoDetectChangesEnabled
        {
            get
            {
                return this.ChangeTracker.AutoDetectChangesEnabled;
            }
            set
            {
                ChangeTracker.AutoDetectChangesEnabled = value;
            }
        }
        
        public int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = this.Database.GetCommandTimeout();
                this.Database.SetCommandTimeout(timeout);
            }
            
            var result = this.Database.ExecuteSqlCommand(sql, parameters);

            if (previousTimeout.HasValue)
            {
                //set previous timeout back
                this.Database.SetCommandTimeout(previousTimeout);
            }
            
            return result;
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }
            
            var result = this.SqlQuery<TEntity>(commandText, parameters).ToList();
            
            bool acd = this.ChangeTracker.AutoDetectChangesEnabled;
            try
            {
                this.ChangeTracker.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.ChangeTracker.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters) where TElement : class
        {
           return Set<TElement>().FromSql(sql, parameters);
        }
    }
}
