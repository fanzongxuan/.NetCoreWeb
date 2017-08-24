using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCore.Core.Extensions
{
    public static class ModelStateExtension
    {
        public static IEnumerable<string> Errors(this ModelStateDictionary modelstate)
        {
            return modelstate.Values.Where(t => t.Errors.Any())
                .SelectMany(t => t.Errors.Select(e => e.ErrorMessage));
        }
    }
}
