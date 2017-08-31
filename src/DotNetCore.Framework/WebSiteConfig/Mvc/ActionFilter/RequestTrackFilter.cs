using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.WebSiteConfig.Mvc.ActionFilter
{
    public class RequestTrackFilter : IActionFilter
    {
        private readonly ILogger _logger;
        public RequestTrackFilter(ILogger<RequestTrackFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //TODO
            _logger.LogInformation(context.HttpContext.Request.Path);
        }
    }
}
