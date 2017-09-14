using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Security
{
    public class DefaultPermissionRecord
    {
        public DefaultPermissionRecord()
        {
            this.PermissionRecords = new List<PermissionRecord>();
        }

        /// <summary>
        /// Gets or sets the account role system name
        /// </summary>
        public string AccountRoleName { get; set; }

        /// <summary>
        /// Gets or sets the permissions
        /// </summary>
        public IEnumerable<PermissionRecord> PermissionRecords { get; set; }
    }
}
