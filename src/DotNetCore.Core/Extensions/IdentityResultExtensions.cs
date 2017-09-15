using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCore.Core.Extensions
{
    public static class IdentityResultExtensions
    {
        public static IEnumerable<string> ErrorDescrirtions(this IdentityResult identity)
        {
            return identity.Errors.Select(x => x.Description);
        }
    }
}
