using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Events
{
    public class EntityUpdated<T> where T : BaseEntity
    {
        public EntityUpdated(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
