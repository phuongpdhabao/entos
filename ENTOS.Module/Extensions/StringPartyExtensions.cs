using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ENTOS.Module.Extensions
{
    /// <summary>
    /// Extension cho xử lý các đối tượng nghiệp vụ (con người, tổ chức): cá nhân, khách hàng, nhân viên, đối tác.
    /// Bao gồm: chuẩn hóa, kiểm tra, trích xuất, sinh mã.
    /// </summary>
    public static class StringPartyExtensions
    {
        // --- Personal Info ---

        /// <summary>
        /// Chuẩn hóa tên (viết hoa chữ cái đầu, loại bỏ khoảng trắng thừa).
        /// </summary>
        public static string NormalizePersonName(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            var words = text.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words.Select(w => char.ToUpper(w[0]) + w.Substring(1)));
        }

        /// <summary>
        /// Chuẩn hóa địa chỉ (loại bỏ khoảng trắng thừa, viết hoa chữ cái đầu mỗi từ).
        /// </summary>
        public static string NormalizePersonAddress(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            var words = text.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words.Select(w => char.ToUpper(w[0]) + w.Substring(1)));
        }

        /// <summary>
        /// Kiểm tra số điện thoại Việt Nam hợp lệ.
        /// </summary>
        public static bool IsValidPersonalPhone(this string text)
        {
            return Regex.IsMatch(text, @"^(0|\+84)[0-9]{9,10}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Trích xuất số điện thoại Việt Nam trong chuỗi.
        /// </summary>
        public static List<string> ExtractPersonalPhones(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"(0|\+84)[0-9]{9,10}").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Kiểm tra email hợp lệ.
        /// </summary>
        public static bool IsValidPersonalEmail(this string text)
        {
            return Regex.IsMatch(text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Trích xuất email trong chuỗi.
        /// </summary>
        public static List<string> ExtractPersonalEmails(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"[^@\s]+@[^@\s]+\.[^@\s]+", RegexOptions.Compiled).Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Kiểm tra số CMND/CCCD hợp lệ.
        /// </summary>
        public static bool IsValidIdCard(this string text)
        {
            return Regex.IsMatch(text, @"^\d{9}$|^\d{12}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Trích xuất số CMND/CCCD trong chuỗi.
        /// </summary>
        public static List<string> ExtractIdCards(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"\b\d{9}\b|\b\d{12}\b", RegexOptions.Compiled).Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Trích xuất ngày sinh (dd/MM/yyyy hoặc yyyy-MM-dd) trong chuỗi.
        /// </summary>
        public static List<string> ExtractBirthDates(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            var list = new List<string>();
            list.AddRange(Regex.Matches(text, @"\b\d{2}/\d{2}/\d{4}\b", RegexOptions.Compiled).Cast<Match>().Select(m => m.Value));
            list.AddRange(Regex.Matches(text, @"\b\d{4}-\d{2}-\d{2}\b", RegexOptions.Compiled).Cast<Match>().Select(m => m.Value));
            return list;
        }

        /// <summary>
        /// Kiểm tra giới tính từ chuỗi (nam/nữ/khác).
        /// </summary>
        public static string DetectGender(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "khác";
            var lower = text.ToLower();
            if (lower.Contains("nam")) return "nam";
            if (lower.Contains("nữ") || lower.Contains("nu")) return "nữ";
            return "khác";
        }

        // --- Customer, Partner, Supplier Info ---

        /// <summary>
        /// Sinh mã khách hàng tự động.
        /// </summary>
        public static string GenerateCustomerCode(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D6}";
        }

        /// <summary>
        /// Sinh mã đối tác tự động.
        /// </summary>
        public static string GeneratePartnerCode(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D6}";
        }

        /// <summary>
        /// Sinh mã nhà cung cấp tự động.
        /// </summary>
        public static string GenerateSupplierCode(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D6}";
        }

        /// <summary>
        /// Kiểm tra mã số thuế Việt Nam hợp lệ.
        /// </summary>
        public static bool IsValidTaxCode(this string text)
        {
            return Regex.IsMatch(text, @"^\d{10}(-\d{3})?$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Trích xuất mã số thuế trong chuỗi.
        /// </summary>
        public static List<string> ExtractTaxCodes(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"\b\d{10}\b|\b\d{10}-\d{3}\b", RegexOptions.Compiled).Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Kiểm tra hợp lệ số tài khoản ngân hàng Việt Nam (8-16 số).
        /// </summary>
        public static bool IsValidBankAccountNumber(this string text)
        {
            return Regex.IsMatch(text, @"^\d{8,16}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Trích xuất số tài khoản ngân hàng trong chuỗi.
        /// </summary>
        public static List<string> ExtractBankAccountNumbers(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"\b\d{8,16}\b", RegexOptions.Compiled).Cast<Match>().Select(m => m.Value).ToList();
        }

        // --- Employee Info ---

        /// <summary>
        /// Sinh mã nhân viên tự động.
        /// </summary>
        public static string GenerateEmployeeCode(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D5}";
        }

        /// <summary>
        /// Kiểm tra định dạng mã nhân viên.
        /// </summary>
        public static bool IsValidEmployeeCode(this string text)
        {
            return Regex.IsMatch(text, @"^NV\d{5,}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Kiểm tra định dạng mã phòng ban.
        /// </summary>
        public static bool IsValidDepartmentCode(this string text)
        {
            return Regex.IsMatch(text, @"^PB\d{3,}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Kiểm tra định dạng mã hợp đồng lao động.
        /// </summary>
        public static bool IsValidLaborContractCode(this string text)
        {
            return Regex.IsMatch(text, @"^HDLD\d{4,}$", RegexOptions.Compiled);
        }
    }
} 