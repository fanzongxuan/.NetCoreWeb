using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Infrastructure
{
    public class DefaultMachineNameProvider:IMachineNameProvider
    {
        public string GetMachineName()
        {
            return System.Environment.MachineName;
        }
    }
}
