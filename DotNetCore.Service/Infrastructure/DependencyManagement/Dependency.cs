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

namespace DotNetCore.Service.Infrastructure.DependencyManagement
{
    public class Dependency : IDependency
    {
        public void Register(IServiceCollection services)
        {
            var typeFinder = new AppDomainTypeFinder();
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
                              services.AddScoped(x, t);
                          });
                     }
                 });

            //Event
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IEventPublisher, EventPublisher>();

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
                        services.AddScoped(t, x);
                    });
                });

            //Register MachineNameProvider
            services.AddSingleton<IMachineNameProvider, DefaultMachineNameProvider>();

            //InstallationService
            services.AddSingleton<IInstallationService, CodeFirstInstallationService>();

            //Register task
            typeFinder.FindClassesOfType(typeof(ITask), assemblies)
                .ToList()
                .ForEach(x => {
                    services.AddSingleton(x);
                });
        }
    }
}
