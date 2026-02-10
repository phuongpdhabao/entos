using ENTOS.Module.Interfaces;
using Serilog;
using Serilog.Context;
using Serilog.Core.Enrichers;
using System.Configuration;
using System.Diagnostics;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm;
using Elastic.Apm.Config;
using Elastic.Apm.Api;
using Elastic.Apm.Logging;


namespace ENTOS.Module.SystemServices;

/// <summary>
/// Dịch vụ ghi log sử dụng Serilog và Elasticsearch (ELK), cho phép cấu hình động qua file cấu hình (ví dụ: SeqUrl, ElkUrl, ElkIndex).
/// </summary>
public class ElkLoggerService : ILogService, IDisposable
{
    private readonly ILogger _logger;
    private static readonly IDisposable SerilogNullDisposable = new NullDisposable();
    private bool _disposed = false;

    // --- ApmFileLogger: Ghi log Elastic.Apm ra file ---
    public class ApmFileLogger : IApmLogger
    {
        private static readonly object _logFileLock = new object();
        private readonly string _logFilePath;
        private readonly LogLevel _level;
        public ApmFileLogger(string logFilePath = "Logs/ApmAgent.log", LogLevel level = LogLevel.Debug)
        {
            _logFilePath = logFilePath;
            _level = level;
        }
        public bool IsEnabled(LogLevel level) => level >= _level;
        public void Log<TState>(LogLevel level, TState state, Exception exception, string message)
        {
            if (!IsEnabled(level)) return;
            var log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message} {state} {exception}\n";
            lock (_logFileLock)
                System.IO.File.AppendAllText(_logFilePath, log);
        }
        public void Log<TState>(LogLevel level, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(level)) return;
            var log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {formatter(state, exception)} {exception}\n";
            lock (_logFileLock)
                System.IO.File.AppendAllText(_logFilePath, log);
        }
    }

    public ElkLoggerService()
    {
        string elkUrl = ConfigurationManager.AppSettings["ElkUrl"] ?? "http://localhost:9200";
        string elkIndex = ConfigurationManager.AppSettings["ElkIndex"] ?? "entos-log";
        string elkUser = ConfigurationManager.AppSettings["ElkUser"] ?? "elastic";
        string elkPassword = ConfigurationManager.AppSettings["ElkPassword"] ?? "";
        string apmUrl = ConfigurationManager.AppSettings["ApmUrl"];
        string apmToken = ConfigurationManager.AppSettings["ApmToken"];
        string apmServiceName = ConfigurationManager.AppSettings["ApmServiceName"] ?? "ENTOS.Win";

        _logger = new Serilog.LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day)
            .Enrich.WithEnvironmentUserName()
            .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elkUrl))
            {
                AutoRegisterTemplate = true,
                IndexFormat = elkIndex + "-{0:yyyy.MM.dd}",
                ModifyConnectionSettings = x => x.BasicAuthentication(elkUser, elkPassword)
            })
            .CreateLogger();

        // Khởi tạo Elastic.Apm agent nếu có cấu hình
        if (!string.IsNullOrEmpty(apmUrl) && !string.IsNullOrEmpty(apmToken))
        {
            Environment.SetEnvironmentVariable("ELASTIC_APM_SERVER_URL", apmUrl);
            Environment.SetEnvironmentVariable("ELASTIC_APM_SECRET_TOKEN", apmToken);
            Environment.SetEnvironmentVariable("ELASTIC_APM_SERVICE_NAME", apmServiceName);
            var apmLogger = new ApmFileLogger();
            var agentComponents = new Elastic.Apm.AgentComponents(apmLogger);
            Elastic.Apm.Agent.Setup(agentComponents);
        }
    }

    public ElkLoggerService(Serilog.ILogger logger)
    {
        _logger = logger;
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
        var enrichers = properties.Select(p => new PropertyEnricher(p.name, p.value, true)).Cast<Serilog.Core.ILogEventEnricher>().ToArray();
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

    // --- Elastic APM Extensions ---
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
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