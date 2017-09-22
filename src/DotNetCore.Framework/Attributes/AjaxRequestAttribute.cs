using DotNetCore.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace DotNetCore.Framework.Attributes
{
    public class AjaxRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.IsAjaxRequest())
                context.Result = new StatusCodeResult((int)HttpStatusCode.BadRequest);
            base.OnActionExecuting(context);
        }
    }
}
