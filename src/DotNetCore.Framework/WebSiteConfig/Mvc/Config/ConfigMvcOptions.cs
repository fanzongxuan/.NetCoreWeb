using DotNetCore.Framework.WebSiteConfig.Mvc.ActionFilter;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Framework.WebSiteConfig.Mvc.Config
{
    /// <summary>
    /// config mvc options
    /// </summary>
    public static class ConfigMvcOptions
    {
        public static void Config(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add(typeof(ExceptionFilter));
            mvcOptions.Filters.Add(typeof(RequestTrackFilter));
            mvcOptions.MaxModelValidationErrors = 50;
        }
    }
}
