using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Web.Controllers;
using DotNetCore.Framework.Controllers;

namespace DotNetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class BaseAdminController : BaseController
    {
    }
}