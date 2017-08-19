using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Events
{
    public class EntityInserted<T> where T : BaseEntity
    {
        public EntityInserted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
