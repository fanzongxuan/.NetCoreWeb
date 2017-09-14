using DotNetCore.Core.Infrastructure;
using DotNetCore.Service.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCore.Framework.Controllers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        private void HandleUnauthorizedRequest(AuthorizationFilterContext filterContext)
        {
            filterContext.Result = new UnauthorizedResult();
        }

      

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if(!this.HasAdminAccess())
                this.HandleUnauthorizedRequest(filterContext);

        }

        public virtual bool HasAdminAccess()
        {
            var permissionService = EngineContext.Current.GetService<IPermissionService>();
            bool result = permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel);
            return result;
        }
    }
}
