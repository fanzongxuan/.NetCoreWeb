using DotNetCore.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Logger
{
    public static class LoggerExtensions
    {
        public static ILoggerFactory AddESLogger(this ILoggerFactory factory, IConfigurationSection optionSection, string indexName = null)
        {
            factory.AddProvider(new ESLoggerProvider(optionSection, indexName));
            return factory;
        }
    }
}
