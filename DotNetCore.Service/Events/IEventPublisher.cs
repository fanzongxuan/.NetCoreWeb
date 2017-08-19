using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Events
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage);
    }
}
