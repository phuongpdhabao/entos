using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ENTOS.Module.Extensions
{
    /// <summary>
    /// Extension cho xử lý tài chính/kế toán: số tiền, hóa đơn, phiếu, hợp đồng, mã bút toán.
    /// </summary>
    public static class StringFinanceExtensions
    {
        /// <summary>
        /// Định dạng số tiền theo chuẩn ERP Việt Nam.
        /// </summary>
        public static string ToERPAmountString(this decimal amount)
        {
            return amount.ToString("#,##0", new CultureInfo("vi-VN"));
        }

        /// <summary>
        /// Định dạng số dư, số phát sinh theo chuẩn ERP.
        /// </summary>
        public static string ToERPBalanceString(this decimal amount)
        {
            return amount.ToString("#,##0.00", new CultureInfo("vi-VN"));
        }

        /// <summary>
        /// Chuyển số thành chữ tiếng Việt (placeholder).
        /// </summary>
        public static string ToVietnameseWords(this decimal number)
        {
            // Placeholder: cần logic chuyển số thành chữ tiếng Việt
            return number.ToString();
        }

        /// <summary>
        /// Trích xuất số hóa đơn trong chuỗi.
        /// </summary>
        public static List<string> ExtractInvoiceNumbers(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"\b\d{7,12}\b").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Trích xuất số phiếu trong chuỗi.
        /// </summary>
        public static List<string> ExtractVoucherNumbers(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"PN\d{6,}|PX\d{6,}|PT\d{6,}|PC\d{6,}").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Trích xuất số hợp đồng trong chuỗi.
        /// </summary>
        public static List<string> ExtractContractNumbers(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"HD\d{6,}").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Kiểm tra định dạng mã hợp đồng.
        /// </summary>
        public static bool IsValidContractCode(this string text)
        {
            return Regex.IsMatch(text, @"^HD\d{6,}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Sinh mã bút toán tự động.
        /// </summary>
        public static string GenerateJournalEntryCode(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D7}";
        }
    }
} 