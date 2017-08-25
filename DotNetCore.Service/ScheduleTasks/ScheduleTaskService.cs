using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.ScheduleTasks;
using DotNetCore.Core.Interface;
using System.Linq;

namespace DotNetCore.Service.ScheduleTasks
{
    public class ScheduleTaskService : IScheduleTaskService
    {

        private readonly IRepository<ScheduleTask> _taskRepository;

        public ScheduleTaskService(IRepository<ScheduleTask> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public void Delete(ScheduleTask entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("task");

            _taskRepository.Delete(entitiy);
        }

        public IList<ScheduleTask> GetAllTasks(bool showHidden = false)
        {
            var query = _taskRepository.Table;
            if (!showHidden)
            {
                query = query.Where(t => t.Enabled);
            }
            query = query.OrderByDescending(t => t.Seconds);

            var tasks = query.ToList();
            return tasks;
        }

        public ScheduleTask GetById(int id)
        {
            if (id == 0)
                return null;

            return _taskRepository.GetById(id);
        }

        public IPagedList<ScheduleTask> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public ScheduleTask GetTaskByType(string type)
        {
            if (String.IsNullOrWhiteSpace(type))
                return null;

            var query = _taskRepository.Table;
            query = query.Where(st => st.Type == type);
            query = query.OrderByDescending(t => t.Id);

            var task = query.FirstOrDefault();
            return task;
        }

        public void Insert(ScheduleTask entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("task");

            _taskRepository.Insert(entitiy);
        }

        public void Update(ScheduleTask entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("task");

            _taskRepository.Update(entitiy);
        }
    }
}
