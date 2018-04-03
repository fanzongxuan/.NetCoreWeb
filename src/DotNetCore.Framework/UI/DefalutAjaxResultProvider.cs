using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.UI
{
    public class DefalutAjaxResultProvider
    {
        public static AjaxResult AccessDenied { get { return new AjaxResult() { Code = ReturnCode.Error, Message = "Access denied!" }; } }
        public static AjaxResult Success { get { return new AjaxResult() { Code = ReturnCode.Sucess, Message = "Success!" }; } }
        public static AjaxResult NotFound { get { return new AjaxResult() { Code = ReturnCode.Error, Message = "Not found!" }; } }
        public static AjaxResult Error { get { return new AjaxResult() { Code = ReturnCode.Error, Message = "Error!" }; } }
    }
}
