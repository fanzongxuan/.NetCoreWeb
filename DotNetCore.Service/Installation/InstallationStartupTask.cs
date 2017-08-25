using DotNetCore.Core.Infrastructure;
using DotNetCore.Core.Interface;
using System;

namespace DotNetCore.Service.Installation
{
    public class InstallationStartupTask : IStartupTask
    {
        public int Order => 1;

        public void Execute()
        {
            var installationService = EngineContext.Current.GetService<IInstallationService>();
            installationService.InstallData();
        }
    }
}
