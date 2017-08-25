using DotNetCore.Core.Domain.ScheduleTasks;
using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCore.Service.Installation
{
    public class CodeFirstInstallationService : IInstallationService
    {
        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;

        public CodeFirstInstallationService(IRepository<ScheduleTask> scheduleTaskRepository)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
        }

        protected virtual void InstallScheduleTasks()
        {
            var tasks = new List<ScheduleTask>();
            var keepAliveTask = new ScheduleTask
            {
                Name = "Keep alive",
                Seconds = 300,
                Type = "DotNetCore.Service.Common.KeepAliveTask, DotNetCore.Service",
                Enabled = true,
                StopOnError = false,
            };
            if (!_scheduleTaskRepository.Table.Where(x => x.Type == keepAliveTask.Type).Any())
            {
                tasks.Add(keepAliveTask);
            }

            _scheduleTaskRepository.Insert(tasks);
        }


        public void InstallData()
        {
            InstallScheduleTasks();
        }

    }
}
