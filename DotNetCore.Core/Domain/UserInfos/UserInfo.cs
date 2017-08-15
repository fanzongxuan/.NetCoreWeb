using System;
using System.Collections.Generic;

namespace DotNetCore.Core.Domain.UserInfos
{
    public class UserInfo: BaseEntity
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
