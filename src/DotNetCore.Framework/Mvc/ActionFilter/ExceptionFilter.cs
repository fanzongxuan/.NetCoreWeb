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
            _logger.LogError(0, context.Exception,context.Exception.Message);
        }
    }
}
