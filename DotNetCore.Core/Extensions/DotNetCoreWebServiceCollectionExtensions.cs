using DotNetCore.Core.Infrastructure;
using DotNetCore.Core.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCore.Core.Extensions
{
    public static class DotNetCoreWebServiceCollectionExtensions
    {
        public static void AddAllServices(this IServiceCollection services)
        {
            var typeFinder = new AppDomainTypeFinder();
            typeFinder.FindClassesOfType(typeof(IDependency))
                .Select(Activator.CreateInstance)
                .Cast<IDependency>().ToList()
                .ForEach(x => { x.Register(services); });
        }
    }
}
