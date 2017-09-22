using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.Messages;
using DotNetCore.Core.Interface;
using DotNetCore.Data.Interface;
using DotNetCore.Service.Events;
using System.Linq;
using DotNetCore.Core;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Service.Messages
{
    public class QueuedEmailService : IQueuedEmailService
    {
        #region Fileds

        private readonly IRepository<QueuedEmail> _queuedEmailRepository;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public QueuedEmailService(IRepository<QueuedEmail> queuedEmailRepository,
            IDbContext dbContext,
            IEventPublisher eventPublisher
            )
        {
            _queuedEmailRepository = queuedEmailRepository;
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
        }

        #endregion

        public void Delete(QueuedEmail entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("queuedEmail");

            _queuedEmailRepository.Delete(entitiy);

            //event notification
            _eventPublisher.EntityDeleted(entitiy);
        }

        public void DeleteAllEmails()
        {
            var queuedEmails = _queuedEmailRepository.Table.ToList();
            _queuedEmailRepository.Delete(queuedEmails);
        }

        public void DeleteQueuedEmails(IList<QueuedEmail> queuedEmails)
        {
            throw new NotImplementedException();
        }

        public QueuedEmail GetById(int id)
        {
            if (id == 0)
                return null;

            return _queuedEmailRepository.GetById(id);
        }

        public IPagedList<QueuedEmail> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _queuedEmailRepository.Table;
            return new PagedList<QueuedEmail>(query, pageIndex, pageSize);
        }

        public IList<QueuedEmail> GetQueuedEmailsByIds(int[] queuedEmailIds)
        {
            if (queuedEmailIds == null || queuedEmailIds.Length == 0)
                return new List<QueuedEmail>();

            var query = from qe in _queuedEmailRepository.Table
                        where queuedEmailIds.Contains(qe.Id)
                        select qe;
            var queuedEmails = query.ToList();
            //sort by passed identifiers
            var sortedQueuedEmails = new List<QueuedEmail>();
            foreach (int id in queuedEmailIds)
            {
                var queuedEmail = queuedEmails.Find(x => x.Id == id);
                if (queuedEmail != null)
                    sortedQueuedEmails.Add(queuedEmail);
            }
            return sortedQueuedEmails;
        }

        public void Insert(QueuedEmail entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("queuedEmail");

            _queuedEmailRepository.Insert(entitiy);

            //event notification
            _eventPublisher.EntityInserted(entitiy);
        }

        public IPagedList<QueuedEmail> SearchEmails(string fromEmail, string toEmail, DateTime? createdFromUtc, DateTime? createdToUtc, bool loadNotSentItemsOnly, bool loadOnlyItemsToBeSent, int maxSendTries, bool loadNewest, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            fromEmail = (fromEmail ?? String.Empty).Trim();
            toEmail = (toEmail ?? String.Empty).Trim();

            var query = _queuedEmailRepository.Table;
            if (!String.IsNullOrEmpty(fromEmail))
                query = query.Where(qe => qe.From.Contains(fromEmail));
            if (!String.IsNullOrEmpty(toEmail))
                query = query.Where(qe => qe.To.Contains(toEmail));
            if (createdFromUtc.HasValue)
                query = query.Where(qe => qe.CreatedOnUtc >= createdFromUtc);
            if (createdToUtc.HasValue)
                query = query.Where(qe => qe.CreatedOnUtc <= createdToUtc);
            if (loadNotSentItemsOnly)
                query = query.Where(qe => !qe.SentOnUtc.HasValue);
            if (loadOnlyItemsToBeSent)
            {
                var nowUtc = DateTime.UtcNow;
                query = query.Where(qe => !qe.DontSendBeforeDateUtc.HasValue || qe.DontSendBeforeDateUtc.Value <= nowUtc);
            }
            query = query.Where(qe => qe.SentTries < maxSendTries).Include(x=>x.EmailAccount);
            query = loadNewest ?
                //load the newest records
                query.OrderByDescending(qe => qe.CreatedOnUtc) :
                //load by priority
                query.OrderByDescending(qe => qe.Priority).ThenBy(qe => qe.CreatedOnUtc);

            var queuedEmails = new PagedList<QueuedEmail>(query, pageIndex, pageSize);
            return queuedEmails;
        }

        public void Update(QueuedEmail entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("queuedEmail");

            _queuedEmailRepository.Update(entitiy);

            //event notification
            _eventPublisher.EntityUpdated(entitiy);
        }
    }
}
