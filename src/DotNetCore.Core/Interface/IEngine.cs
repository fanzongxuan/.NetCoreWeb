using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Interface
{
    public interface IEngine
    {
        IServiceProvider ServiceProvider { get; set; }

        void Initialize(IServiceCollection serviceCollection, IConfiguration configuration);

        T GetService<T>() where T : class;

        object GetService(Type type);

        IEnumerable<T> GetServices<T>();
    }
}
