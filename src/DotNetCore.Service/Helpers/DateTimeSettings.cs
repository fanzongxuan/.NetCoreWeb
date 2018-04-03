using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Helpers
{
    public class DateTimeSettings: ISetting
    {
        /// <summary>
        /// Gets or sets a default store time zone identifier
        /// </summary>
        public string DefaultStoreTimeZoneId { get; set; }
    }
}
