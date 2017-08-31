using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.ElasticSearch
{
    /// <summary>
    /// ElasticSeach option
    /// </summary>
    public class ESOptions
    {
        public bool Enable { get; set; }
        public string Url { get; set; }
        public string DefaultIndex { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
