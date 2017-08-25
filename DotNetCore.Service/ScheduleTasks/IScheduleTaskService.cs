using DotNetCore.Core.Domain.ScheduleTasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.ScheduleTasks
{
    public interface IScheduleTaskService : IBaseService<Core.Domain.ScheduleTasks.ScheduleTask>
    {
        ScheduleTask GetTaskByType(string type);

        IList<Core.Domain.ScheduleTasks.ScheduleTask> GetAllTasks(bool showHidden = false);
    }
}
