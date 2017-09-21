using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Messages
{
    public enum QueuedEmailPriority
    {
        /// <summary>
        /// Low
        /// </summary>
        Low = 0,
        /// <summary>
        /// High
        /// </summary>
        High = 5
    }
}
