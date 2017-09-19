using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.UI
{
    public class AjaxResult
    {
        public ReturnCode Code { get; set; }

        public string Message { get; set; }
    }

    public enum ReturnCode
    {
        Error = -1,
        Sucess = 1
    }
}
