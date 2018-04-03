using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Mvc.Config
{
    public static class RuoteConfig
    {
        //docs https://docs.microsoft.com/zh-cn/aspnet/core/mvc/controllers/routing#setting-up-routing-middleware
        public static void Config(this IRouteBuilder routes)
        {
            routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
