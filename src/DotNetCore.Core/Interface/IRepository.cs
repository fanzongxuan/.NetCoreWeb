using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCore.Core.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 根据主键获取entity
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity);

        /// <summary>
        /// 插入实体（批量）
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// 更新实体（批量）
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// 获去表
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// 获取表（只读，数据库上下文不追踪）
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
