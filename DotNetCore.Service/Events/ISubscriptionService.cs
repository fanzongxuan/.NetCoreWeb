using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Events
{
    public interface ISubscriptionService
    {
        IList<IConsumer<T>> GetSubscriptions<T>();
    }
}
