// using System.Windows.Forms;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper gửi thông báo đơn giản (MessageBox, Console).
    /// </summary>
    public static class NotificationHelper
    {
        /// <summary>
        /// Gửi thông báo ra console.
        /// </summary>
        public static void NotifyConsole(string message)
        {
            Console.WriteLine($"[NOTIFY] {message}");
        }

        /// <summary>
        /// Gửi thông báo popup (chỉ dùng được với WinForms).
        /// </summary>
        public static void NotifyPopup(string message, string title = "Thông báo")
        {
            // MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
} 