using DotNetCore.Core.Domain.Security;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Service.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCore.Framework.Controllers.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AdminAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {

        #region Uilities

        private void HandleUnauthorizedRequest(AuthorizationFilterContext filterContext)
        {
            filterContext.Result = new UnauthorizedResult();
        }


        public virtual bool HasAdminAccess()
        {
            var permissionService = EngineContext.Current.GetService<IPermissionService>();
            return permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel);

        }
        #endregion

        #region Methods

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (!this.HasAdminAccess())
                this.HandleUnauthorizedRequest(filterContext);

        }
        #endregion

    }
}
