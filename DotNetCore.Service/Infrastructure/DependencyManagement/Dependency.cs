using DotNetCore.Core.Interface;
using System;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Service.Events;
using DotNetCore.Service.UserInfoService;
using DotNetCore.Core.Infrastructure;
using System.Linq;

namespace DotNetCore.Service.Infrastructure.DependencyManagement
{
    public class Dependency : IDependency
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IUserinfoService, UserinfoService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IEventPublisher, EventPublisher>();

            //Register event consumers
            var typeFinder = new AppDomainTypeFinder();
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                //find the consumer's all interfaces which is GenericType
                var interfaces = consumer.FindInterfaces((type, criteria) =>
                   {
                       var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                       return isMatch;
                   }, typeof(IConsumer<>));

                foreach (var item in interfaces)
                {
                    services.AddScoped(item, consumer);
                }


            }
        }
    }
}
