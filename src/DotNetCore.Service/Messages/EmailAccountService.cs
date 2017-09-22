using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.Messages;
using DotNetCore.Core.Interface;
using DotNetCore.Service.Events;
using DotNetCore.Core;
using System.Linq;

namespace DotNetCore.Service.Messages
{
    public class EmailAccountService : IEmailAccountService
    {
        #region Fileds

        private readonly IRepository<EmailAccount> _emailAccountRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public EmailAccountService(IRepository<EmailAccount> emailAccountRepository,
            IEventPublisher eventPublisher
            )
        {
            _emailAccountRepository = emailAccountRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        public void Delete(EmailAccount entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("emailAccount");

            if (GetAllEmailAccounts().Count == 1)
                throw new Exception("You cannot delete this email account. At least one account is required.");

            _emailAccountRepository.Delete(entitiy);

            //event notification
            _eventPublisher.EntityDeleted(entitiy);
        }

        public EmailAccount GetById(int id)
        {
            if (id == 0)
                return null;

            return _emailAccountRepository.GetById(id);
        }

        public IPagedList<EmailAccount> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _emailAccountRepository.Table;

            return new PagedList<EmailAccount>(query, pageIndex, pageSize);
        }

        public void Insert(EmailAccount entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("emailAccount");

            entitiy.Email = CommonHelper.EnsureNotNull(entitiy.Email);
            entitiy.DisplayName = CommonHelper.EnsureNotNull(entitiy.DisplayName);
            entitiy.Host = CommonHelper.EnsureNotNull(entitiy.Host);
            entitiy.Username = CommonHelper.EnsureNotNull(entitiy.Username);
            entitiy.Password = CommonHelper.EnsureNotNull(entitiy.Password);

            entitiy.Email = entitiy.Email.Trim();
            entitiy.DisplayName = entitiy.DisplayName.Trim();
            entitiy.Host = entitiy.Host.Trim();
            entitiy.Username = entitiy.Username.Trim();
            entitiy.Password = entitiy.Password.Trim();

            entitiy.Email = CommonHelper.EnsureMaximumLength(entitiy.Email, 255);
            entitiy.DisplayName = CommonHelper.EnsureMaximumLength(entitiy.DisplayName, 255);
            entitiy.Host = CommonHelper.EnsureMaximumLength(entitiy.Host, 255);
            entitiy.Username = CommonHelper.EnsureMaximumLength(entitiy.Username, 255);
            entitiy.Password = CommonHelper.EnsureMaximumLength(entitiy.Password, 255);

            _emailAccountRepository.Insert(entitiy);

            //event notification
            _eventPublisher.EntityInserted(entitiy);
        }

        public void Update(EmailAccount entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("emailAccount");

            entitiy.Email = CommonHelper.EnsureNotNull(entitiy.Email);
            entitiy.DisplayName = CommonHelper.EnsureNotNull(entitiy.DisplayName);
            entitiy.Host = CommonHelper.EnsureNotNull(entitiy.Host);
            entitiy.Username = CommonHelper.EnsureNotNull(entitiy.Username);
            entitiy.Password = CommonHelper.EnsureNotNull(entitiy.Password);

            entitiy.Email = entitiy.Email.Trim();
            entitiy.DisplayName = entitiy.DisplayName.Trim();
            entitiy.Host = entitiy.Host.Trim();
            entitiy.Username = entitiy.Username.Trim();
            entitiy.Password = entitiy.Password.Trim();

            entitiy.Email = CommonHelper.EnsureMaximumLength(entitiy.Email, 255);
            entitiy.DisplayName = CommonHelper.EnsureMaximumLength(entitiy.DisplayName, 255);
            entitiy.Host = CommonHelper.EnsureMaximumLength(entitiy.Host, 255);
            entitiy.Username = CommonHelper.EnsureMaximumLength(entitiy.Username, 255);
            entitiy.Password = CommonHelper.EnsureMaximumLength(entitiy.Password, 255);

            _emailAccountRepository.Update(entitiy);

            //event notification
            _eventPublisher.EntityUpdated(entitiy);
        }

        /// <summary>
        /// Gets all email accounts
        /// </summary>
        /// <returns>Email accounts list</returns>
        public virtual IList<EmailAccount> GetAllEmailAccounts()
        {
            var query = _emailAccountRepository.Table.Where(x => true).OrderBy(x => x.Id);
            var emailAccounts = query.ToList();
            return emailAccounts;
        }
    }
}
