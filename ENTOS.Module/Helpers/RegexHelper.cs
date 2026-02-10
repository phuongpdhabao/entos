using System.Text.RegularExpressions;
using System.IO;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper class cung cấp các pattern regex thường dùng và phương thức validation
    /// Bao gồm các pattern cho email, phone, URL, và các validation khác
    /// 
    /// Ví dụ sử dụng:
    /// - RegexHelper.IsValidEmail("test@example.com") // returns true
    /// - RegexHelper.ExtractNumbers("Price: $123.45") // returns "123.45"
    /// - RegexHelper.IsValidVietnamesePhone("0987654321") // returns true
    /// </summary>
    public static class RegexHelper
    {
        #region Common Patterns

        /// <summary>
        /// Pattern cho email chuẩn RFC 5322
        /// Ví dụ: user@domain.com, test.email+tag@example.org
        /// </summary>
        public static readonly string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        /// <summary>
        /// Pattern cho số điện thoại Việt Nam (10-11 số)
        /// Ví dụ: 0987654321, +84987654321, 84987654321
        /// </summary>
        public static readonly string VietnamesePhonePattern = @"^(\+84|84|0)(3[2-9]|5[689]|7[06-9]|8[1-689]|9[0-46-9])[0-9]{7}$";

        /// <summary>
        /// Pattern cho URL (http/https)
        /// Ví dụ: https://example.com, http://sub.domain.com/path
        /// </summary>
        public static readonly string UrlPattern = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";

        /// <summary>
        /// Pattern cho chỉ số (integers và decimals)
        /// Ví dụ: 123, -456, 78.90, +12.34
        /// </summary>
        public static readonly string NumberPattern = @"^[+-]?\d+(\.\d+)?$";

        /// <summary>
        /// Pattern cho mã bưu chính Việt Nam (6 chữ số)
        /// Ví dụ: 100000, 700000, 123456
        /// </summary>
        public static readonly string VietnamPostalCodePattern = @"^[0-9]{6}$";

        /// <summary>
        /// Pattern cho CCCD/CMND Việt Nam (9 hoặc 12 số)
        /// Ví dụ: 123456789, 123456789012
        /// </summary>
        public static readonly string VietnamIdCardPattern = @"^[0-9]{9}([0-9]{3})?$";

        /// <summary>
        /// Pattern cho IPv4 address
        /// Ví dụ: 192.168.1.1, 10.0.0.1, 127.0.0.1
        /// </summary>
        public static readonly string IPv4Pattern = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";

        /// <summary>
        /// Pattern cho IPv6 address
        /// Ví dụ: 2001:0db8:85a3:0000:0000:8a2e:0370:7334
        /// </summary>
        public static readonly string IPv6Pattern = @"^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$";

        /// <summary>
        /// Pattern cho MAC address
        /// Ví dụ: 00:1B:44:11:3A:B7, 00-1B-44-11-3A-B7
        /// </summary>
        public static readonly string MacAddressPattern = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";

        /// <summary>
        /// Pattern cho credit card number (các loại chính)
        /// Ví dụ: 4111111111111111 (Visa), 5555555555554444 (MasterCard)
        /// </summary>
        public static readonly string CreditCardPattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[47][0-9]{13}|3[0-9]{13}|6(?:011|5[0-9]{2})[0-9]{12})$";

        /// <summary>
        /// Pattern cho hexadecimal color code
        /// Ví dụ: #FF0000, #ff0000, #F00
        /// </summary>
        public static readonly string HexColorPattern = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";

        /// <summary>
        /// Pattern cho HTML tags
        /// Ví dụ: <div>, </div>, <img src="..."/>
        /// </summary>
        public static readonly string HtmlTagPattern = @"<[^>]+>";

        /// <summary>
        /// Pattern cho base64 string
        /// Ví dụ: SGVsbG8gV29ybGQ=
        /// </summary>
        public static readonly string Base64Pattern = @"^[A-Za-z0-9+/]*={0,2}$";

        #endregion

        #region Email Validation

        /// <summary>
        /// Kiểm tra email có hợp lệ không
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>True nếu email hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidEmail("user@example.com"); // returns true
        /// </example>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email, EmailPattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Trích xuất tất cả email từ text
        /// </summary>
        /// <param name="text">Text chứa email</param>
        /// <returns>Danh sách email tìm được</returns>
        /// <example>
        /// var emails = RegexHelper.ExtractEmails("Contact: john@test.com or admin@site.org");
        /// // returns ["john@test.com", "admin@site.org"]
        /// </example>
        public static List<string> ExtractEmails(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            var matches = Regex.Matches(text, EmailPattern, RegexOptions.IgnoreCase);
            return matches.Cast<Match>().Select(m => m.Value).ToList();
        }

        #endregion

        #region Phone Validation

        /// <summary>
        /// Kiểm tra số điện thoại Việt Nam có hợp lệ không
        /// </summary>
        /// <param name="phone">Số điện thoại cần kiểm tra</param>
        /// <returns>True nếu số điện thoại hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidVietnamesePhone("0987654321"); // returns true
        /// </example>
        public static bool IsValidVietnamesePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Loại bỏ spaces và dashes
            var cleanPhone = phone.Replace(" ", "").Replace("-", "");
            return Regex.IsMatch(cleanPhone, VietnamesePhonePattern);
        }

        /// <summary>
        /// Chuẩn hóa số điện thoại Việt Nam về dạng +84xxxxxxxxx
        /// </summary>
        /// <param name="phone">Số điện thoại cần chuẩn hóa</param>
        /// <returns>Số điện thoại đã chuẩn hóa</returns>
        /// <example>
        /// string normalized = RegexHelper.NormalizeVietnamesePhone("0987654321");
        /// // returns "+84987654321"
        /// </example>
        public static string NormalizeVietnamesePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return string.Empty;

            var cleanPhone = phone.Replace(" ", "").Replace("-", "");

            if (cleanPhone.StartsWith("+84"))
                return cleanPhone;

            if (cleanPhone.StartsWith("84"))
                return "+" + cleanPhone;

            if (cleanPhone.StartsWith("0"))
                return "+84" + cleanPhone.Substring(1);

            return phone; // Trả về nguyên bản nếu không nhận dạng được
        }

        #endregion

        #region URL Validation

        /// <summary>
        /// Kiểm tra URL có hợp lệ không
        /// </summary>
        /// <param name="url">URL cần kiểm tra</param>
        /// <returns>True nếu URL hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidUrl("https://example.com"); // returns true
        /// </example>
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Regex.IsMatch(url, UrlPattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Trích xuất tất cả URL từ text
        /// </summary>
        /// <param name="text">Text chứa URL</param>
        /// <returns>Danh sách URL tìm được</returns>
        /// <example>
        /// var urls = RegexHelper.ExtractUrls("Visit https://example.com or http://test.org");
        /// // returns ["https://example.com", "http://test.org"]
        /// </example>
        public static List<string> ExtractUrls(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            var matches = Regex.Matches(text, UrlPattern, RegexOptions.IgnoreCase);
            return matches.Cast<Match>().Select(m => m.Value).ToList();
        }

        #endregion

        #region Number Operations

        /// <summary>
        /// Kiểm tra chuỗi có phải là số không
        /// </summary>
        /// <param name="input">Chuỗi cần kiểm tra</param>
        /// <returns>True nếu là số hợp lệ</returns>
        /// <example>
        /// bool isNumber = RegexHelper.IsNumber("123.45"); // returns true
        /// </example>
        public static bool IsNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return Regex.IsMatch(input, NumberPattern);
        }

        /// <summary>
        /// Trích xuất tất cả số từ text
        /// </summary>
        /// <param name="text">Text chứa số</param>
        /// <returns>Danh sách số tìm được</returns>
        /// <example>
        /// var numbers = RegexHelper.ExtractNumbers("Price: $123.45, Quantity: 10");
        /// // returns ["123.45", "10"]
        /// </example>
        public static List<string> ExtractNumbers(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            var matches = Regex.Matches(text, @"[+-]?\d+(\.\d+)?");
            return matches.Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Chỉ giữ lại ký tự số trong chuỗi
        /// </summary>
        /// <param name="input">Chuỗi đầu vào</param>
        /// <returns>Chuỗi chỉ chứa số</returns>
        /// <example>
        /// string digitsOnly = RegexHelper.ExtractDigitsOnly("abc123def456");
        /// // returns "123456"
        /// </example>
        public static string ExtractDigitsOnly(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return Regex.Replace(input, @"[^\d]", "");
        }

        #endregion

        #region Vietnamese Specific

        /// <summary>
        /// Kiểm tra mã bưu chính Việt Nam có hợp lệ không
        /// </summary>
        /// <param name="postalCode">Mã bưu chính</param>
        /// <returns>True nếu mã bưu chính hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidVietnamPostalCode("100000"); // returns true
        /// </example>
        public static bool IsValidVietnamPostalCode(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                return false;

            return Regex.IsMatch(postalCode, VietnamPostalCodePattern);
        }

        /// <summary>
        /// Kiểm tra CCCD/CMND Việt Nam có hợp lệ không
        /// </summary>
        /// <param name="idCard">Số CCCD/CMND</param>
        /// <returns>True nếu số CCCD/CMND hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidVietnamIdCard("123456789012"); // returns true
        /// </example>
        public static bool IsValidVietnamIdCard(string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
                return false;

            var cleanId = idCard.Replace(" ", "").Replace("-", "");
            return Regex.IsMatch(cleanId, VietnamIdCardPattern);
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt từ chuỗi
        /// </summary>
        /// <param name="input">Chuỗi có dấu</param>
        /// <returns>Chuỗi không dấu</returns>
        /// <example>
        /// string normalized = RegexHelper.RemoveVietnameseDiacritics("Tiếng Việt");
        /// // returns "Tieng Viet"
        /// </example>
        public static string RemoveVietnameseDiacritics(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var replacements = new Dictionary<string, string>
            {
                {"[àáạảãâầấậẩẫăằắặẳẵ]", "a"},
                {"[èéẹẻẽêềếệểễ]", "e"},
                {"[ìíịỉĩ]", "i"},
                {"[òóọỏõôồốộổỗơờớợởỡ]", "o"},
                {"[ùúụủũưừứựửữ]", "u"},
                {"[ỳýỵỷỹ]", "y"},
                {"[đ]", "d"},
                {"[ÀÁẠẢÃÂẦẤẬẨẪĂẰẮẶẲẴ]", "A"},
                {"[ÈÉẸẺẼÊỀẾỆỂỄ]", "E"},
                {"[ÌÍỊỈĨ]", "I"},
                {"[ÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠ]", "O"},
                {"[ÙÚỤỦŨƯỪỨỰỬỮ]", "U"},
                {"[ỲÝỴỶỸ]", "Y"},
                {"[Đ]", "D"}
            };

            string result = input;
            foreach (var replacement in replacements)
            {
                result = Regex.Replace(result, replacement.Key, replacement.Value);
            }

            return result;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Thay thế nhiều spaces liên tiếp bằng 1 space
        /// </summary>
        /// <param name="input">Chuỗi đầu vào</param>
        /// <returns>Chuỗi đã được chuẩn hóa spaces</returns>
        /// <example>
        /// string normalized = RegexHelper.NormalizeSpaces("Hello    World   !!");
        /// // returns "Hello World !!"
        /// </example>
        public static string NormalizeSpaces(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return Regex.Replace(input.Trim(), @"\s+", " ");
        }

        /// <summary>
        /// Kiểm tra chuỗi có chứa ký tự đặc biệt không
        /// </summary>
        /// <param name="input">Chuỗi cần kiểm tra</param>
        /// <returns>True nếu chứa ký tự đặc biệt</returns>
        /// <example>
        /// bool hasSpecial = RegexHelper.ContainsSpecialCharacters("Hello@World");
        /// // returns true
        /// </example>
        public static bool ContainsSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return Regex.IsMatch(input, @"[^a-zA-Z0-9\s]");
        }

        /// <summary>
        /// Kiểm tra password có đủ mạnh không (8+ ký tự, có chữ hoa, chữ thường, số, ký tự đặc biệt)
        /// </summary>
        /// <param name="password">Password cần kiểm tra</param>
        /// <returns>True nếu password đủ mạnh</returns>
        /// <example>
        /// bool isStrong = RegexHelper.IsStrongPassword("MyPass123!");
        /// // returns true
        /// </example>
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Ít nhất 8 ký tự
            if (password.Length < 8)
                return false;

            // Có chữ hoa
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            // Có chữ thường
            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;

            // Có số
            if (!Regex.IsMatch(password, @"[0-9]"))
                return false;

            // Có ký tự đặc biệt
            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
                return false;

            return true;
        }

        /// <summary>
        /// Tạo slug từ chuỗi (dùng cho URL friendly)
        /// </summary>
        /// <param name="input">Chuỗi đầu vào</param>
        /// <returns>Slug string</returns>
        /// <example>
        /// string slug = RegexHelper.CreateSlug("Bài viết về .NET Core");
        /// // returns "bai-viet-ve-net-core"
        /// </example>
        public static string CreateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Loại bỏ dấu tiếng Việt
            string slug = RemoveVietnameseDiacritics(input);

            // Chuyển về lowercase
            slug = slug.ToLowerInvariant();

            // Thay thế ký tự không phải chữ cái/số bằng dấu gạch ngang
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Thay thế nhiều spaces/dashes liên tiếp bằng 1 dash
            slug = Regex.Replace(slug, @"[\s-]+", "-");

            // Loại bỏ dash ở đầu/cuối
            slug = slug.Trim('-');

            return slug;
        }

        #endregion

        #region Network & IP Validation

        /// <summary>
        /// Kiểm tra IPv4 address có hợp lệ không
        /// </summary>
        /// <param name="ip">IP address cần kiểm tra</param>
        /// <returns>True nếu IPv4 hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidIPv4("192.168.1.1"); // returns true
        /// </example>
        public static bool IsValidIPv4(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return false;

            return Regex.IsMatch(ip, IPv4Pattern);
        }

        /// <summary>
        /// Kiểm tra IPv6 address có hợp lệ không
        /// </summary>
        /// <param name="ip">IPv6 address cần kiểm tra</param>
        /// <returns>True nếu IPv6 hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidIPv6("2001:0db8:85a3::8a2e:0370:7334"); // returns true
        /// </example>
        public static bool IsValidIPv6(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return false;

            return Regex.IsMatch(ip, IPv6Pattern);
        }

        /// <summary>
        /// Kiểm tra MAC address có hợp lệ không
        /// </summary>
        /// <param name="mac">MAC address cần kiểm tra</param>
        /// <returns>True nếu MAC address hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidMacAddress("00:1B:44:11:3A:B7"); // returns true
        /// </example>
        public static bool IsValidMacAddress(string mac)
        {
            if (string.IsNullOrWhiteSpace(mac))
                return false;

            return Regex.IsMatch(mac, MacAddressPattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Trích xuất tất cả IP addresses từ text
        /// </summary>
        /// <param name="text">Text chứa IP addresses</param>
        /// <returns>Danh sách IP addresses tìm được</returns>
        /// <example>
        /// var ips = RegexHelper.ExtractIPAddresses("Server: 192.168.1.1, DNS: 8.8.8.8");
        /// // returns ["192.168.1.1", "8.8.8.8"]
        /// </example>
        public static List<string> ExtractIPAddresses(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            var matches = Regex.Matches(text, IPv4Pattern);
            return matches.Cast<Match>().Select(m => m.Value).ToList();
        }

        #endregion

        #region Security & Validation

        /// <summary>
        /// Kiểm tra credit card number có hợp lệ không (format check only)
        /// </summary>
        /// <param name="cardNumber">Số thẻ tín dụng</param>
        /// <returns>True nếu format hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidCreditCard("4111111111111111"); // returns true
        /// </example>
        public static bool IsValidCreditCard(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            // Loại bỏ spaces và dashes
            var cleanCard = cardNumber.Replace(" ", "").Replace("-", "");
            return Regex.IsMatch(cleanCard, CreditCardPattern);
        }

        /// <summary>
        /// Xác định loại thẻ tín dụng
        /// </summary>
        /// <param name="cardNumber">Số thẻ tín dụng</param>
        /// <returns>Loại thẻ (Visa, MasterCard, AmEx, etc.)</returns>
        /// <example>
        /// string cardType = RegexHelper.GetCreditCardType("4111111111111111"); // returns "Visa"
        /// </example>
        public static string GetCreditCardType(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return "Unknown";

            var cleanCard = cardNumber.Replace(" ", "").Replace("-", "");

            if (Regex.IsMatch(cleanCard, @"^4[0-9]{12}(?:[0-9]{3})?$"))
                return "Visa";

            if (Regex.IsMatch(cleanCard, @"^5[1-5][0-9]{14}$"))
                return "MasterCard";

            if (Regex.IsMatch(cleanCard, @"^3[47][0-9]{13}$"))
                return "American Express";

            if (Regex.IsMatch(cleanCard, @"^6(?:011|5[0-9]{2})[0-9]{12}$"))
                return "Discover";

            return "Unknown";
        }

        /// <summary>
        /// Mask credit card number (chỉ hiện 4 số cuối)
        /// </summary>
        /// <param name="cardNumber">Số thẻ tín dụng</param>
        /// <returns>Số thẻ đã được mask</returns>
        /// <example>
        /// string masked = RegexHelper.MaskCreditCard("4111111111111111"); // returns "****-****-****-1111"
        /// </example>
        public static string MaskCreditCard(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return string.Empty;

            var cleanCard = cardNumber.Replace(" ", "").Replace("-", "");

            if (cleanCard.Length < 4)
                return cardNumber;

            var lastFour = cleanCard.Substring(cleanCard.Length - 4);
            return $"****-****-****-{lastFour}";
        }

        /// <summary>
        /// Kiểm tra có chứa SQL injection patterns không
        /// </summary>
        /// <param name="input">Input string cần kiểm tra</param>
        /// <returns>True nếu có khả năng SQL injection</returns>
        /// <example>
        /// bool hasSqlInjection = RegexHelper.ContainsSqlInjection("'; DROP TABLE users; --"); // returns true
        /// </example>
        public static bool ContainsSqlInjection(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            var sqlInjectionPatterns = new[]
            {
                @"(\b(SELECT|INSERT|UPDATE|DELETE|DROP|CREATE|ALTER|EXEC|EXECUTE)\b)",
                @"(\b(UNION|OR|AND)\b.*\b(SELECT|INSERT|UPDATE|DELETE)\b)",
                @"(--|#|/\*|\*/)",
                @"(\b(SCRIPT|JAVASCRIPT|VBSCRIPT)\b)",
                @"(\<\s*script\b[^<]*(?:(?!\<\/script\>)<[^<]*)*\<\/script\>)"
            };

            return sqlInjectionPatterns.Any(pattern =>
                Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase));
        }

        #endregion

        #region File & Format Validation

        /// <summary>
        /// Kiểm tra hex color code có hợp lệ không
        /// </summary>
        /// <param name="color">Hex color code</param>
        /// <returns>True nếu hex color hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidHexColor("#FF0000"); // returns true
        /// </example>
        public static bool IsValidHexColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
                return false;

            return Regex.IsMatch(color, HexColorPattern);
        }

        /// <summary>
        /// Kiểm tra Base64 string có hợp lệ không
        /// </summary>
        /// <param name="base64">Base64 string</param>
        /// <returns>True nếu Base64 hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidBase64("SGVsbG8gV29ybGQ="); // returns true
        /// </example>
        public static bool IsValidBase64(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return false;

            // Base64 length phải chia hết cho 4
            if (base64.Length % 4 != 0)
                return false;

            return Regex.IsMatch(base64, Base64Pattern);
        }

        /// <summary>
        /// Loại bỏ tất cả HTML tags từ chuỗi
        /// </summary>
        /// <param name="html">HTML string</param>
        /// <returns>Plain text không có HTML tags</returns>
        /// <example>
        /// string plain = RegexHelper.StripHtmlTags("<p>Hello <b>World</b></p>"); // returns "Hello World"
        /// </example>
        public static string StripHtmlTags(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            return Regex.Replace(html, HtmlTagPattern, "");
        }

        /// <summary>
        /// Trích xuất tất cả HTML tags từ chuỗi
        /// </summary>
        /// <param name="html">HTML string</param>
        /// <returns>Danh sách HTML tags</returns>
        /// <example>
        /// var tags = RegexHelper.ExtractHtmlTags("<p>Hello <b>World</b></p>");
        /// // returns ["<p>", "<b>", "</b>", "</p>"]
        /// </example>
        public static List<string> ExtractHtmlTags(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return new List<string>();

            var matches = Regex.Matches(html, HtmlTagPattern);
            return matches.Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Kiểm tra file extension có hợp lệ không
        /// </summary>
        /// <param name="fileName">Tên file</param>
        /// <param name="allowedExtensions">Danh sách extension được phép (vd: jpg,png,gif)</param>
        /// <returns>True nếu extension hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidFileExtension("image.jpg", "jpg,png,gif"); // returns true
        /// </example>
        public static bool IsValidFileExtension(string fileName, string allowedExtensions)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(allowedExtensions))
                return false;

            var extensions = allowedExtensions.Split(',').Select(ext => ext.Trim().ToLowerInvariant());
            var fileExt = Path.GetExtension(fileName)?.TrimStart('.').ToLowerInvariant();

            return extensions.Contains(fileExt);
        }

        #endregion

        #region Text Processing

        /// <summary>
        /// Kiểm tra chuỗi có chứa emoji không
        /// </summary>
        /// <param name="text">Text cần kiểm tra</param>
        /// <returns>True nếu có emoji</returns>
        /// <example>
        /// bool hasEmoji = RegexHelper.ContainsEmoji("Hello 😊 World"); // returns true
        /// </example>
        public static bool ContainsEmoji(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            // Unicode ranges for common emoji
            var emojiPattern = @"[\u{1F600}-\u{1F64F}]|[\u{1F300}-\u{1F5FF}]|[\u{1F680}-\u{1F6FF}]|[\u{1F1E0}-\u{1F1FF}]|[\u{2600}-\u{26FF}]|[\u{2700}-\u{27BF}]";
            return Regex.IsMatch(text, emojiPattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Loại bỏ emoji từ chuỗi
        /// </summary>
        /// <param name="text">Text chứa emoji</param>
        /// <returns>Text không có emoji</returns>
        /// <example>
        /// string clean = RegexHelper.RemoveEmojis("Hello 😊 World 🌍"); // returns "Hello  World "
        /// </example>
        public static string RemoveEmojis(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var emojiPattern = @"[\u{1F600}-\u{1F64F}]|[\u{1F300}-\u{1F5FF}]|[\u{1F680}-\u{1F6FF}]|[\u{1F1E0}-\u{1F1FF}]|[\u{2600}-\u{26FF}]|[\u{2700}-\u{27BF}]";
            return Regex.Replace(text, emojiPattern, "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Chuẩn hóa tên người (viết hoa chữ cái đầu mỗi từ)
        /// </summary>
        /// <param name="name">Tên cần chuẩn hóa</param>
        /// <returns>Tên đã chuẩn hóa</returns>
        /// <example>
        /// string formatted = RegexHelper.FormatPersonName("nguyễn văn a"); // returns "Nguyễn Văn A"
        /// </example>
        public static string FormatPersonName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            // Chuẩn hóa spaces trước
            var normalized = NormalizeSpaces(name);

            // Viết hoa chữ cái đầu mỗi từ
            return Regex.Replace(normalized.ToLowerInvariant(), @"\b\w",
                match => match.Value.ToUpperInvariant());
        }

        /// <summary>
        /// Tạo initials từ tên đầy đủ
        /// </summary>
        /// <param name="fullName">Tên đầy đủ</param>
        /// <returns>Initials (vd: "John Doe" -> "JD")</returns>
        /// <example>
        /// string initials = RegexHelper.GetInitials("Nguyễn Văn An"); // returns "NVA"
        /// </example>
        public static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;

            var words = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(words.Select(word => word[0].ToString().ToUpperInvariant()));
        }

        /// <summary>
        /// Đếm số từ trong chuỗi
        /// </summary>
        /// <param name="text">Text cần đếm</param>
        /// <returns>Số từ</returns>
        /// <example>
        /// int wordCount = RegexHelper.CountWords("Hello world, how are you?"); // returns 5
        /// </example>
        public static int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            var matches = Regex.Matches(text, @"\b\w+\b");
            return matches.Count;
        }

        /// <summary>
        /// Tìm từ dài nhất trong chuỗi
        /// </summary>
        /// <param name="text">Text cần tìm</param>
        /// <returns>Từ dài nhất</returns>
        /// <example>
        /// string longest = RegexHelper.FindLongestWord("Hello beautiful world"); // returns "beautiful"
        /// </example>
        public static string FindLongestWord(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var matches = Regex.Matches(text, @"\b\w+\b");
            return matches.Cast<Match>()
                         .OrderByDescending(m => m.Value.Length)
                         .FirstOrDefault()?.Value ?? string.Empty;
        }

        #endregion

        #region Date & Time

        /// <summary>
        /// Kiểm tra định dạng ngày DD/MM/YYYY có hợp lệ không
        /// </summary>
        /// <param name="date">Chuỗi ngày tháng</param>
        /// <returns>True nếu định dạng hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidDateFormat("31/12/2023"); // returns true
        /// </example>
        public static bool IsValidDateFormat(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return false;

            var datePattern = @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$";
            return Regex.IsMatch(date, datePattern);
        }

        /// <summary>
        /// Kiểm tra định dạng thời gian HH:MM có hợp lệ không
        /// </summary>
        /// <param name="time">Chuỗi thời gian</param>
        /// <returns>True nếu định dạng hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidTimeFormat("14:30"); // returns true
        /// </example>
        public static bool IsValidTimeFormat(string time)
        {
            if (string.IsNullOrWhiteSpace(time))
                return false;

            var timePattern = @"^([01]?[0-9]|2[0-3]):[0-5][0-9]$";
            return Regex.IsMatch(time, timePattern);
        }

        /// <summary>
        /// Trích xuất tất cả ngày tháng từ text
        /// </summary>
        /// <param name="text">Text chứa ngày tháng</param>
        /// <returns>Danh sách ngày tháng tìm được</returns>
        /// <example>
        /// var dates = RegexHelper.ExtractDates("Meeting on 31/12/2023 and 01/01/2024");
        /// // returns ["31/12/2023", "01/01/2024"]
        /// </example>
        public static List<string> ExtractDates(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            var datePattern = @"\b(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}\b";
            var matches = Regex.Matches(text, datePattern);
            return matches.Cast<Match>().Select(m => m.Value).ToList();
        }

        #endregion

        #region Vietnamese Extended

        /// <summary>
        /// Kiểm tra mã tỉnh thành Việt Nam (2 chữ số đầu của mã bưu chính)
        /// </summary>
        /// <param name="provinceCode">Mã tỉnh thành</param>
        /// <returns>True nếu mã tỉnh thành hợp lệ</returns>
        /// <example>
        /// bool isValid = RegexHelper.IsValidVietnamProvinceCode("10"); // returns true (Hà Nội)
        /// </example>
        public static bool IsValidVietnamProvinceCode(string provinceCode)
        {
            if (string.IsNullOrWhiteSpace(provinceCode))
                return false;

            // Mã tỉnh thành từ 01-96
            var provincePattern = @"^(0[1-9]|[1-8][0-9]|9[0-6])$";
            return Regex.IsMatch(provinceCode, provincePattern);
        }

        /// <summary>
        /// Chuẩn hóa tên địa chỉ Việt Nam
        /// </summary>
        /// <param name="address">Địa chỉ cần chuẩn hóa</param>
        /// <returns>Địa chỉ đã chuẩn hóa</returns>
        /// <example>
        /// string formatted = RegexHelper.FormatVietnameseAddress("123 nguyễn trãi, quận 1, hcm");
        /// // returns "123 Nguyễn Trãi, Quận 1, HCM"
        /// </example>
        public static string FormatVietnameseAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return string.Empty;

            var normalized = NormalizeSpaces(address);

            // Viết hoa chữ cái đầu sau dấu phẩy và đầu chuỗi
            var result = Regex.Replace(normalized.ToLowerInvariant(),
                @"(^|\,\s*)(\w)",
                match => match.Groups[1].Value + match.Groups[2].Value.ToUpperInvariant());

            // Viết hoa các từ khóa địa chỉ
            var keywords = new Dictionary<string, string>
            {
                { @"\bquận\b", "Quận" },
                { @"\bhuyện\b", "Huyện" },
                { @"\bphường\b", "Phường" },
                { @"\bxã\b", "Xã" },
                { @"\bthị trấn\b", "Thị Trấn" },
                { @"\btp\b", "TP" },
                { @"\bhcm\b", "HCM" },
                { @"\bhà nội\b", "Hà Nội" },
                { @"\bđà nẵng\b", "Đà Nẵng" }
            };

            foreach (var keyword in keywords)
            {
                result = Regex.Replace(result, keyword.Key, keyword.Value, RegexOptions.IgnoreCase);
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Lấy phần nội dung nằm giữa hai đoạn văn bản cho trước trong một chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi nguồn cần tìm kiếm.</param>
        /// <param name="startText">Đoạn văn bản bắt đầu.</param>
        /// <param name="endText">Đoạn văn bản kết thúc.</param>
        /// <returns>
        /// Nội dung nằm giữa <paramref name="startText"/> và <paramref name="endText"/>,        
        /// </returns>
        public static string GetInnerContent(string text, string startText, string endText)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Regex tìm: #region RegionName ... #endregion
            // [\s\S]*?  => match mọi ký tự kể cả xuống dòng, ở chế độ non-greedy
            string pattern = $@"{startText}([\s\S]*?){endText}";

            var match = Regex.Match(text, pattern, RegexOptions.Multiline);

            return match.Success ? match.Groups[1].Value.Trim() : null;
        }

    }
}
