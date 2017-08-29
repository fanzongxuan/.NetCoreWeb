using DotNetCore.Core.Interface;
using System;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Service.Events;
using DotNetCore.Service.UserInfoService;
using DotNetCore.Core.Infrastructure;
using System.Linq;
using DotNetCore.Service.Installation;
using DotNetCore.Service.Common;
using DotNetCore.Service.ScheduleTasks;
using DotNetCore.Service.Accounts;
using DotNetCore.Service.Settings;
using Microsoft.Extensions.Configuration;
using Autofac;

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
            
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();

        }
    }
}
