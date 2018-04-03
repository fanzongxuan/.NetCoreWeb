using DotNetCore.Core.Interface;
using System;
using DotNetCore.Service.Events;
using DotNetCore.Core.Infrastructure;
using System.Linq;
using DotNetCore.Service.Installation;
using DotNetCore.Service.ScheduleTasks;
using DotNetCore.Service.Accounts;
using DotNetCore.Service.Settings;
using Microsoft.Extensions.Configuration;
using Autofac;
using DotNetCore.Service.Security;
using DotNetCore.Service.Messages;
using Autofac.Core;
using System.Reflection;
using Autofac.Builder;
using System.Collections.Generic;
using DotNetCore.Service.Helpers;

namespace DotNetCore.Service.Infrastructure.DependencyManagement
{
    public class Dependency : IDependency
    {
        public int Order => 0;

        public void Register(ContainerBuilder builder, AppDomainTypeFinder typeFinder, IConfiguration configuration)
        {
            var assemblies = typeFinder.GetAssemblies();

            //Register my services
            typeFinder.FindInterfacesOfType(typeof(IBaseService<>), assemblies)
                 .ToList()
                 .ForEach(x =>
                 {
                     if (x != typeof(IBaseService<>))
                     {
                         typeFinder.FindClassesOfType(x)
                          .ToList()
                          .ForEach(t =>
                          {
                              builder.RegisterType(t).As(x).InstancePerLifetimeScope();
                          });
                     }
                 });

            //Event
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().InstancePerLifetimeScope();

            //Register Consumers
            typeFinder.FindClassesOfType(typeof(IConsumer<>), assemblies)
                .ToList()
                .ForEach(x =>
                {
                    x.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>))
                    .ToList()
                    .ForEach(t =>
                    {
                        builder.RegisterType(x).As(t).InstancePerLifetimeScope();
                    });
                });


            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();

            //Register MachineNameProvider
            builder.RegisterType<DefaultMachineNameProvider>().As<IMachineNameProvider>().InstancePerLifetimeScope();

            //InstallationService
            builder.RegisterType<CodeFirstInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();

            //Register task
            typeFinder.FindClassesOfType(typeof(ITask), assemblies)
                .ToList()
                .ForEach(x =>
                {
                    builder.RegisterType(x).SingleInstance();
                });

            //Register settings
            builder.RegisterSource(new SettingsSource());

            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();
            builder.RegisterType<StandardPermissionProvider>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();

        }
    }

    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Autofac.Core.Service service,
                Func<Autofac.Core.Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISetting).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISetting, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) =>
                {
                    return c.Resolve<ISettingService>().LoadSetting<TSettings>();
                })
                .InstancePerLifetimeScope()
                .CreateRegistration();
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }
}
