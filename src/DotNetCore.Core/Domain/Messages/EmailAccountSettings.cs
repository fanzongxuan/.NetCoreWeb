using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Messages
{
    public class EmailAccountSettings : ISetting
    {
        /// <summary>
        /// Gets or sets a store default email account identifier
        /// </summary>
        public int DefaultEmailAccountId { get; set; }
    }
}
