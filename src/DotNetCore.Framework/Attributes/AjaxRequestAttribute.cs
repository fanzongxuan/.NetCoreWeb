using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DotNetCore.Framework.Attributes
{
    public class AjaxRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestWith = context.HttpContext.Request.Headers["X-Requested-With"];
            bool isAjaxRequest = (!string.IsNullOrEmpty(requestWith) && requestWith == "XMLHttpRequest") ? true : false;
            if (!isAjaxRequest)
                context.Result = new StatusCodeResult((int)HttpStatusCode.BadRequest);
            base.OnActionExecuting(context);
        }
    }
}
