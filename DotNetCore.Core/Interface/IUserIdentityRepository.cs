using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Interface
{
    interface IUserIdentityRepository<T> where T : IdentityUser
    {
    }
}
