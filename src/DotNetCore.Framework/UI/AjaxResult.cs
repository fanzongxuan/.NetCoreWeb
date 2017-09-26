using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.UI
{
    public class AjaxResult
    {
        public ReturnCode Code { get; set; } = ReturnCode.Sucess;

        public string Message { get; set; } = "Success";
    }

    public enum ReturnCode
    {
        Error = -1,
        Sucess = 1
    }
}
