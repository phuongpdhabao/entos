using ENTOS.Module.Interfaces;
using System;
using System.Runtime.CompilerServices;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Lớp helper tĩnh để truy cập dịch vụ ghi log từ bất kỳ đâu trong ứng dụng.
    /// Cần được khởi tạo tại điểm bắt đầu của ứng dụng (ví dụ: Program.cs) bằng cách gọi hàm Initialize.
    /// </summary>
    public static class LogHelper
    {
        private static ILogService _logService;
        private static readonly object _lock = new object();

        /// <summary>
        /// Khởi tạo LogHelper với một triển khai logger cụ thể.
        /// </summary>
        /// <param name="logService">Dịch vụ logger (ví dụ: SerilogLoggerService).</param>
        public static void Initialize(ILogService logService)
        {
            lock (_lock)
            {
                // Dispose service cũ nếu có
                _logService?.Dispose();
                _logService = logService;
            }
        }

        /// <summary>
        /// Dispose logger service hiện tại.
        /// </summary>
        public static void Dispose()
        {
            lock (_lock)
            {
                _logService?.Dispose();
                _logService = null;
            }
        }

        public static void Info(string message) => _logService?.LogInformation(message);

        public static void Warn(string message, Exception ex = null) => _logService?.LogWarning(message, ex);

        public static void Error(string message, Exception ex = null) => _logService?.LogError(message, ex);

        public static void Fatal(string message, Exception ex = null) => _logService?.LogFatal(message, ex);

        public static void Debug(string message) => _logService?.LogDebug(message);

        public static void Verbose(string message) => _logService?.LogVerbose(message);

        /// <summary>
        /// Thêm một thuộc tính vào context của log.
        /// </summary>
        public static IDisposable PushProperty(string name, object value) => _logService?.PushProperty(name, value);

        /// <summary>
        /// Thêm nhiều thuộc tính vào context của log.
        /// </summary>
        /// <param name="properties">Mảng các thuộc tính cần thêm, mỗi thuộc tính là một cặp (tên, giá trị).</param>
        public static IDisposable PushProperties(params (string name, object value)[] properties) => _logService?.PushProperties(properties);

        #region --- Extensions ---

        /// <summary>
        /// Ghi log Information nếu điều kiện đúng.
        /// </summary>
        public static void InfoIf(bool condition, string message) => _logService?.LogInformationIf(condition, message);
        
        /// <summary>
        /// Ghi log Warning nếu điều kiện đúng.
        /// </summary>
        public static void WarnIf(bool condition, string message, Exception ex = null) => _logService?.LogWarningIf(condition, message, ex);

        /// <summary>
        /// Ghi log Error nếu điều kiện đúng.
        /// </summary>
        public static void ErrorIf(bool condition, string message, Exception ex = null) => _logService?.LogErrorIf(condition, message, ex);

        /// <summary>
        /// Đo thời gian thực thi của một khối lệnh.
        /// </summary>
        public static IDisposable TimeOperation(string operationName) => _logService?.TimeOperation(operationName);

        /// <summary>
        /// Đo thời gian thực thi của một khối lệnh và đính kèm các thuộc tính vào log context.
        /// </summary>
        public static IDisposable TimeOperation(string operationName, params (string name, object value)[] properties) => _logService?.TimeOperation(operationName, properties);

        /// <summary>
        /// Ghi log Information kèm thông tin mã nguồn (tên file, phương thức, số dòng).
        /// </summary>
        public static void SourceInfo(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            _logService?.LogSourceInformation(message, memberName, sourceFilePath, sourceLineNumber);
        }

        #endregion
    }
} 