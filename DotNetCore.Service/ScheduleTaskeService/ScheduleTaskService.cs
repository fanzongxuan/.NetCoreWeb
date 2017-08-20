using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.ScheduleTasks;
using DotNetCore.Core.Interface;

namespace DotNetCore.Service.ScheduleTaskeService
{
    public class ScheduleTaskService : IScheduleTaskService
    {

        public ScheduleTaskService()
        {

        }

        public void Delete(ScheduleTask entitiy)
        {
            throw new NotImplementedException();
        }

        public IList<ScheduleTask> GetAllTasks(bool showHidden = false)
        {
            throw new NotImplementedException();
        }

        public ScheduleTask GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IPagedList<ScheduleTask> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public ScheduleTask GetTaskByType(string type)
        {
            throw new NotImplementedException();
        }

        public void Insert(ScheduleTask entitiy)
        {
            throw new NotImplementedException();
        }

        public void Update(ScheduleTask entitiy)
        {
            throw new NotImplementedException();
        }
    }
}
