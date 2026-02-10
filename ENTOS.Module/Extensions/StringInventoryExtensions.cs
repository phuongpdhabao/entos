using System;
using System.Text.RegularExpressions;

namespace ENTOS.Module.Extensions
{
    /// <summary>
    /// Extension cho xử lý kho/vật tư: mã hàng hóa, mã kho, mã lô, mã vị trí.
    /// </summary>
    public static class StringInventoryExtensions
    {
        /// <summary>
        /// Sinh mã hàng hóa tự động.
        /// </summary>
        public static string GenerateProductCode(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D8}";
        }

        /// <summary>
        /// Kiểm tra định dạng mã kho.
        /// </summary>
        public static bool IsValidWarehouseCode(this string text)
        {
            return Regex.IsMatch(text, @"^KHO\d{3,}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Kiểm tra định dạng mã lô hàng.
        /// </summary>
        public static bool IsValidBatchCode(this string text)
        {
            return Regex.IsMatch(text, @"^LO\d{4,}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Kiểm tra định dạng mã vị trí kho.
        /// </summary>
        public static bool IsValidLocationCode(this string text)
        {
            return Regex.IsMatch(text, @"^VT\d{4,}$", RegexOptions.Compiled);
        }
    }
} 