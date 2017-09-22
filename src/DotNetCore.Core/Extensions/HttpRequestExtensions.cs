using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException("httpRequest");

            if (httpRequest.Headers == null)
                return false;

            var requestWith = httpRequest.Headers["X-Requested-With"];
            return (!string.IsNullOrEmpty(requestWith) && requestWith == "XMLHttpRequest") ? true : false;
        }
    }
}
