using ENTOS.Module.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;
using System.Configuration;
using System.Diagnostics;


namespace ENTOS.Module.SystemServices
{
    public class SeqLoggerService : ILogService, IDisposable
    {
        private readonly ILogger _logger;
        private static readonly IDisposable SerilogNullDisposable = new NullDisposable();
        private bool _disposed = false;

        public SeqLoggerService(IConfiguration? configuration = null)
        {
            // Tự cấu hình Serilog
            string? seqUrl = null;
            string? apiKey = null;
            string solutionName = null;

            // Ưu tiên đọc từ IConfiguration nếu có (Blazor)
            if (configuration != null)
            {
                seqUrl = configuration["Serilog:WriteTo:0:Args:serverUrl"];
                apiKey = configuration["Serilog:WriteTo:0:Args:apiKey"];
            }
            else
            {
                // Nếu không có IConfiguration thì đọc từ app.config (WinForms)
                seqUrl = System.Configuration.ConfigurationManager.AppSettings["SeqUrl"];
                apiKey = System.Configuration.ConfigurationManager.AppSettings["SeqApiKey"];
            }
            try
            {
                solutionName = AppDomain.CurrentDomain.FriendlyName.Split('.')[0];
            }
            catch (Exception) { }
            seqUrl ??= "http://localhost:5341";

            _logger = new Serilog.LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Solution", solutionName)
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .WriteTo.Seq(seqUrl, apiKey: apiKey)
                .CreateLogger();
        }

        public void LogDebug(string message) => _logger.Debug(message);
        public void LogError(string message, Exception ex = null) => _logger.Error(ex, message);
        public void LogFatal(string message, Exception ex = null) => _logger.Fatal(ex, message);
        public void LogInformation(string message) => _logger.Information(message);
        public void LogVerbose(string message) => _logger.Verbose(message);
        public void LogWarning(string message, Exception ex = null) => _logger.Warning(ex, message);

        public IDisposable PushProperty(string name, object value) => LogContext.PushProperty(name, value);

        public IDisposable PushProperties(params (string name, object value)[] properties)
        {
            if (properties == null || properties.Length == 0) return SerilogNullDisposable;
            var enrichers = properties.Select(p => new PropertyEnricher(p.name, p.value, true)).Cast<ILogEventEnricher>().ToArray();
            return LogContext.Push(enrichers);
        }

        public void LogInformationIf(bool condition, string message)
        {
            if (condition) _logger.Information(message);
        }

        public void LogWarningIf(bool condition, string message, Exception ex = null)
        {
            if (condition) _logger.Warning(ex, message);
        }

        public void LogErrorIf(bool condition, string message, Exception ex = null)
        {
            if (condition) _logger.Error(ex, message);
        }

        public IDisposable TimeOperation(string operationName)
        {
            return new OperationTimer(_logger, operationName, null);
        }

        public IDisposable TimeOperation(string operationName, params (string name, object value)[] properties)
        {
            var propertiesDisposable = PushProperties(properties);
            var timerDisposable = new OperationTimer(_logger, operationName, properties);
            return new CombinedDisposable(propertiesDisposable, timerDisposable);
        }

        public void LogSourceInformation(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            _logger.ForContext("MemberName", memberName)
                   .ForContext("SourceFilePath", Path.GetFileName(sourceFilePath))
                   .ForContext("SourceLineNumber", sourceLineNumber)
                   .Information(message);
        }

        #region --- Elastic APM Extensions (Simple Implementation) ---

        public string StartTransaction(string name, string type = "custom")
        {
            _logger.Information("Bắt đầu transaction: {TransactionName} ({TransactionType})", name, type);
            return Guid.NewGuid().ToString();
        }

        public void EndTransaction(string result = "success")
        {
            _logger.Information("Kết thúc transaction với kết quả: {Result}", result);
        }

        public string StartSpan(string name, string type = "custom", string subtype = null, string action = null)
        {
            _logger.Information("Bắt đầu span: {SpanName} ({SpanType}/{SpanSubtype}/{SpanAction})", name, type, subtype ?? "null", action ?? "null");
            return Guid.NewGuid().ToString();
        }

        public void EndSpan(string result = "success")
        {
            _logger.Information("Kết thúc span với kết quả: {Result}", result);
        }

        public void RecordMetric(string name, double value, string unit = "count")
        {
            _logger.Information("Metric: {MetricName} = {MetricValue} {MetricUnit}", name, value, unit);
        }

        public void RecordEvent(string name, object data = null)
        {
            if (data != null)
            {
                _logger.Information("Event: {EventName} - Data: {@EventData}", name, data);
            }
            else
            {
                _logger.Information("Event: {EventName}", name);
            }
        }

        public void SetCorrelationId(string correlationId)
        {
            _logger.Information("Set correlation ID: {CorrelationId}", correlationId);
        }

        public void RecordBusinessMetric(string name, double value, string category = "business")
        {
            _logger.Information("Business Metric: {Category}.{MetricName} = {MetricValue}", category, name, value);
        }

        public void SetUserContext(string userId, string username = null, string email = null)
        {
            _logger.Information("Set user context: ID={UserId}, Username={Username}, Email={Email}", userId, username, email);
        }

        public void SetCustomContext(string key, object value)
        {
            _logger.Information("Set custom context: {ContextKey} = {ContextValue}", key, value);
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // Đóng và flush Serilog logger
                if (_logger != null)
                {
                    Serilog.Log.CloseAndFlush();
                }
                _disposed = true;
            }
        }

        private class NullDisposable : IDisposable
        {
            public void Dispose() { }
        }

        private class OperationTimer : IDisposable
        {
            private readonly ILogger _logger;
            private readonly string _operationName;
            private readonly Stopwatch _stopwatch;
            private readonly (string name, object value)[] _properties;

            public OperationTimer(ILogger logger, string operationName, (string name, object value)[] properties)
            {
                _logger = logger;
                _operationName = operationName;
                _properties = properties;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                var loggerForOperation = _logger;
                if (_properties != null && _properties.Length > 0)
                {
                    foreach (var prop in _properties)
                    {
                        loggerForOperation = loggerForOperation.ForContext(prop.name, prop.value);
                    }
                }
                loggerForOperation.Information("Thao tác '{OperationName}' hoàn thành trong {ElapsedMilliseconds}ms.", _operationName, _stopwatch.ElapsedMilliseconds);
            }
        }

        private class CombinedDisposable : IDisposable
        {
            private readonly Stack<IDisposable> _disposables;

            public CombinedDisposable(params IDisposable[] disposables)
            {
                _disposables = new Stack<IDisposable>(disposables.Where(d => d != null));
            }

            public void Dispose()
            {
                while (_disposables.Any())
                {
                    _disposables.Pop().Dispose();
                }
            }
        }
    }
}