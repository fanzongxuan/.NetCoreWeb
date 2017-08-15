using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.UserInfos
{
    public class  Address: BaseEntity
    {
        public string Street { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
