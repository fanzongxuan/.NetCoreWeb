using DotNetCore.Core.Domain.ScheduleTasks;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Core.Interface;
using DotNetCore.Service.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCore.Service.Installation
{
    public class CodeFirstInstallationService : IInstallationService
    {
        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly ISettingService _settingService;

        public CodeFirstInstallationService(IRepository<ScheduleTask> scheduleTaskRepository,
            ISettingService settingService)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
            _settingService = settingService;
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

        protected virtual void InstallSettings()
        {
            _settingService.SaveSetting(new AuthorizeSettings
            {
                RequireUniqueEmail=true,
                RequirePasswordLength=8,
                RequiredPasswordUniqueChars=5,
                RequirePasswordNonAlphanumeric=false,
                RequirePasswordLowercase=false,
                RequirePasswordUppercase=false,
                RequirePasswordDigit=true,
                MaxFailedAccessAttempts=5,
                DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5),
                RequireConfirmedEmail=false,
                RequireConfirmedPhoneNumber=false
            });
        }

        public void InstallData()
        {
            InstallScheduleTasks();
            InstallSettings();
        }

    }
}
