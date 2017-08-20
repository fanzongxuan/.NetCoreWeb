using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.ScheduleTaskeService
{
    public interface ITask
    {
        void Execute();
    }
}
