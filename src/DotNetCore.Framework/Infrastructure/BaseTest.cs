using DotNetCore.Framework.WebSiteConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.Framework.Infrastructure
{
    public class BaseTest
    {
        public BaseTest()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

            var config = builder.Build();

            IServiceCollection services = new ServiceCollection();
            // config services
            services.ConfigMyWebServices(config); // Create the container builder.
            
        }
    }
}
