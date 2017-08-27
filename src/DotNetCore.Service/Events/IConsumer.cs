using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Events
{
    public interface IConsumer<T>
    {
        void HandleEvent(T eventMessage);
    }
}
