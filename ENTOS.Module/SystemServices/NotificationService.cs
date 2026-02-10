using DevExpress.ExpressApp;
using ENTOS.Module.Interfaces;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.SystemServices
{

    public abstract class NotificationService : INotificationService
    {
        protected XafApplication? _application;


        /// <summary>
        /// Gửi thông báo thông tin (Info) cho người dùng hiện tại.
        /// </summary>
        public void Notify(string caption, string message, Action? OkDelegate = null, Action? CancelDelegate = null)
            => ShowMessage(caption, message, InformationType.Info, OkDelegate, CancelDelegate);

        public void Notify(string caption, string message, object obj, int duration = 5000)
            => ShowMessage(caption, message, (InformationType)obj, null, null, duration);

        /// <summary>
        /// Gửi thông báo cảnh báo (Warning) cho người dùng hiện tại.
        /// </summary>
        public void NotifyWarning(string caption, string message, Action? OkDelegate = null, Action? CancelDelegate = null)
            => ShowMessage(caption, message, InformationType.Warning, OkDelegate, CancelDelegate);

        /// <summary>
        /// Gửi thông báo lỗi (Error) cho người dùng hiện tại.
        /// </summary>
        public void NotifyError(string caption, string message, Action? OkDelegate = null, Action? CancelDelegate = null)
            => ShowMessage(caption, message, InformationType.Error, OkDelegate, CancelDelegate);

        /// <summary>
        /// Gửi thông báo thành công (Success) cho người dùng hiện tại.
        /// </summary>
        public void NotifySuccess(string caption, string message, Action? OkDelegate = null, Action? CancelDelegate = null)
            => ShowMessage(caption, message, InformationType.Info, OkDelegate, CancelDelegate); // Có thể mở rộng nếu có NotificationLevel.Success

        /// <summary>
        /// Gửi thông báo cho nhiều người dùng.
        /// </summary>
        public void NotifyToUsers(IEnumerable<string> userIds, string caption, string message)
        {
            foreach (var userId in userIds)
            {
                NotifyWithData(userId, caption, message, null);
            }
        }

        /// <summary>
        /// Gửi thông báo với tiêu đề và dữ liệu đính kèm.
        /// </summary>
        public void NotifyWithData(string userId, string title, string message, object? data = null)
        {
            using (var objectSpace = _application?.CreateObjectSpace(typeof(UserNotifications)))
            {
                if (objectSpace is null)
                    return;
                var notification = objectSpace.CreateObject<UserNotifications>();
                notification.CurrentUserId = Guid.Parse(userId);
                notification.Subject = string.IsNullOrEmpty(title) ? message : title;
                notification.DueDate = DateTime.Now;
                notification.Readed = false;
                notification.ObjectType = data?.GetType().FullName;
                notification.ObjectId = data is null ? Guid.Empty : (data is Guid guid ? guid : Guid.Empty);
                objectSpace.CommitChanges();
            }
        }

        /// <summary>
        /// Gửi thông báo thông tin (Info) cho nhiều người dùng.
        /// </summary>
        public void NotifyInfoToUsers(IEnumerable<string> userIds, string caption, string message)
        {
            foreach (var userId in userIds)
            {
                NotifyWithData(userId, caption, message, null);
            }
        }

        /// <summary>
        /// Gửi thông báo thông tin (Info) với dữ liệu đính kèm.
        /// </summary>
        public void NotifyInfoWithData(string userId, string title, string message, object? data = null)
        {
            NotifyWithData(userId, title, message, data);
        }

        /// <summary>
        /// Lấy lịch sử thông báo của người dùng.
        /// </summary>
        public IEnumerable<UserNotifications> GetNotificationHistory(string userId, int take = 20)
        {
            using (var objectSpace = _application?.CreateObjectSpace(typeof(UserNotifications)))
            {
                var guid = Guid.Parse(userId);
                return objectSpace.GetObjectsQuery<UserNotifications>()
                    .Where(n => n.CurrentUserId == guid)
                    .OrderByDescending(n => n.DueDate)
                    .Take(take)
                    .ToList();

            }
        }

        /// <summary>
        /// Đánh dấu thông báo đã đọc.
        /// </summary>
        public void MarkAsRead(string userId, Guid notificationId)
        {
            using (var objectSpace = _application?.CreateObjectSpace(typeof(UserNotifications)))
            {
                var guid = Guid.Parse(userId);
                var notification = objectSpace.GetObjectsQuery<UserNotifications>()
                    .FirstOrDefault(n => n.Oid == notificationId && n.CurrentUserId == guid);
                if (notification != null)
                {
                    notification.Readed = true;
                    objectSpace.CommitChanges();
                }
            }
        }



        public void ShowMessage(string caption, string message, InformationType informationType, Action? OkDelegate = null, Action? CancelDelegate = null, int duration = 10000)
        {
            var messageOptions = new MessageOptions();
            messageOptions.Duration = duration;
            messageOptions.Message = message;
            messageOptions.Type = informationType;
            messageOptions.Web.Position = InformationPosition.Right;
            messageOptions.Win.Caption = caption;
            messageOptions.Win.Type = WinMessageType.Alert;
            if (OkDelegate != null)
                messageOptions.OkDelegate = OkDelegate;
            if (CancelDelegate != null)
                messageOptions.CancelDelegate = CancelDelegate;
            _application?.ShowViewStrategy.ShowMessage(messageOptions);
        }
    }
}
