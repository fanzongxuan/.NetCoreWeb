using DotNetCore.Core.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCore.Core.Infrastructure
{
    public class WebEngine : IEngine
    {

        protected virtual void RegisterMapperConfiguration()
        {
            //dependencies
            var typeFinder = new AppDomainTypeFinder();

            //register mapper configurations provided by other assemblies
            var configurationActions = typeFinder.FindClassesOfType<IMapperConfiguration>()
                .Select(Activator.CreateInstance)
                .OfType<IMapperConfiguration>()
                .OrderBy(t => t.Order)
                .Select(t => t.GetConfiguration())
                .ToList();
            //register
            AutoMapperConfiguration.Init(configurationActions);
        }

        protected virtual void AddServices(IServiceCollection services)
        {
            var typeFinder = new AppDomainTypeFinder();
            typeFinder.FindClassesOfType(typeof(IDependency))
                .Select(Activator.CreateInstance)
                .Cast<IDependency>().ToList()
                .ForEach(x => { x.Register(services); });
        }

        protected virtual void RunStartupTasks()
        {
            var typeFinder = new AppDomainTypeFinder();
            typeFinder.FindClassesOfType<IStartupTask>()
                .Select(Activator.CreateInstance)
                .Cast<IStartupTask>()
                .OrderBy(x => x.Order).ToList()
                .ForEach(x => x.Execute());
        }

        public ServiceProvider ServiceProvider { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            AddServices(serviceCollection);
            RegisterMapperConfiguration();
            ServiceProvider = serviceCollection.BuildServiceProvider();
            RunStartupTasks();
        }


        public T GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }

        public object GetService(Type type)
        {
            return ServiceProvider.GetService(type);
        }

        public IEnumerable<T> GetServices<T>()
        {
            return ServiceProvider.GetServices<T>();
        }

    }
}
