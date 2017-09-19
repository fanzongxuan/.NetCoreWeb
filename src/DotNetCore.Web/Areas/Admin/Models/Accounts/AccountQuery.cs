using DotNetCore.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Web.Areas.Admin.Models.Accounts
{
    public class AccountQuery:DataSourceRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Keywords { get; set; }
    }
}
