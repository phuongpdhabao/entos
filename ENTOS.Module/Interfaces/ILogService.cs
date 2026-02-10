using System;

namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface trừu tượng cho dịch vụ ghi log.
    /// Cho phép hoán đổi các thư viện log (Serilog, NLog, AppInsights, etc.) mà không ảnh hưởng đến code ứng dụng.
    /// </summary>
    public interface ILogService : IDisposable
    {
        /// <summary>
        /// Ghi log một thông điệp thông tin (Information).
        /// </summary>
        /// <param name="message">Nội dung thông điệp.</param>
        void LogInformation(string message);

        /// <summary>
        /// Ghi log một thông điệp cảnh báo (Warning).
        /// </summary>
        /// <param name="message">Nội dung cảnh báo.</param>
        /// <param name="ex">Exception đi kèm (tùy chọn).</param>
        void LogWarning(string message, Exception ex = null);

        /// <summary>
        /// Ghi log một lỗi (Error).
        /// </summary>
        /// <param name="message">Nội dung lỗi.</param>
        /// <param name="ex">Exception đi kèm (tùy chọn).</param>
        void LogError(string message, Exception ex = null);

        /// <summary>
        /// Ghi log một lỗi nghiêm trọng (Fatal).
        /// </summary>
        /// <param name="message">Nội dung lỗi.</param>
        /// <param name="ex">Exception đi kèm (tùy chọn).</param>
        void LogFatal(string message, Exception ex = null);

        /// <summary>
        /// Ghi log thông điệp debug (chỉ dùng khi phát triển).
        /// </summary>
        /// <param name="message">Nội dung debug.</param>
        void LogDebug(string message);

        /// <summary>
        /// Ghi log thông điệp chi tiết (Verbose/Trace).
        /// </summary>
        /// <param name="message">Nội dung chi tiết.</param>
        void LogVerbose(string message);

        /// <summary>
        /// Thêm một thuộc tính vào context của log, sẽ được đính kèm vào tất cả các log event trong scope.
        /// Cần được đặt trong một khối `using`.
        /// </summary>
        /// <param name="name">Tên thuộc tính.</param>
        /// <param name="value">Giá trị thuộc tính.</param>
        /// <returns>Một đối tượng IDisposable để xóa thuộc tính khỏi context khi scope kết thúc.</returns>
        IDisposable PushProperty(string name, object value);

        /// <summary>
        /// Thêm nhiều thuộc tính vào context của log một cách đồng thời.
        /// Các thuộc tính này sẽ được đính kèm vào tất cả các log event được ghi trong scope của khối `using`.
        /// Rất hữu ích để truy vết các request, transaction hoặc các tiến trình có nhiều thông tin ngữ cảnh.
        /// </summary>
        /// <example>
        /// <code>
        /// using (LogHelper.PushProperties(("RequestId", "123"), ("UserId", "456")))
        /// {
        ///     LogHelper.Info("Bắt đầu xử lý."); // Log này sẽ chứa cả RequestId và UserId
        /// }
        /// </code>
        /// </example>
        /// <param name="properties">Mảng các thuộc tính cần thêm, mỗi thuộc tính là một cặp (tên, giá trị).</param>
        /// <returns>Một đối tượng IDisposable để tự động xóa các thuộc tính khỏi context khi ra khỏi khối `using`.</returns>
        IDisposable PushProperties(params (string name, object value)[] properties);

        #region --- Extensions ---

        /// <summary>
        /// Ghi log một thông điệp Information nếu điều kiện đúng.
        /// </summary>
        /// <param name="condition">Điều kiện để ghi log.</param>
        /// <param name="message">Nội dung thông điệp.</param>
        void LogInformationIf(bool condition, string message);

        /// <summary>
        /// Ghi log một thông điệp Warning nếu điều kiện đúng.
        /// </summary>
        /// <param name="condition">Điều kiện để ghi log.</param>
        /// <param name="message">Nội dung cảnh báo.</param>
        /// <param name="ex">Exception đi kèm (tùy chọn).</param>
        void LogWarningIf(bool condition, string message, Exception ex = null);
        
        /// <summary>
        /// Ghi log một thông điệp Error nếu điều kiện đúng.
        /// </summary>
        /// <param name="condition">Điều kiện để ghi log.</param>
        /// <param name="message">Nội dung lỗi.</param>
        /// <param name="ex">Exception đi kèm (tùy chọn).</param>
        void LogErrorIf(bool condition, string message, Exception ex = null);

        /// <summary>
        /// Bắt đầu đo thời gian thực thi của một thao tác và ghi log khi kết thúc.
        /// Phải được sử dụng trong một khối `using`.
        /// </summary>
        /// <param name="operationName">Tên của thao tác cần đo.</param>
        /// <returns>Một đối tượng IDisposable để tự động dừng đo và ghi log khi ra khỏi scope.</returns>
        IDisposable TimeOperation(string operationName);

        /// <summary>
        /// Bắt đầu đo thời gian thực thi và thêm các thuộc tính ngữ cảnh vào log.
        /// </summary>
        /// <param name="operationName">Tên của thao tác cần đo.</param>
        /// <param name="properties">Các thuộc tính sẽ được thêm vào context trong suốt thời gian đo.</param>
        /// <returns>Một đối tượng IDisposable để tự động dừng đo, xóa thuộc tính và ghi log khi ra khỏi scope.</returns>
        IDisposable TimeOperation(string operationName, params (string name, object value)[] properties);

        /// <summary>
        /// Ghi log một thông điệp Information kèm theo ngữ cảnh mã nguồn (tên file, tên phương thức, số dòng).
        /// </summary>
        /// <param name="message">Nội dung thông điệp.</param>
        /// <param name="memberName">Tên phương thức gọi (tự động gán).</param>
        /// <param name="sourceFilePath">Đường dẫn file gọi (tự động gán).</param>
        /// <param name="sourceLineNumber">Số dòng gọi (tự động gán).</param>
        void LogSourceInformation(string message, string memberName, string sourceFilePath, int sourceLineNumber);

        #endregion

        #region --- Elastic APM Extensions ---

        /// <summary>
        /// Bắt đầu một transaction mới trong Elastic APM.
        /// </summary>
        /// <param name="name">Tên transaction.</param>
        /// <param name="type">Loại transaction (web, db, messaging, etc.).</param>
        /// <returns>Transaction ID để theo dõi.</returns>
        string StartTransaction(string name, string type = "custom");

        /// <summary>
        /// Kết thúc transaction hiện tại.
        /// </summary>
        /// <param name="result">Kết quả transaction (success, error, etc.).</param>
        void EndTransaction(string result = "success");

        /// <summary>
        /// Bắt đầu một span mới trong transaction hiện tại.
        /// </summary>
        /// <param name="name">Tên span.</param>
        /// <param name="type">Loại span.</param>
        /// <param name="subtype">Loại con của span.</param>
        /// <param name="action">Hành động cụ thể.</param>
        /// <returns>Span ID để theo dõi.</returns>
        string StartSpan(string name, string type = "custom", string subtype = null, string action = null);

        /// <summary>
        /// Kết thúc span hiện tại.
        /// </summary>
        /// <param name="result">Kết quả span.</param>
        void EndSpan(string result = "success");

        /// <summary>
        /// Ghi một custom metric vào Elastic APM.
        /// </summary>
        /// <param name="name">Tên metric.</param>
        /// <param name="value">Giá trị metric.</param>
        /// <param name="unit">Đơn vị đo (count, percent, byte, etc.).</param>
        void RecordMetric(string name, double value, string unit = "count");

        /// <summary>
        /// Ghi một custom event vào Elastic APM.
        /// </summary>
        /// <param name="name">Tên event.</param>
        /// <param name="data">Dữ liệu event (tùy chọn).</param>
        void RecordEvent(string name, object data = null);

        /// <summary>
        /// Thiết lập correlation ID để liên kết các transaction/span.
        /// </summary>
        /// <param name="correlationId">ID để liên kết.</param>
        void SetCorrelationId(string correlationId);

        /// <summary>
        /// Thêm business metric để theo dõi KPI.
        /// </summary>
        /// <param name="name">Tên KPI.</param>
        /// <param name="value">Giá trị KPI.</param>
        /// <param name="category">Danh mục KPI (performance, business, etc.).</param>
        void RecordBusinessMetric(string name, double value, string category = "business");

        /// <summary>
        /// Thiết lập user context cho transaction hiện tại.
        /// </summary>
        /// <param name="userId">ID người dùng.</param>
        /// <param name="username">Tên người dùng.</param>
        /// <param name="email">Email người dùng.</param>
        void SetUserContext(string userId, string username = null, string email = null);

        /// <summary>
        /// Thiết lập custom context cho transaction hiện tại.
        /// </summary>
        /// <param name="key">Khóa context.</param>
        /// <param name="value">Giá trị context.</param>
        void SetCustomContext(string key, object value);

        #endregion
    }
} 