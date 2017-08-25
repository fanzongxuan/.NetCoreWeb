using DotNetCore.Core.Domain.ScheduleTasks;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Service.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.ScheduleTasks
{
    public partial class Task
    {

        #region Ctor

        /// <summary>
        /// Ctor for Task
        /// </summary>
        private Task()
        {
            this.Enabled = true;
        }

        /// <summary>
        /// Ctor for Task
        /// </summary>
        /// <param name="task">Task </param>
        public Task(Core.Domain.ScheduleTasks.ScheduleTask task)
        {
            this.Type = task.Type;
            this.Enabled = task.Enabled;
            this.StopOnError = task.StopOnError;
            this.Name = task.Name;
            this.LastSuccessUtc = task.LastSuccessUtc;
        }

        #endregion

        #region Utilities

        private ITask CreateTask()
        {
            ITask task = null;
            if (this.Enabled)
            {
                var type2 = System.Type.GetType(Type);
                if (type2 != null)
                {
                    object instance;
                    instance = EngineContext.Current.ServiceProvider.GetService(type2);
                    task = instance as ITask;
                }
            }
            return task;
        }

        #endregion

        #region Methods

        public void Execute(bool throwException = false, bool dispose = true, bool ensureRunOnOneWebFarmInstance = true)
        {
            var scheduleTaskService = EngineContext.Current.GetService<IScheduleTaskService>();
            var scheduleTask = scheduleTaskService.GetTaskByType(this.Type);

            try
            {
                //flag that task is already executed
                var taskExecuted = false;

                //task is run on one farm node at a time?
                if (ensureRunOnOneWebFarmInstance)
                {
                    var machineNameProvider = EngineContext.Current.GetService<IMachineNameProvider>();
                    var machineName = machineNameProvider.GetMachineName();
                    if (String.IsNullOrEmpty(machineName))
                    {
                        throw new Exception("Machine name cannot be detected. You cannot run in web farm.");
                        //actually in this case we can generate some unique string (e.g. Guid) and store it in some "static" (!!!) variable
                        //then it can be used as a machine name
                    }

                    if (scheduleTask != null)
                    {
                        //lease can't be acquired only if for a different machine and it has not expired
                        if (scheduleTask.LeasedUntilUtc.HasValue &&
                            scheduleTask.LeasedUntilUtc.Value >= DateTime.UtcNow &&
                            scheduleTask.LeasedByMachineName != machineName)
                            return;

                        //lease the task. so it's run on one farm node at a time
                        scheduleTask.LeasedByMachineName = machineName;
                        scheduleTask.LeasedUntilUtc = DateTime.UtcNow.AddMinutes(30);
                        scheduleTaskService.Update(scheduleTask);
                    }
                }

                //execute task in case if is not executed yet
                if (!taskExecuted)
                {
                    //initialize and execute
                    var task = this.CreateTask();
                    if (task != null)
                    {
                        this.LastStartUtc = DateTime.UtcNow;
                        if (scheduleTask != null)
                        {
                            //update appropriate datetime properties
                            scheduleTask.LastStartUtc = this.LastStartUtc;
                            scheduleTaskService.Update(scheduleTask);
                        }
                        task.Execute();
                        this.LastEndUtc = this.LastSuccessUtc = DateTime.UtcNow;
                    }
                }
            }
            catch (Exception exc)
            {
                this.Enabled = !this.StopOnError;
                this.LastEndUtc = DateTime.UtcNow;

                //log error
                var logger = EngineContext.Current.GetService<ILogger<Task>>();
                logger.LogError(string.Format("Error while running the '{0}' schedule task. {1}", this.Name, exc.Message), exc);
                if (throwException)
                    throw;
            }

            if (scheduleTask != null)
            {
                //update appropriate datetime properties
                scheduleTask.LastEndUtc = this.LastEndUtc;
                scheduleTask.LastSuccessUtc = this.LastSuccessUtc;
                scheduleTaskService.Update(scheduleTask);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Datetime of the last start
        /// </summary>
        public DateTime? LastStartUtc { get; private set; }

        /// <summary>
        /// Datetime of the last end
        /// </summary>
        public DateTime? LastEndUtc { get; private set; }

        /// <summary>
        /// Datetime of the last success
        /// </summary>
        public DateTime? LastSuccessUtc { get; private set; }

        /// <summary>
        /// A value indicating type of the task
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// A value indicating whether to stop task on error
        /// </summary>
        public bool StopOnError { get; private set; }

        /// <summary>
        /// Get the task name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A value indicating whether the task is enabled
        /// </summary>
        public bool Enabled { get; set; }

        #endregion
    }
}
