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

namespace DotNetCore.Service.Installation
{
    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fileds

        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly ISettingService _settingService;
        private readonly IAccountService _accountService;
        #endregion

        #region Ctor

        public CodeFirstInstallationService(IRepository<ScheduleTask> scheduleTaskRepository,
            ISettingService settingService,
            IAccountService accountService,
            RoleManager<AccountRole> roleManager,
            UserManager<Account> userManager)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
            _settingService = settingService;
            _accountService = accountService;
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
            // add system defalut roles
            if (!_accountService.RoleExists(AccountRoleNames.Guest))
                _accountService.CreateRole(new AccountRole(AccountRoleNames.Guest));

            if (!_accountService.RoleExists(AccountRoleNames.Administrators))
                _accountService.CreateRole(new AccountRole(AccountRoleNames.Administrators));

            if (!_accountService.RoleExists(AccountRoleNames.Register))
                _accountService.CreateRole(new AccountRole(AccountRoleNames.Register));
        }

        protected virtual void InsertDefaultAccount()
        {
            //install defalut administration account
            var user = new Account() { UserName = "admin@123", Email = "admin@123.com" };
            var isExist = _accountService.AccountIsExist(user.UserName);

            if (!isExist)
            {
                var res = _accountService.Register(user, "Admin@123.com");
                if (res != IdentityResult.Success)
                {
                    var errors = res.Errors.Select(x => x.Description);
                    throw new Exception($"Inital administration failed! Error description:{string.Join(";", errors)}");
                }
                var roleRes = _accountService.AddToRole(user, AccountRoleNames.Administrators);
                if (!roleRes.Succeeded)
                    throw new Exception("attach administration to administration role failed");
            }

            // install defalut system backgroud task user
            var isExist2 = _accountService.AccountIsExist(SystemAccountNames.BackgroundTask);

            if (!isExist2)
            {
                var backGroundTask = new Account() { UserName = SystemAccountNames.BackgroundTask, Email = "BackgroundTask@123.com" };
                var res = _accountService.Register(backGroundTask, "BackgroundTask@123.com");
                if (res != IdentityResult.Success)
                {
                    var errors = res.Errors.Select(x => x.Description);
                    throw new Exception($"Inital backGroundTask user failed! Error description:{string.Join(";", errors)}");
                }
                var roleRes = _accountService.AddToRole(backGroundTask, AccountRoleNames.SystemRole);
                if (!roleRes.Succeeded)
                    throw new Exception("attach backGroundTask to SystemRole role failed");
            }

            // install system search engine task user
            var isExist3 = _accountService.AccountIsExist(SystemAccountNames.SearchEngine);

            if (!isExist3)
            {
                var searchEngine = new Account() { UserName = SystemAccountNames.SearchEngine, Email = "SearchEngine@123.com" };
                var res = _accountService.Register(searchEngine, "SearchEngine@123.com");
                if (res != IdentityResult.Success)
                {
                    var errors = res.Errors.Select(x => x.Description);
                    throw new Exception($"Inital SearchEngine user failed! Error description:{string.Join(";", errors)}");
                }
                var roleRes = _accountService.AddToRole(searchEngine, AccountRoleNames.SystemRole);
                if (!roleRes.Succeeded)
                    throw new Exception("Attach SearchEngine to systemRole role failed");

            }
        }
        #endregion

        #region Methods

        public void InstallData()
        {
            InstallScheduleTasks();
            InstallSettings();
            InstallRoles();
            InsertDefaultAccount();
        }
        #endregion

    }
}
