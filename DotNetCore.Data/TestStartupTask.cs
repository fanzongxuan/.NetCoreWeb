using DotNetCore.Core.Interface;
using Microsoft.Extensions.Logging;
using System;

namespace DotNetCore.Data
{
    public class TestStartupTask : IStartupTask
    {

        public int Order => 5;

        public void Execute()
        {
            Console.Write("Start Up Task is Running!");
        }
    }
}
