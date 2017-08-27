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
using DotNetCore.Core.Cache;

namespace DotNetCore.Service.UserInfoService
{
    public class UserinfoService : IUserinfoService
    {
        private const string USERINFO_BY_ID_KEY = "DotNetWeb.userinfo.id-{0}";

        private readonly ICacheManager _cacheManager;
        protected readonly IRepository<UserInfo> _userinfoRepository;
        private readonly IEventPublisher _eventPublisher;

        public UserinfoService(IRepository<UserInfo> userinfoRepository,
            IEventPublisher eventPublisher,
            ICacheManager cacheManager
            )
        {
            _userinfoRepository = userinfoRepository;
            _eventPublisher = eventPublisher;
            _cacheManager = cacheManager;
        }

        public void Delete(UserInfo entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("UserInfo");
            entitiy.IsDeleted = true;
            _userinfoRepository.Update(entitiy);

            string key = string.Format(USERINFO_BY_ID_KEY, entitiy.Id);
            _cacheManager.Remove(key);
            _eventPublisher.EntityDeleted(entitiy);
        }

        public UserInfo GetById(int id)
        {
            if (id == 0)
                return null;

            string key = string.Format(USERINFO_BY_ID_KEY, id);
            return _cacheManager.Get(key, () => _userinfoRepository.GetById(id));
        }

        public IPagedList<UserInfo> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _userinfoRepository.Table.Include(x => x.Addresses).OrderByDescending(x => x.CreateTime);

            return new PagedList<UserInfo>(query, pageIndex, pageSize);
        }

        public void Insert(UserInfo entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("UserInfo");
            _userinfoRepository.Insert(entitiy);

            //remove cache
            string key = string.Format(USERINFO_BY_ID_KEY, entitiy.Id);
            _cacheManager.Remove(key);

            //event publish
            _eventPublisher.EntityInserted(entitiy);
        }

        public void Update(UserInfo entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("entitiy");
            _userinfoRepository.Update(entitiy);

            //remove cache
            string key = string.Format(USERINFO_BY_ID_KEY, entitiy.Id);
            _cacheManager.Remove(key);

            //event publish
            _eventPublisher.EntityUpdated(entitiy);
        }
    }
}
