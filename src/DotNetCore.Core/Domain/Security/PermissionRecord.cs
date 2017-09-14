using DotNetCore.Core.Domain.Accounts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Security
{
    /// <summary>
    /// Represents a permission record
    /// </summary>
    public class PermissionRecord : BaseEntity
    {
        private ICollection<RolePermissionMap> _rolePermissionMap;

        /// <summary>
        /// Gets or sets the permission name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permission system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the permission category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets role permission map
        /// </summary>
        public virtual ICollection<RolePermissionMap> RolePermissionMaps
        {
            get { return _rolePermissionMap ?? (_rolePermissionMap = new List<RolePermissionMap>()); }
            protected set { _rolePermissionMap = value; }
        }
    }
}
