namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface gửi thông báo cho người dùng.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Gửi thông báo.
        /// </summary>
        void Notify(string caption, string message, Action? OkDelegate = null, Action? CancelDelegate = null);

        void Notify(string caption, string message, object obj, int duration = 5000);
        /// <summary>
        /// Gửi thông báo cảnh báo (Warning).
        /// </summary>
        void NotifyWarning(string caption, string message, Action? OkDelegate = null, Action? CancelDelegate = null);

        /// <summary>
        /// Gửi thông báo lỗi (Error).
        /// </summary>
        void NotifyError(string caption, string message, Action? OkDelegate = null, Action? CancelDelegate = null);

        /// <summary>
        /// Gửi thông báo thành công (Success).
        /// </summary>
        void NotifySuccess(string caption, string message, Action? OkDelegate = null, Action? CancelDelegate = null);
        /// <summary>
        /// Gửi thông báo cho nhiều người dùng.
        /// </summary>
        void NotifyToUsers(IEnumerable<string> userIds, string caption, string message);

        /// <summary>
        /// Gửi thông báo với tiêu đề và dữ liệu đính kèm.
        /// </summary>
        void NotifyWithData(string userId, string title, string message, object? data = null);

        /// <summary>
        /// Gửi thông báo thông tin (Info).
        /// </summary>

        void NotifyInfoToUsers(IEnumerable<string> userIds, string caption, string message);
        void NotifyInfoWithData(string userId, string title, string message, object? data = null);

        /// <summary>
        /// Lấy lịch sử thông báo của người dùng.
        /// </summary>
        //IEnumerable<Module.SystemObjects.UserNotifications> GetNotificationHistory(string userId, int take = 20);

        /// <summary>
        /// Đánh dấu thông báo đã đọc.
        /// </summary>
        void MarkAsRead(string userId, Guid notificationId);
    }

    

}