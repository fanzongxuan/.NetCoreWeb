using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Interface
{
    public interface IEngine
    {
        void Initialize(IServiceCollection serviceCollection);

        T GetService<T>() where T : class;

        object GetService(Type type);

        IEnumerable<T> GetServices<T>();
    }
}
