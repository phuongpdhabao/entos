using System.Text.RegularExpressions;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper kiểm tra, validate dữ liệu thường dùng.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Kiểm tra chuỗi có khớp với pattern Regex không.
        /// </summary>
        public static bool Validate(string text, string pattern)
        {
            if (text == null || pattern == null)
                return false;
            return Regex.IsMatch(text, pattern);
        }

        /// <summary>
        /// Kiểm tra email hợp lệ.
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        /// <summary>
        /// Kiểm tra số điện thoại hợp lệ (Việt Nam).
        /// </summary>
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            return Regex.IsMatch(phone, @"^(0|\+84)[0-9]{9,10}$");
        }
    }
} 