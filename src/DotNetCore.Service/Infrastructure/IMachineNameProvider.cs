using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Infrastructure
{
    public interface IMachineNameProvider
    {
        string GetMachineName();
    }
}
