using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.ScheduleTasks
{
    public interface ITask
    {
        void Execute();
    }
}
