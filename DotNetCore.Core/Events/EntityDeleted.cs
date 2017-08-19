using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Events
{
    public class EntityDeleted<T> where T : BaseEntity
    {
        public EntityDeleted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
