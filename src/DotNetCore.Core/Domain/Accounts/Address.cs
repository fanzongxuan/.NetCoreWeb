using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Accounts
{
    public class Address : BaseEntity
    {
        public string PhoneNumber { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
