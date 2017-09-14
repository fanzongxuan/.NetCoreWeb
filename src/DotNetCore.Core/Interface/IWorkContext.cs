using DotNetCore.Core.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Interface
{
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current account
        /// </summary>
        Account CurrentAccount { get; set; }
    }
}
