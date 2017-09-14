using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DotNetCore.Core.Domain.Accounts
{
    public class UserInfo: BaseEntity
    {
        public string LoginName { get; set; }

        public string RealName { get; set; }

        public string Password { get; set; }
        
        public string LastLoginIpAddress { get; set; }

        public int MyProperty { get; set; }

        public DateTime LastLoginTime { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public Sex Sex { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }

    public enum Sex
    {
        UnDefine=0,

        Man=5,

        Female=10
    }
}
