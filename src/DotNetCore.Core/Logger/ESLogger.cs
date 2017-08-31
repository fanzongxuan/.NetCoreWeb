﻿using DotNetCore.Core.ElasticSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Logger
{
    public class ESLogger:ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ESClientProvider _esClient;
        private readonly string _loggerName;
        private readonly LogLevel _logLevel;

        public ESLogger(ESClientProvider esClient, IHttpContextAccessor httpContextAccessor, string loggerName, LogLevel logLevel)
        {
            _esClient = esClient;
            _httpContextAccessor = httpContextAccessor;
            _loggerName = loggerName;
            _logLevel = logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            // Outside scope of article
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            var context = _httpContextAccessor.HttpContext;

            if (true == context?.Request.Path.Value?.ToLower().Contains("."))
            {
                return false;
            }

            return logLevel >= _logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            var entry = new LogEntry
            {
                EventId = eventId,
                DateTime = DateTime.UtcNow,
                Category = _loggerName,
                Message = message,
                Level = logLevel
            };

            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                entry.TraceIdentifier = context.TraceIdentifier;
                entry.UserName = context.User.Identity.Name;
                var request = context.Request;
                entry.ContentLength = request.ContentLength;
                entry.ContentType = request.ContentType;
                entry.Host = request.Host.Value;
                entry.IsHttps = request.IsHttps;
                entry.Method = request.Method;
                entry.Path = request.Path;
                entry.PathBase = request.PathBase;
                entry.Protocol = request.Protocol;
                entry.QueryString = request.QueryString.Value;
                entry.Scheme = request.Scheme;

                entry.Cookies = request.Cookies;
                entry.Headers = request.Headers;
            }

            if (exception != null)
            {
                entry.Exception = exception.ToString();
                entry.ExceptionMessage = exception.Message;
                entry.ExceptionType = exception.GetType().Name;
                entry.StackTrace = exception.StackTrace;
            }

            _esClient.Client.IndexAsync(entry);
        }
    }
}
