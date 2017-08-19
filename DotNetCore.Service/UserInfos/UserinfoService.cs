using DotNetCore.Service.UserInfoService;
using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Interface;
using DotNetCore.Core;
using DotNetCore.Service.Events;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DotNetCore.Service.UserInfoService
{
    public class UserinfoService : IUserinfoService
    {
        protected readonly IRepository<UserInfo> _userinfoRepository;
        private readonly IEventPublisher _eventPublisher;

        public UserinfoService(IRepository<UserInfo> userinfoRepository,
            IEventPublisher eventPublisher
            )
        {
            _userinfoRepository = userinfoRepository;
            _eventPublisher = eventPublisher;
        }

        public void Delete(UserInfo entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("UserInfo");
            entitiy.IsDeleted = true;
            _userinfoRepository.Update(entitiy);
            _eventPublisher.EntityDeleted(entitiy);
        }

        public UserInfo GetById(int id)
        {
            if (id == 0)
                return null;
            return _userinfoRepository.GetById(id);
        }

        public IPagedList<UserInfo> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _userinfoRepository.Table.Include(x=>x.Addresses).ToList();
            //TODO 分页有问题
            return new PagedList<UserInfo>(query, pageIndex, pageSize);
        }

        public void Insert(UserInfo entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("UserInfo");
            _userinfoRepository.Insert(entitiy);
            _eventPublisher.EntityInserted(entitiy);
        }

        public void Update(UserInfo entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("entitiy");
            _userinfoRepository.Update(entitiy);
            _eventPublisher.EntityUpdated(entitiy);
        }
    }
}
