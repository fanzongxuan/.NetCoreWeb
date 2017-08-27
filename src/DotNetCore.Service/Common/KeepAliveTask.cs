using DotNetCore.Service.ScheduleTasks;
using System;
using System.Net;

namespace DotNetCore.Service.Common
{
    public class KeepAliveTask : ITask
    {
        public void Execute()
        {
            string url = "https://www.baidu.com";
            using (var wc = new WebClient())
            {
                wc.DownloadString(url);
            }
        }
    }
}
