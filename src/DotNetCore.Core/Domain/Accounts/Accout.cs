using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Accounts
{
    public class Account : IdentityUser
    {
        public DateTime CreateOnUtc { get; set; }

        public DateTime LastActivityDateUtc { get; set; }
    }
}
