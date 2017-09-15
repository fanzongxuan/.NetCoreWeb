using DotNetCore.Core.Domain.ScheduleTasks;
using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Core.Interface;
using DotNetCore.Service.Settings;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetCore.Service.Accounts;
using DotNetCore.Service.Security;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Core.Extensions;

namespace DotNetCore.Service.Installation
{
    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fileds

        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly ISettingService _settingService;
        private readonly IAccountService _accountService;
        private readonly IPermissionService _permissionService;
        #endregion

        #region Ctor

        public CodeFirstInstallationService(IRepository<ScheduleTask> scheduleTaskRepository,
            ISettingService settingService,
            IAccountService accountService,
            RoleManager<AccountRole> roleManager,
            UserManager<Account> userManager,
            IPermissionService permissionService)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
            _settingService = settingService;
            _accountService = accountService;
            _permissionService = permissionService;
        }
        #endregion

        #region Uilities

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
                RequireUniqueEmail = true,
                RequirePasswordLength = 8,
                RequiredPasswordUniqueChars = 5,
                RequirePasswordNonAlphanumeric = false,
                RequirePasswordLowercase = false,
                RequirePasswordUppercase = false,
                RequirePasswordDigit = true,
                MaxFailedAccessAttempts = 5,
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
                RequireConfirmedEmail = false,
                RequireConfirmedPhoneNumber = false
            });
        }

        protected virtual void InstallRoles()
        {
            _accountService.InstallRoles();
        }

        protected virtual void InsertDefaultAccount()
        {
            //install defalut administration account
            var user = new Account() { UserName = "admin@123", Email = "admin@123.com" };
            _accountService.CreateAccountIfNotExist(user, "Admin@123.com");
            _accountService.AddToRoleIfNotIn(user.UserName, AccountRoleNames.Administrators);

            // install defalut system backgroud task user
            var backGroundTask = new Account() { UserName = SystemAccountNames.BackgroundTask, Email = "BackgroundTask@123.com" };
            _accountService.CreateAccountIfNotExist(backGroundTask, "backGroundTask@123.com");
            _accountService.AddToRoleIfNotIn(backGroundTask.UserName, AccountRoleNames.SystemRole);

            // install system search engine task user
            var searchEngine = new Account() { UserName = SystemAccountNames.SearchEngine, Email = "SearchEngine@123.com" };
            _accountService.CreateAccountIfNotExist(searchEngine, "SearchEngine@123.com");
            _accountService.AddToRoleIfNotIn(searchEngine.UserName, AccountRoleNames.SystemRole);

        }

        protected virtual void InstallPermission()
        {
            var typeFinder = new AppDomainTypeFinder();
            typeFinder.FindClassesOfType<IPermissionProvider>().ToList()
                      .ForEach(x =>
                      {
                          var provider = Activator.CreateInstance(x) as IPermissionProvider;
                          if (provider != null)
                              _permissionService.InstallPermissions(provider);
                      });
        }

        #endregion

        #region Methods

        public void InstallData()
        {
            InstallScheduleTasks();
            InstallSettings();
            InstallRoles();
            InstallPermission();
            InsertDefaultAccount();
        }
        #endregion

    }
}
