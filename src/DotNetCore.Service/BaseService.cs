using DotNetCore.Core;
using DotNetCore.Core.Interface;
using DotNetCore.Service.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service
{
    public class BaseService<T> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;
        private readonly IEventPublisher _eventPublisher;

        public BaseService(IRepository<T> repository,
            IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        public T GetById(int id)
        {
            if (id == 0)
                return null;
            return _repository.GetById(id);
        }

        public IPagedList<T> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _repository.Table;
            return new PagedList<T>(query, pageIndex, pageSize);
        }

        public void Insert(T entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("entitiy");
            _repository.Insert(entitiy);
            _eventPublisher.EntityInserted(entitiy);
        }

        public void Update(T entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("entitiy");
            _repository.Update(entitiy);
            _eventPublisher.EntityUpdated(entitiy);
        }
    }
}
