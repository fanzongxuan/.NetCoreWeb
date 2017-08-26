using DotNetCore.Core.Domain.ScheduleTasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.ScheduleTasks
{
    public interface IScheduleTaskService : IBaseService<ScheduleTask>
    {
        ScheduleTask GetTaskByType(string type);

        IList<ScheduleTask> GetAllTasks(bool showHidden = false);
    }
}
