using DotNetCore.Core.Infrastructure;
using DotNetCore.Core.Interface;
using DotNetCore.Service.Accounts;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Attributes
{
    public class CustomerLastActivityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null || context.HttpContext == null || context.HttpContext.Request == null)
                return;

            //only GET requests
            if (!String.Equals(context.HttpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var workContext = EngineContext.Current.GetService<IWorkContext>();
            var customer = workContext.CurrentAccount;

            //update last activity date
            if (customer.LastActivityDateUtc.AddMinutes(1.0) < DateTime.UtcNow)
            {
                var customerService = EngineContext.Current.GetService<IAccountService>();
                customer.LastActivityDateUtc = DateTime.UtcNow;
                customerService.Update(customer);
            }

            base.OnActionExecuting(context);
        }
    }
}
