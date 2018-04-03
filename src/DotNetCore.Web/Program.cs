using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using DotNetCore.Core.Logger;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace DotNetCore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseIISIntegration()
                //you can config your log privoder and filter rules by code or appsetting.json file,visit https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/logging?tabs=aspnetcore2x;
                //.ConfigureLogging(x => x.AddConsole())
                .Build();
    }
}
