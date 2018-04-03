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
using DotNetCore.Core.Domain.Messages;
using DotNetCore.Service.Messages;
using DotNetCore.Service.Helpers;

namespace DotNetCore.Service.Installation
{
    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fileds

        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly ISettingService _settingService;
        private readonly IAccountService _accountService;
        private readonly IPermissionService _permissionService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IRepository<EmailAccount> _emailAccountRepository;

        #endregion

        #region Ctor

        public CodeFirstInstallationService(IRepository<ScheduleTask> scheduleTaskRepository,
            ISettingService settingService,
            IAccountService accountService,
            IPermissionService permissionService,
            IEmailAccountService emailAccountService,
            IRepository<EmailAccount> emailAccountRepository)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
            _settingService = settingService;
            _accountService = accountService;
            _permissionService = permissionService;
            _emailAccountService = emailAccountService;
            _emailAccountRepository = emailAccountRepository;
        }
        #endregion

        #region Uilities
        protected virtual void InstallEmialAccount()
        {
            var defaultEmailAccount = new EmailAccount
            {
                Email = "13814063516@163.com",
                DisplayName = "Store name",
                Host = "smtp.163.com",
                Port = 25,
                Username = "123",
                Password = "123",
                EnableSsl = false,
                UseDefaultCredentials = false
            };

            var isExist = _emailAccountRepository.Table.Any(x => x.Email == defaultEmailAccount.Email);
            if (!isExist)
            {
                _emailAccountRepository.Insert(defaultEmailAccount);
            }

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
            var keepAlive = _scheduleTaskRepository.Table.FirstOrDefault(x => x.Type == keepAliveTask.Type);
            if (keepAlive == null)
            {
                _scheduleTaskRepository.Insert(keepAliveTask);
            }

            //schedule task
            var queuedEmailSendTask = new ScheduleTask
            {
                Name = "Email sender",
                Seconds = 5,
                Type = "DotNetCore.Service.Messages.QueuedEmailSendTask,DotNetCore.Service",
                Enabled = true,
                StopOnError = false,
            };
            var queuedEmailTask = _scheduleTaskRepository.Table.FirstOrDefault(x => x.Type == queuedEmailSendTask.Type);
            if (queuedEmailTask == null)
            {
                _scheduleTaskRepository.Insert(queuedEmailSendTask);
            }
            //else
            //{
            //    keepAlive.Name = keepAliveTask.Name;
            //    keepAlive.Seconds = keepAliveTask.Seconds;
            //    keepAlive.Type = keepAliveTask.Type;
            //    keepAlive.Enabled = keepAliveTask.Enabled;
            //    keepAlive.StopOnError = keepAliveTask.StopOnError;
            //    _scheduleTaskRepository.Update(keepAlive);
            //}

        }

        protected virtual void InstallSettings()
        {
            _settingService.SaveSetting(new AuthorizeSettings
            {
                RequireUniqueEmail = false,
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

            var eaGeneral = _emailAccountRepository.Table.FirstOrDefault();
            if (eaGeneral == null)
                throw new Exception("Default email account cannot be loaded");
            _settingService.SaveSetting(new EmailAccountSettings
            {
                DefaultEmailAccountId = eaGeneral.Id
            });

            _settingService.SaveSetting(new DateTimeSettings
            {
                DefaultStoreTimeZoneId = TimeZoneInfo.Local.Id
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
            InstallEmialAccount();
            InstallScheduleTasks();
            InstallSettings();
            InstallRoles();
            InstallPermission();
            InsertDefaultAccount();
        }
        #endregion

    }
}
