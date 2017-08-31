using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using DotNetCore.Core.ElasticSearch;
using Microsoft.AspNetCore.Http;
using DotNetCore.Core.Infrastructure;

namespace DotNetCore.Core.Logger
{
    public class ESLoggerProvider : ILoggerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfigurationSection _optionSection;
        private readonly ESClientProvider _esClient;

        public ESLoggerProvider(IConfigurationSection optionSection,
            string indexName)
        {
            this._httpContextAccessor = EngineContext.Current.GetService<IHttpContextAccessor>();
            this._optionSection = optionSection;
            _esClient = EngineContext.Current.GetService<ESClientProvider>();
            _esClient.EnsureIndexWithMapping<LogEntry>(indexName);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ESLogger(_esClient, _httpContextAccessor, categoryName, FindLevel(categoryName));
        }

        private LogLevel FindLevel(string categoryName)
        {
            var configLevel = _optionSection.GetSection("LogLevel")["ElasticSearch"] ?? _optionSection.GetSection("LogLevel")["Default"];
            if (!string.IsNullOrEmpty(configLevel))
            {
                return (LogLevel)Enum.Parse(typeof(LogLevel), configLevel);
            }
            return LogLevel.Debug;
        }

        public void Dispose()
        {
            // Nothing to do
        }
    }
}