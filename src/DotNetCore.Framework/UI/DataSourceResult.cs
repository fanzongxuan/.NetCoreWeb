using System.Collections;

namespace DotNetCore.Framework.UI
{
    public class DataSourceResult
    {
        public IEnumerable rows { get; set; }

        public int total { get; set; }
    }
}
