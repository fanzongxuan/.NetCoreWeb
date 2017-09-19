using Autofac;
using Autofac.Extensions.DependencyInjection;
using DotNetCore.Core.Interface;
using Microsoft.Extensions.Configuration;
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

        protected virtual void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);

            var typeFinder = new AppDomainTypeFinder();
            typeFinder.FindClassesOfType(typeof(IDependency))
                .Select(Activator.CreateInstance)
                .Cast<IDependency>()
                .OrderBy(x => x.Order).ToList()
                .ForEach(x => { x.Register(builder, typeFinder, configuration); });

            var container = builder.Build();
            this.ServiceProvider = container.Resolve<IServiceProvider>();
            //this.ServiceProvider = new AutofacServiceProvider(container);
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

        public IServiceProvider ServiceProvider { get; set; }

        public void Initialize(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            RegisterServices(serviceCollection, configuration);
            RegisterMapperConfiguration();
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
