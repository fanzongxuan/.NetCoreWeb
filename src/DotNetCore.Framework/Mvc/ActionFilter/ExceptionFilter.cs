using DotNetCore.Core.Extensions;
using DotNetCore.Framework.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace DotNetCore.Framework.Mvc.ActionFilter
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("DotNetCore-Web");
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(0, context.Exception, context.Exception.Message);

            //Is ajax request
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new JsonResult(new AjaxResult()
                {
                    Code = ReturnCode.Error,
                    Message = context.Exception.Message
                });
            }
        }
    }
}
