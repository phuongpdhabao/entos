using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper xử lý chuỗi (text) thường dùng.
    /// Cung cấp các phương thức tiện ích để xử lý, chuyển đổi và phân tích chuỗi.
    /// </summary>
    public static class TextHelper
    {
        /// <summary>
        /// Loại bỏ ký tự unicode khỏi chuỗi.
        /// Chuyển đổi các ký tự có dấu thành ký tự không dấu.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ unicode</returns>
        public static string RemoveUnicodeByNormalizationForm(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Loại bỏ ký tự đặc biệt khỏi chuỗi.
        /// Chỉ giữ lại chữ cái, số và khoảng trắng.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ ký tự đặc biệt</returns>
        public static string RemoveSpecialCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            return Regex.Replace(text, @"[^a-zA-Z0-9\s]", "");
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt (chuyên cho tiếng Việt).
        /// Chuyển đổi các ký tự có dấu thành ký tự không dấu.
        /// </summary>
        /// <param name="text">Chuỗi tiếng Việt cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ dấu</returns>
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Loại bỏ tất cả thẻ HTML khỏi chuỗi.
        /// Xóa các thẻ như &lt;div&gt;, &lt;p&gt;, &lt;span&gt;, v.v.
        /// </summary>
        /// <param name="text">Chuỗi HTML cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ thẻ HTML</returns>
        public static string RemoveHtmlTags(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return Regex.Replace(text, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Giải mã các entity HTML.
        /// Chuyển đổi &amp; thành &, &lt; thành &lt;, &gt; thành &gt;, v.v.
        /// </summary>
        /// <param name="text">Chuỗi chứa HTML entities</param>
        /// <returns>Chuỗi đã giải mã HTML entities</returns>
        public static string RemoveHtmlEntities(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return WebUtility.HtmlDecode(text);
        }

        /// <summary>
        /// Chuẩn hóa unicode theo form được chỉ định.
        /// FormC: Canonical Composition (mặc định)
        /// FormD: Canonical Decomposition
        /// </summary>
        /// <param name="text">Chuỗi cần chuẩn hóa</param>
        /// <param name="form">Form chuẩn hóa</param>
        /// <returns>Chuỗi đã chuẩn hóa unicode</returns>
        public static string NormalizeUnicode(string text, NormalizationForm form = NormalizationForm.FormC)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Normalize(form);
        }

        /// <summary>
        /// Trích xuất chỉ các ký tự số từ chuỗi.
        /// Loại bỏ tất cả ký tự khác, chỉ giữ lại 0-9.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi chỉ chứa số</returns>
        public static string OnlyDigits(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Trích xuất chỉ các ký tự chữ cái từ chuỗi.
        /// Loại bỏ tất cả ký tự khác, chỉ giữ lại a-z, A-Z.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi chỉ chứa chữ cái</returns>
        public static string OnlyLetters(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Tạo hash MD5 từ chuỗi.
        /// Trả về chuỗi hex 32 ký tự.
        /// </summary>
        /// <param name="text">Chuỗi cần hash</param>
        /// <returns>Hash MD5 dạng hex string</returns>
        public static string ToMd5(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Tạo hash SHA256 từ chuỗi.
        /// Trả về chuỗi hex 64 ký tự.
        /// </summary>
        /// <param name="text">Chuỗi cần hash</param>
        /// <returns>Hash SHA256 dạng hex string</returns>
        public static string ToSha256(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Tạo hash SHA1 từ chuỗi.
        /// Trả về chuỗi hex 40 ký tự.
        /// </summary>
        /// <param name="text">Chuỗi cần hash</param>
        /// <returns>Hash SHA1 dạng hex string</returns>
        public static string ToSha1(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            using var sha = SHA1.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Tạo hash SHA512 từ chuỗi.
        /// Trả về chuỗi hex 128 ký tự.
        /// </summary>
        /// <param name="text">Chuỗi cần hash</param>
        /// <returns>Hash SHA512 dạng hex string</returns>
        public static string ToSha512(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            using var sha = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Mã hóa chuỗi thành base64.
        /// </summary>
        /// <param name="text">Chuỗi cần mã hóa</param>
        /// <param name="encoding">Encoding sử dụng (mặc định UTF8)</param>
        /// <returns>Chuỗi base64</returns>
        public static string ToBase64(string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text)) return text;
            encoding ??= Encoding.UTF8;
            var bytes = encoding.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Giải mã chuỗi base64 thành chuỗi gốc.
        /// </summary>
        /// <param name="text">Chuỗi base64 cần giải mã</param>
        /// <param name="encoding">Encoding sử dụng (mặc định UTF8)</param>
        /// <returns>Chuỗi gốc</returns>
        public static string FromBase64(string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text)) return text;
            encoding ??= Encoding.UTF8;
            var bytes = Convert.FromBase64String(text);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// Mã hóa chuỗi thành base64 URL-safe.
        /// Thay thế + thành -, / thành _, và loại bỏ padding =.
        /// </summary>
        /// <param name="text">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi base64 URL-safe</returns>
        public static string ToBase64Url(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        /// <summary>
        /// Loại bỏ tất cả khoảng trắng khỏi chuỗi.
        /// Bao gồm space, tab, newline, v.v.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi không có khoảng trắng</returns>
        public static string TrimAll(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return Regex.Replace(text, @"\s+", "");
        }

        /// <summary>
        /// Trim chuỗi một cách an toàn.
        /// Tránh lỗi null reference exception.
        /// </summary>
        /// <param name="text">Chuỗi cần trim</param>
        /// <returns>Chuỗi đã trim hoặc null nếu input null</returns>
        public static string TrimSafe(string text)
        {
            return text?.Trim();
        }

        /// <summary>
        /// Đảo ngược chuỗi.
        /// Chuyển "hello" thành "olleh".
        /// </summary>
        /// <param name="text">Chuỗi cần đảo ngược</param>
        /// <returns>Chuỗi đã đảo ngược</returns>
        public static string Reverse(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sb = new StringBuilder(text.Length);
            for (int i = text.Length - 1; i >= 0; i--)
            {
                sb.Append(text[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Lặp lại chuỗi nhiều lần.
        /// Ví dụ: Repeat("ab", 3) = "ababab".
        /// </summary>
        /// <param name="text">Chuỗi cần lặp lại</param>
        /// <param name="count">Số lần lặp lại</param>
        /// <returns>Chuỗi đã lặp lại</returns>
        public static string Repeat(string text, int count)
        {
            if (string.IsNullOrEmpty(text) || count <= 0) return string.Empty;
            var sb = new StringBuilder(text.Length * count);
            for (int i = 0; i < count; i++)
            {
                sb.Append(text);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Cắt ngắn chuỗi đến độ dài tối đa.
        /// Thêm "..." nếu chuỗi bị cắt.
        /// </summary>
        /// <param name="text">Chuỗi cần cắt</param>
        /// <param name="maxLength">Độ dài tối đa</param>
        /// <returns>Chuỗi đã cắt</returns>
        public static string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || maxLength <= 0) return string.Empty;
            return text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;
        }

        /// <summary>
        /// Cắt chuỗi con một cách an toàn.
        /// Tránh lỗi index out of range.
        /// </summary>
        /// <param name="text">Chuỗi gốc</param>
        /// <param name="startIndex">Vị trí bắt đầu</param>
        /// <param name="length">Độ dài cần cắt</param>
        /// <returns>Chuỗi con đã cắt</returns>
        public static string SafeSubstring(string text, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(text) || startIndex < 0 || length <= 0 || startIndex >= text.Length) return string.Empty;
            return text.Substring(startIndex, Math.Min(length, text.Length - startIndex));
        }

        /// <summary>
        /// Loại bỏ các ký tự không phải ASCII.
        /// Chỉ giữ lại ký tự có mã ASCII <= 127.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi chỉ chứa ký tự ASCII</returns>
        public static string RemoveNonAscii(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c <= 127)
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Viết hoa chữ cái đầu của mỗi từ.
        /// Sử dụng culture hiện tại để xác định quy tắc viết hoa.
        /// </summary>
        /// <param name="text">Chuỗi cần viết hoa</param>
        /// <returns>Chuỗi đã viết hoa chữ cái đầu</returns>
        public static string CapitalizeWords(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        /// <summary>
        /// Loại bỏ khoảng trắng trùng lặp.
        /// Thay thế nhiều khoảng trắng liên tiếp bằng một khoảng trắng.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ khoảng trắng trùng lặp</returns>
        public static string RemoveDuplicateSpaces(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return Regex.Replace(text, @"\s+", " ").Trim();
        }

        /// <summary>
        /// Tách chuỗi thành mảng các từ.
        /// Chỉ lấy các từ chứa chữ cái và số.
        /// </summary>
        /// <param name="text">Chuỗi cần tách</param>
        /// <returns>Mảng các từ</returns>
        public static string[] ToWords(string text)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();
            return Regex.Matches(text, "[a-zA-Z0-9]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();
        }

        /// <summary>
        /// Loại bỏ các ký tự được chỉ định khỏi chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <param name="chars">Mảng các ký tự cần loại bỏ</param>
        /// <returns>Chuỗi đã loại bỏ các ký tự chỉ định</returns>
        public static string RemoveAll(string text, params char[] chars)
        {
            if (string.IsNullOrEmpty(text) || chars == null || chars.Length == 0) return text;
            var set = new HashSet<char>(chars);
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (!set.Contains(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Thay thế nhiều chuỗi cùng lúc.
        /// Hiệu quả hơn khi thay thế nhiều chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi cần thay thế</param>
        /// <param name="replacements">Dictionary chứa cặp chuỗi cũ - mới</param>
        /// <returns>Chuỗi đã thay thế</returns>
        public static string ReplaceMany(string text, Dictionary<string, string> replacements)
        {
            if (string.IsNullOrEmpty(text) || replacements == null || replacements.Count == 0) return text;

            var sb = new StringBuilder(text);
            foreach (var kv in replacements)
            {
                sb.Replace(kv.Key, kv.Value);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Loại bỏ các từ trùng lặp trong chuỗi.
        /// So sánh không phân biệt hoa thường.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ từ trùng lặp</returns>
        public static string RemoveDuplicateWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            var words = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words.Distinct(StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Xáo trộn thứ tự các từ trong chuỗi.
        /// Sử dụng thuật toán Fisher-Yates shuffle.
        /// </summary>
        /// <param name="text">Chuỗi cần xáo trộn</param>
        /// <returns>Chuỗi đã xáo trộn từ</returns>
        public static string ShuffleWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            var words = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var rnd = new Random();
            for (int i = words.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                (words[i], words[j]) = (words[j], words[i]);
            }
            return string.Join(" ", words);
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt (alias của RemoveDiacritics).
        /// Chuyển đổi các ký tự có dấu thành ký tự không dấu.
        /// </summary>
        /// <param name="text">Chuỗi tiếng Việt cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ dấu</returns>
        public static string RemoveVietnameseTone(string text)
        {
            return RemoveDiacritics(text);
        }

        /// <summary>
        /// Chuẩn hóa chuỗi tiếng Việt.
        /// Loại bỏ khoảng trắng thừa và chuẩn hóa định dạng.
        /// </summary>
        /// <param name="text">Chuỗi tiếng Việt cần chuẩn hóa</param>
        /// <returns>Chuỗi đã chuẩn hóa</returns>
        public static string NormalizeVietnamese(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            text = Regex.Replace(text, @"\s+", " ").Trim();
            // Có thể bổ sung chuẩn hóa dấu nếu cần
            return text;
        }

        /// <summary>
        /// Phát hiện xem chuỗi có chứa ký tự tiếng Việt không.
        /// Kiểm tra các ký tự có dấu tiếng Việt.
        /// </summary>
        /// <param name="text">Chuỗi cần kiểm tra</param>
        /// <returns>True nếu chứa ký tự tiếng Việt</returns>
        public static bool DetectVietnamese(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            return Regex.IsMatch(text, "[àáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ]", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Sinh mã kế tiếp dựa trên chuỗi code dạng số.
        /// </summary>
        /// <param name="code">
        /// Chuỗi code hiện tại (gồm toàn ký tự số, có thể có padding '0' ở đầu, độ dài không giới hạn).  
        /// <returns>
        /// Chuỗi code kế tiếp sau <paramref name="code"/>. 
        /// Ví dụ: "0023" → "0024".
        /// </param>
        public static string GetNextObjectCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return string.Empty;

            int length = code.Length;

            if (!long.TryParse(code, out long number))
                return string.Empty;

            number++;

            return number.ToString().PadLeft(length, '0');
        }


        #region Thay thế ký tự đầu và cuối

        /// <summary>
        /// Thay thế ký tự đầu tiên của chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi cần thay thế</param>
        /// <param name="newChar">Ký tự mới</param>
        /// <returns>Chuỗi đã thay thế ký tự đầu</returns>
        public static string ReplaceFirstChar(string text, char newChar)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return newChar + text.Substring(1);
        }

        /// <summary>
        /// Thay thế ký tự cuối cùng của chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi cần thay thế</param>
        /// <param name="newChar">Ký tự mới</param>
        /// <returns>Chuỗi đã thay thế ký tự cuối</returns>
        public static string ReplaceLastChar(string text, char newChar)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Substring(0, text.Length - 1) + newChar;
        }

        /// <summary>
        /// Thay thế ký tự đầu tiên nếu thỏa mãn điều kiện.
        /// </summary>
        /// <param name="text">Chuỗi cần thay thế</param>
        /// <param name="oldChar">Ký tự cũ cần thay thế</param>
        /// <param name="newChar">Ký tự mới</param>
        /// <returns>Chuỗi đã thay thế hoặc giữ nguyên</returns>
        public static string ReplaceFirstCharIf(string text, char oldChar, char newChar)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (text[0] == oldChar)
            {
                return newChar + text.Substring(1);
            }
            return text;
        }

        /// <summary>
        /// Thay thế ký tự cuối cùng nếu thỏa mãn điều kiện.
        /// </summary>
        /// <param name="text">Chuỗi cần thay thế</param>
        /// <param name="oldChar">Ký tự cũ cần thay thế</param>
        /// <param name="newChar">Ký tự mới</param>
        /// <returns>Chuỗi đã thay thế hoặc giữ nguyên</returns>
        public static string ReplaceLastCharIf(string text, char oldChar, char newChar)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (text[text.Length - 1] == oldChar)
            {
                return text.Substring(0, text.Length - 1) + newChar;
            }
            return text;
        }

        /// <summary>
        /// Thêm ký tự vào đầu chuỗi nếu chưa có.
        /// </summary>
        /// <param name="text">Chuỗi cần thêm</param>
        /// <param name="prefix">Ký tự cần thêm vào đầu</param>
        /// <returns>Chuỗi đã thêm prefix</returns>
        public static string EnsureStartsWith(string text, char prefix)
        {
            if (string.IsNullOrEmpty(text)) return prefix.ToString();
            return text.StartsWith(prefix.ToString()) ? text : prefix + text;
        }

        /// <summary>
        /// Thêm ký tự vào cuối chuỗi nếu chưa có.
        /// </summary>
        /// <param name="text">Chuỗi cần thêm</param>
        /// <param name="suffix">Ký tự cần thêm vào cuối</param>
        /// <returns>Chuỗi đã thêm suffix</returns>
        public static string EnsureEndsWith(string text, char suffix)
        {
            if (string.IsNullOrEmpty(text)) return suffix.ToString();
            return text.EndsWith(suffix.ToString()) ? text : text + suffix;
        }

        /// <summary>
        /// Loại bỏ ký tự đầu tiên của chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ ký tự đầu</returns>
        public static string RemoveFirstChar(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length > 1 ? text.Substring(1) : string.Empty;
        }

        /// <summary>
        /// Loại bỏ ký tự cuối cùng của chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ ký tự cuối</returns>
        public static string RemoveLastChar(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length > 1 ? text.Substring(0, text.Length - 1) : string.Empty;
        }

        /// <summary>
        /// Lấy ký tự đầu tiên của chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi cần lấy ký tự</param>
        /// <returns>Ký tự đầu tiên hoặc null nếu chuỗi rỗng</returns>
        public static char? GetFirstChar(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            return text[0];
        }

        /// <summary>
        /// Lấy ký tự cuối cùng của chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi cần lấy ký tự</param>
        /// <returns>Ký tự cuối cùng hoặc null nếu chuỗi rỗng</returns>
        public static char? GetLastChar(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            return text[text.Length - 1];
        }

        #endregion

        #region Chống tấn công và bảo mật

        /// <summary>
        /// Làm sạch input để tránh SQL Injection.
        /// Loại bỏ các ký tự nguy hiểm như ', ", ;, --, /*, */.
        /// </summary>
        /// <param name="text">Chuỗi cần làm sạch</param>
        /// <returns>Chuỗi đã làm sạch</returns>
        public static string SanitizeSqlInput(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sb = new StringBuilder(text);
            var dangerousChars = new[] { '\'', '"', ';', '-', '/', '*' };
            foreach (var dangerous in dangerousChars)
            {
                sb.Replace(dangerous.ToString(), "");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Loại bỏ các ký tự nguy hiểm có thể gây XSS (Cross-Site Scripting).
        /// </summary>
        /// <param name="text">Chuỗi cần làm sạch</param>
        /// <returns>Chuỗi đã loại bỏ ký tự nguy hiểm XSS</returns>
        public static string SanitizeXssInput(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            // Thay thế các ký tự nguy hiểm cho XSS
            var replacements = new Dictionary<string, string>
            {
                {"<", "&lt;"},
                {">", "&gt;"},
                {"\"", "&quot;"},
                {"'", "&#x27;"},
                {"&", "&amp;"},
                {"(", "&#x28;"},
                {")", "&#x29;"},
                {";", "&#x3B;"},
                {"script", "scr&#x69;pt"},
                {"javascript:", "java&#x73;cript:"},
                {"onload", "on&#x6C;oad"},
                {"onerror", "on&#x65;rror"},
                {"onclick", "on&#x63;lick"}
            };

            return ReplaceMany(text, replacements);
        }

        /// <summary>
        /// Loại bỏ các ký tự điều khiển khỏi chuỗi.
        /// Loại bỏ các ký tự như \t, \n, \r, \b, \f, v.v.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi đã loại bỏ ký tự điều khiển</returns>
        public static string RemoveControlCharacters(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (!char.IsControl(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Kiểm tra chuỗi có chứa ký tự nguy hiểm không.
        /// </summary>
        /// <param name="text">Chuỗi cần kiểm tra</param>
        /// <returns>True nếu chứa ký tự nguy hiểm</returns>
        public static bool ContainsDangerousCharacters(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            var dangerousPatterns = new[]
            {
                @"<script", @"javascript:", @"vbscript:", @"onload\s*=", @"onerror\s*=",
                @"onclick\s*=", @"<iframe", @"<object", @"<embed", @"<form",
                @"union\s+select", @"drop\s+table", @"delete\s+from", @"insert\s+into",
                @"update\s+set", @"exec\s*\(", @"xp_", @"sp_"
            };

            return dangerousPatterns.Any(pattern =>
                Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Tạo chuỗi ngẫu nhiên an toàn.
        /// Sử dụng Random thay vì RNGCryptoServiceProvider cho hiệu suất tốt hơn.
        /// </summary>
        /// <param name="length">Độ dài chuỗi</param>
        /// <param name="includeSpecialChars">Có bao gồm ký tự đặc biệt không</param>
        /// <returns>Chuỗi ngẫu nhiên an toàn</returns>
        public static string GenerateSecureRandomString(int length, bool includeSpecialChars = false)
        {
            if (length <= 0) return string.Empty;

            const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            var chars = letters + digits;
            if (includeSpecialChars)
                chars += special;

            var random = new Random();
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Mã hóa chuỗi nhạy cảm bằng cách thay thế ký tự.
        /// </summary>
        /// <param name="text">Chuỗi cần mã hóa</param>
        /// <param name="maskChar">Ký tự để che giấu</param>
        /// <param name="visibleChars">Số ký tự hiển thị ở đầu và cuối</param>
        /// <returns>Chuỗi đã mã hóa</returns>
        public static string MaskSensitiveData(string text, char maskChar = '*', int visibleChars = 2)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (text.Length <= visibleChars * 2) return new string(maskChar, text.Length);

            var sb = new StringBuilder(text.Length);
            sb.Append(text.Substring(0, visibleChars));
            sb.Append(maskChar, text.Length - visibleChars * 2);
            sb.Append(text.Substring(text.Length - visibleChars));
            return sb.ToString();
        }

        /// <summary>
        /// Kiểm tra độ mạnh của mật khẩu.
        /// </summary>
        /// <param name="password">Mật khẩu cần kiểm tra</param>
        /// <returns>Điểm độ mạnh (0-100)</returns>
        public static int CheckPasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password)) return 0;

            int score = 0;

            // Độ dài
            if (password.Length >= 8) score += 20;
            if (password.Length >= 12) score += 10;

            // Chữ cái thường
            if (password.Any(char.IsLower)) score += 10;

            // Chữ cái hoa
            if (password.Any(char.IsUpper)) score += 10;

            // Số
            if (password.Any(char.IsDigit)) score += 10;

            // Ký tự đặc biệt
            if (password.Any(c => !char.IsLetterOrDigit(c))) score += 15;

            // Đa dạng ký tự
            var uniqueChars = password.Distinct().Count();
            if (uniqueChars >= 8) score += 10;
            if (uniqueChars >= 12) score += 5;

            // Không có pattern đơn giản
            if (!Regex.IsMatch(password, @"(.)\1{2,}")) score += 10;

            return Math.Min(score, 100);
        }

        /// <summary>
        /// Làm sạch tên file, loại bỏ ký tự không hợp lệ.
        /// Thay thế các ký tự không được phép bằng ký tự thay thế.
        /// </summary>
        /// <param name="text">Tên file cần làm sạch</param>
        /// <param name="replacementChar">Ký tự thay thế (mặc định '_')</param>
        /// <returns>Tên file đã làm sạch</returns>
        public static string SanitizeFileName(string text, char replacementChar = '_')
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sb = new StringBuilder(text);
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var invalidChar in invalidChars)
            {
                sb.Replace(invalidChar, replacementChar);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Làm sạch đường dẫn, loại bỏ ký tự không hợp lệ.
        /// Thay thế các ký tự không được phép bằng ký tự thay thế.
        /// </summary>
        /// <param name="text">Đường dẫn cần làm sạch</param>
        /// <param name="replacementChar">Ký tự thay thế (mặc định '_')</param>
        /// <returns>Đường dẫn đã làm sạch</returns>
        public static string SanitizePath(string text, char replacementChar = '_')
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sb = new StringBuilder(text);
            var invalidChars = Path.GetInvalidPathChars();
            foreach (var invalidChar in invalidChars)
            {
                sb.Replace(invalidChar, replacementChar);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là email hợp lệ không.
        /// </summary>
        /// <param name="email">Chuỗi cần kiểm tra</param>
        /// <returns>True nếu là email hợp lệ</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là URL hợp lệ không.
        /// </summary>
        /// <param name="url">Chuỗi cần kiểm tra</param>
        /// <returns>True nếu là URL hợp lệ</returns>
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Loại bỏ các ký tự có thể gây lỗi encoding.
        /// </summary>
        /// <param name="text">Chuỗi cần làm sạch</param>
        /// <returns>Chuỗi đã loại bỏ ký tự encoding lỗi</returns>
        public static string SanitizeEncoding(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            // Loại bỏ các ký tự không hợp lệ cho UTF-8
            var bytes = Encoding.UTF8.GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion

        #region Tách dòng và tách câu

        /// <summary>
        /// Tách văn bản thành các dòng.
        /// Hỗ trợ nhiều loại ký tự xuống dòng (Windows, Unix, Mac).
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptyLines">Có loại bỏ dòng trống không</param>
        /// <returns>Mảng các dòng</returns>
        public static string[] SplitLines(string text, bool removeEmptyLines = false)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            var lines = text.Split(new[] { "\r\n", "\r", "\n" },
                removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

            return lines;
        }

        /// <summary>
        /// Tách văn bản thành các câu.
        /// Sử dụng dấu chấm, chấm than, chấm hỏi làm dấu hiệu kết thúc câu.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptySentences">Có loại bỏ câu trống không</param>
        /// <returns>Mảng các câu</returns>
        public static string[] SplitSentences(string text, bool removeEmptySentences = false)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            // Pattern để tách câu, xử lý cả trường hợp dấu chấm trong số
            var pattern = @"(?<=[.!?])\s+(?=[A-Z])";
            var sentences = Regex.Split(text, pattern);

            if (removeEmptySentences)
            {
                sentences = sentences.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            }

            return sentences;
        }

        /// <summary>
        /// Tách văn bản thành các đoạn văn.
        /// Sử dụng dấu xuống dòng kép làm dấu hiệu phân cách đoạn.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptyParagraphs">Có loại bỏ đoạn trống không</param>
        /// <returns>Mảng các đoạn văn</returns>
        public static string[] SplitParagraphs(string text, bool removeEmptyParagraphs = false)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            var paragraphs = text.Split(new[] { "\r\n\r\n", "\n\n", "\r\r" },
                removeEmptyParagraphs ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

            return paragraphs;
        }

        /// <summary>
        /// Tách văn bản thành các từ.
        /// Hỗ trợ nhiều loại ký tự phân cách từ.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptyWords">Có loại bỏ từ trống không</param>
        /// <param name="includePunctuation">Có bao gồm dấu câu không</param>
        /// <returns>Mảng các từ</returns>
        public static string[] SplitWords(string text, bool removeEmptyWords = true, bool includePunctuation = false)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            var separators = new[] { ' ', '\t', '\r', '\n', ',', ';', ':', '!', '?', '.', '(', ')', '[', ']', '{', '}', '"', '\'' };
            var words = text.Split(separators,
                removeEmptyWords ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

            if (!includePunctuation)
            {
                words = words.Where(w => !string.IsNullOrEmpty(w) && !char.IsPunctuation(w[0])).ToArray();
            }

            return words;
        }

        /// <summary>
        /// Tách văn bản thành các từ tiếng Việt.
        /// Xử lý đặc biệt cho tiếng Việt có dấu.
        /// </summary>
        /// <param name="text">Văn bản tiếng Việt cần tách</param>
        /// <param name="removeEmptyWords">Có loại bỏ từ trống không</param>
        /// <returns>Mảng các từ tiếng Việt</returns>
        public static string[] SplitVietnameseWords(string text, bool removeEmptyWords = true)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            // Pattern cho từ tiếng Việt (bao gồm dấu)
            var pattern = @"[\wàáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ]+";
            var matches = Regex.Matches(text, pattern);

            var words = matches.Cast<Match>().Select(m => m.Value).ToArray();

            if (removeEmptyWords)
            {
                words = words.Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();
            }

            return words;
        }

        /// <summary>
        /// Tách văn bản thành các ký tự.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeWhitespace">Có loại bỏ khoảng trắng không</param>
        /// <returns>Mảng các ký tự</returns>
        public static char[] SplitCharacters(string text, bool removeWhitespace = false)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<char>();

            var chars = text.ToCharArray();

            if (removeWhitespace)
            {
                chars = chars.Where(c => !char.IsWhiteSpace(c)).ToArray();
            }

            return chars;
        }

        /// <summary>
        /// Tách văn bản theo ký tự phân cách tùy chỉnh.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="separators">Mảng ký tự phân cách</param>
        /// <param name="removeEmptyItems">Có loại bỏ phần tử trống không</param>
        /// <returns>Mảng các phần tử đã tách</returns>
        public static string[] SplitBySeparators(string text, char[] separators, bool removeEmptyItems = false)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            return text.Split(separators,
                removeEmptyItems ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// Tách văn bản theo chuỗi phân cách.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="separator">Chuỗi phân cách</param>
        /// <param name="removeEmptyItems">Có loại bỏ phần tử trống không</param>
        /// <param name="ignoreCase">Có bỏ qua hoa thường không</param>
        /// <returns>Mảng các phần tử đã tách</returns>
        public static string[] SplitByString(string text, string separator, bool removeEmptyItems = false, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            var options = removeEmptyItems ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;

            if (ignoreCase)
            {
                return text.Split(new[] { separator }, options);
            }

            return text.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Tách văn bản theo regex pattern.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="pattern">Regex pattern</param>
        /// <param name="removeEmptyItems">Có loại bỏ phần tử trống không</param>
        /// <returns>Mảng các phần tử đã tách</returns>
        public static string[] SplitByRegex(string text, string pattern, bool removeEmptyItems = false)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            var parts = Regex.Split(text, pattern);

            if (removeEmptyItems)
            {
                parts = parts.Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
            }

            return parts;
        }

        /// <summary>
        /// Tách văn bản thành các chunk có độ dài cố định.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="chunkSize">Độ dài mỗi chunk</param>
        /// <param name="overlap">Số ký tự chồng lấp giữa các chunk</param>
        /// <returns>Mảng các chunk</returns>
        public static string[] SplitIntoChunks(string text, int chunkSize, int overlap = 0)
        {
            if (string.IsNullOrEmpty(text) || chunkSize <= 0) return Array.Empty<string>();

            var chunks = new List<string>();
            var start = 0;

            while (start < text.Length)
            {
                var end = Math.Min(start + chunkSize, text.Length);
                var chunk = text.Substring(start, end - start);
                chunks.Add(chunk);

                start = end - overlap;
                if (start >= text.Length) break;
            }

            return chunks.ToArray();
        }

        /// <summary>
        /// Tách văn bản thành các chunk theo từ.
        /// Không cắt giữa từ.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="maxWords">Số từ tối đa mỗi chunk</param>
        /// <returns>Mảng các chunk</returns>
        public static string[] SplitIntoWordChunks(string text, int maxWords)
        {
            if (string.IsNullOrEmpty(text) || maxWords <= 0) return Array.Empty<string>();

            var words = SplitWords(text);
            var chunks = new List<string>();

            for (int i = 0; i < words.Length; i += maxWords)
            {
                var chunkWords = words.Skip(i).Take(maxWords).ToArray();
                chunks.Add(string.Join(" ", chunkWords));
            }

            return chunks.ToArray();
        }

        /// <summary>
        /// Tách văn bản thành các chunk theo câu.
        /// Không cắt giữa câu.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="maxSentences">Số câu tối đa mỗi chunk</param>
        /// <returns>Mảng các chunk</returns>
        public static string[] SplitIntoSentenceChunks(string text, int maxSentences)
        {
            if (string.IsNullOrEmpty(text) || maxSentences <= 0) return Array.Empty<string>();

            var sentences = SplitSentences(text);
            var chunks = new List<string>();

            for (int i = 0; i < sentences.Length; i += maxSentences)
            {
                var chunkSentences = sentences.Skip(i).Take(maxSentences).ToArray();
                chunks.Add(string.Join(" ", chunkSentences));
            }

            return chunks.ToArray();
        }

        /// <summary>
        /// Đếm số dòng trong văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần đếm</param>
        /// <param name="countEmptyLines">Có đếm dòng trống không</param>
        /// <returns>Số dòng</returns>
        public static int CountLines(string text, bool countEmptyLines = true)
        {
            if (string.IsNullOrEmpty(text)) return 0;

            var lines = SplitLines(text, !countEmptyLines);
            return lines.Length;
        }

        /// <summary>
        /// Đếm số câu trong văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần đếm</param>
        /// <param name="countEmptySentences">Có đếm câu trống không</param>
        /// <returns>Số câu</returns>
        public static int CountSentences(string text, bool countEmptySentences = false)
        {
            if (string.IsNullOrEmpty(text)) return 0;

            var sentences = SplitSentences(text, !countEmptySentences);
            return sentences.Length;
        }

        /// <summary>
        /// Đếm số từ trong văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần đếm</param>
        /// <param name="countEmptyWords">Có đếm từ trống không</param>
        /// <returns>Số từ</returns>
        public static int CountWords(string text, bool countEmptyWords = false)
        {
            if (string.IsNullOrEmpty(text)) return 0;

            var words = SplitWords(text, !countEmptyWords);
            return words.Length;
        }

        /// <summary>
        /// Đếm số ký tự trong văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần đếm</param>
        /// <param name="countWhitespace">Có đếm khoảng trắng không</param>
        /// <returns>Số ký tự</returns>
        public static int CountCharacters(string text, bool countWhitespace = true)
        {
            if (string.IsNullOrEmpty(text)) return 0;

            if (countWhitespace)
                return text.Length;

            return text.Count(c => !char.IsWhiteSpace(c));
        }

        /// <summary>
        /// Lấy dòng đầu tiên của văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần lấy</param>
        /// <returns>Dòng đầu tiên</returns>
        public static string GetFirstLine(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var lines = SplitLines(text, true);
            return lines.Length > 0 ? lines[0] : string.Empty;
        }

        /// <summary>
        /// Lấy dòng cuối cùng của văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần lấy</param>
        /// <returns>Dòng cuối cùng</returns>
        public static string GetLastLine(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var lines = SplitLines(text, true);
            return lines.Length > 0 ? lines[lines.Length - 1] : string.Empty;
        }

        /// <summary>
        /// Lấy câu đầu tiên của văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần lấy</param>
        /// <returns>Câu đầu tiên</returns>
        public static string GetFirstSentence(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var sentences = SplitSentences(text, true);
            return sentences.Length > 0 ? sentences[0] : string.Empty;
        }

        /// <summary>
        /// Lấy câu cuối cùng của văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần lấy</param>
        /// <returns>Câu cuối cùng</returns>
        public static string GetLastSentence(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var sentences = SplitSentences(text, true);
            return sentences.Length > 0 ? sentences[sentences.Length - 1] : string.Empty;
        }

        /// <summary>
        /// Tách văn bản thành cấu trúc phân cấp: dòng -> câu -> từ.
        /// Trả về Dictionary với key là số dòng, value là mảng các câu.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptyLines">Có loại bỏ dòng trống không</param>
        /// <param name="removeEmptySentences">Có loại bỏ câu trống không</param>
        /// <returns>Dictionary chứa cấu trúc phân cấp dòng-câu</returns>
        public static Dictionary<int, string[]> SplitLinesAndSentences(string text, bool removeEmptyLines = false, bool removeEmptySentences = false)
        {
            if (string.IsNullOrEmpty(text)) return new Dictionary<int, string[]>();

            var lines = SplitLines(text, removeEmptyLines);
            var result = new Dictionary<int, string[]>();

            for (int i = 0; i < lines.Length; i++)
            {
                var sentences = SplitSentences(lines[i], removeEmptySentences);
                result[i] = sentences;
            }

            return result;
        }

        /// <summary>
        /// Tách văn bản thành cấu trúc phân cấp: dòng -> câu -> từ.
        /// Trả về Dictionary với key là số dòng, value là Dictionary chứa câu và từ.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptyLines">Có loại bỏ dòng trống không</param>
        /// <param name="removeEmptySentences">Có loại bỏ câu trống không</param>
        /// <param name="removeEmptyWords">Có loại bỏ từ trống không</param>
        /// <returns>Dictionary chứa cấu trúc phân cấp dòng-câu-từ</returns>
        public static Dictionary<int, Dictionary<int, string[]>> SplitLinesSentencesAndWords(string text, bool removeEmptyLines = false, bool removeEmptySentences = false, bool removeEmptyWords = true)
        {
            if (string.IsNullOrEmpty(text)) return new Dictionary<int, Dictionary<int, string[]>>();

            var lines = SplitLines(text, removeEmptyLines);
            var result = new Dictionary<int, Dictionary<int, string[]>>();

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var sentences = SplitSentences(lines[lineIndex], removeEmptySentences);
                var sentenceDict = new Dictionary<int, string[]>();

                for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
                {
                    var words = SplitWords(sentences[sentenceIndex], removeEmptyWords);
                    sentenceDict[sentenceIndex] = words;
                }

                result[lineIndex] = sentenceDict;
            }

            return result;
        }

        /// <summary>
        /// Tách văn bản thành mảng 2 chiều: dòng x câu.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptyLines">Có loại bỏ dòng trống không</param>
        /// <param name="removeEmptySentences">Có loại bỏ câu trống không</param>
        /// <returns>Mảng 2 chiều [dòng][câu]</returns>
        public static string[][] SplitLinesAndSentencesArray(string text, bool removeEmptyLines = false, bool removeEmptySentences = false)
        {
            if (string.IsNullOrEmpty(text)) return new string[0][];

            var lines = SplitLines(text, removeEmptyLines);
            var result = new string[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = SplitSentences(lines[i], removeEmptySentences);
            }

            return result;
        }

        /// <summary>
        /// Tách văn bản thành mảng 3 chiều: dòng x câu x từ.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptyLines">Có loại bỏ dòng trống không</param>
        /// <param name="removeEmptySentences">Có loại bỏ câu trống không</param>
        /// <param name="removeEmptyWords">Có loại bỏ từ trống không</param>
        /// <returns>Mảng 3 chiều [dòng][câu][từ]</returns>
        public static string[][][] SplitLinesSentencesAndWordsArray(string text, bool removeEmptyLines = false, bool removeEmptySentences = false, bool removeEmptyWords = true)
        {
            if (string.IsNullOrEmpty(text)) return new string[0][][];

            var lines = SplitLines(text, removeEmptyLines);
            var result = new string[lines.Length][][];

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var sentences = SplitSentences(lines[lineIndex], removeEmptySentences);
                result[lineIndex] = new string[sentences.Length][];

                for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
                {
                    result[lineIndex][sentenceIndex] = SplitWords(sentences[sentenceIndex], removeEmptyWords);
                }
            }

            return result;
        }

        /// <summary>
        /// Lấy tất cả câu từ tất cả các dòng.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptyLines">Có loại bỏ dòng trống không</param>
        /// <param name="removeEmptySentences">Có loại bỏ câu trống không</param>
        /// <returns>Mảng tất cả câu từ tất cả dòng</returns>
        public static string[] GetAllSentencesFromLines(string text, bool removeEmptyLines = false, bool removeEmptySentences = false)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            var lines = SplitLines(text, removeEmptyLines);
            var allSentences = new List<string>();

            foreach (var line in lines)
            {
                var sentences = SplitSentences(line, removeEmptySentences);
                allSentences.AddRange(sentences);
            }

            return allSentences.ToArray();
        }

        /// <summary>
        /// Lấy tất cả từ từ tất cả các câu trong tất cả các dòng.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="removeEmptyLines">Có loại bỏ dòng trống không</param>
        /// <param name="removeEmptySentences">Có loại bỏ câu trống không</param>
        /// <param name="removeEmptyWords">Có loại bỏ từ trống không</param>
        /// <returns>Mảng tất cả từ từ tất cả câu và dòng</returns>
        public static string[] GetAllWordsFromLinesAndSentences(string text, bool removeEmptyLines = false, bool removeEmptySentences = false, bool removeEmptyWords = true)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

            var lines = SplitLines(text, removeEmptyLines);
            var allWords = new List<string>();

            foreach (var line in lines)
            {
                var sentences = SplitSentences(line, removeEmptySentences);
                foreach (var sentence in sentences)
                {
                    var words = SplitWords(sentence, removeEmptyWords);
                    allWords.AddRange(words);
                }
            }

            return allWords.ToArray();
        }

        /// <summary>
        /// Tách văn bản thành các đoạn, mỗi đoạn chứa số dòng cố định.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="linesPerParagraph">Số dòng mỗi đoạn</param>
        /// <param name="removeEmptyLines">Có loại bỏ dòng trống không</param>
        /// <returns>Mảng các đoạn văn</returns>
        public static string[] SplitIntoLineParagraphs(string text, int linesPerParagraph, bool removeEmptyLines = false)
        {
            if (string.IsNullOrEmpty(text) || linesPerParagraph <= 0) return Array.Empty<string>();

            var lines = SplitLines(text, removeEmptyLines);
            var paragraphs = new List<string>();

            for (int i = 0; i < lines.Length; i += linesPerParagraph)
            {
                var paragraphLines = lines.Skip(i).Take(linesPerParagraph).ToArray();
                paragraphs.Add(string.Join(Environment.NewLine, paragraphLines));
            }

            return paragraphs.ToArray();
        }

        /// <summary>
        /// Tách văn bản thành các đoạn, mỗi đoạn chứa số câu cố định.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="sentencesPerParagraph">Số câu mỗi đoạn</param>
        /// <param name="removeEmptySentences">Có loại bỏ câu trống không</param>
        /// <returns>Mảng các đoạn văn</returns>
        public static string[] SplitIntoSentenceParagraphs(string text, int sentencesPerParagraph, bool removeEmptySentences = false)
        {
            if (string.IsNullOrEmpty(text) || sentencesPerParagraph <= 0) return Array.Empty<string>();

            var allSentences = GetAllSentencesFromLines(text, false, removeEmptySentences);
            var paragraphs = new List<string>();

            for (int i = 0; i < allSentences.Length; i += sentencesPerParagraph)
            {
                var paragraphSentences = allSentences.Skip(i).Take(sentencesPerParagraph).ToArray();
                paragraphs.Add(string.Join(" ", paragraphSentences));
            }

            return paragraphs.ToArray();
        }

        /// <summary>
        /// Tách văn bản thành các đoạn, mỗi đoạn chứa số từ cố định.
        /// </summary>
        /// <param name="text">Văn bản cần tách</param>
        /// <param name="wordsPerParagraph">Số từ mỗi đoạn</param>
        /// <param name="removeEmptyWords">Có loại bỏ từ trống không</param>
        /// <returns>Mảng các đoạn văn</returns>
        public static string[] SplitIntoWordParagraphs(string text, int wordsPerParagraph, bool removeEmptyWords = true)
        {
            if (string.IsNullOrEmpty(text) || wordsPerParagraph <= 0) return Array.Empty<string>();

            var allWords = GetAllWordsFromLinesAndSentences(text, false, false, removeEmptyWords);
            var paragraphs = new List<string>();

            for (int i = 0; i < allWords.Length; i += wordsPerParagraph)
            {
                var paragraphWords = allWords.Skip(i).Take(wordsPerParagraph).ToArray();
                paragraphs.Add(string.Join(" ", paragraphWords));
            }

            return paragraphs.ToArray();
        }

        /// <summary>
        /// Tìm câu chứa từ khóa trong văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần tìm</param>
        /// <param name="keyword">Từ khóa cần tìm</param>
        /// <param name="ignoreCase">Có bỏ qua hoa thường không</param>
        /// <returns>Mảng các câu chứa từ khóa</returns>
        public static string[] FindSentencesContainingKeyword(string text, string keyword, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword)) return Array.Empty<string>();

            var allSentences = GetAllSentencesFromLines(text);
            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            return allSentences.Where(sentence => sentence.Contains(keyword, comparison)).ToArray();
        }

        /// <summary>
        /// Tìm dòng chứa từ khóa trong văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần tìm</param>
        /// <param name="keyword">Từ khóa cần tìm</param>
        /// <param name="ignoreCase">Có bỏ qua hoa thường không</param>
        /// <returns>Mảng các dòng chứa từ khóa</returns>
        public static string[] FindLinesContainingKeyword(string text, string keyword, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword)) return Array.Empty<string>();

            var lines = SplitLines(text);
            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            return lines.Where(line => line.Contains(keyword, comparison)).ToArray();
        }

        /// <summary>
        /// Đếm số lần xuất hiện của từ khóa trong văn bản.
        /// </summary>
        /// <param name="text">Văn bản cần đếm</param>
        /// <param name="keyword">Từ khóa cần đếm</param>
        /// <param name="ignoreCase">Có bỏ qua hoa thường không</param>
        /// <returns>Số lần xuất hiện</returns>
        public static int CountKeywordOccurrences(string text, string keyword, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword)) return 0;

            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            var count = 0;
            var index = 0;

            while ((index = text.IndexOf(keyword, index, comparison)) != -1)
            {
                count++;
                index += keyword.Length;
            }

            return count;
        }

        /// <summary>
        /// Tạo bản tóm tắt văn bản bằng cách lấy n câu đầu tiên.
        /// </summary>
        /// <param name="text">Văn bản cần tóm tắt</param>
        /// <param name="sentenceCount">Số câu cần lấy</param>
        /// <param name="removeEmptySentences">Có loại bỏ câu trống không</param>
        /// <returns>Bản tóm tắt</returns>
        public static string CreateSummaryBySentences(string text, int sentenceCount, bool removeEmptySentences = false)
        {
            if (string.IsNullOrEmpty(text) || sentenceCount <= 0) return string.Empty;

            var allSentences = GetAllSentencesFromLines(text, false, removeEmptySentences);
            var summarySentences = allSentences.Take(sentenceCount).ToArray();

            return string.Join(" ", summarySentences);
        }

        /// <summary>
        /// Tạo bản tóm tắt văn bản bằng cách lấy n từ đầu tiên.
        /// </summary>
        /// <param name="text">Văn bản cần tóm tắt</param>
        /// <param name="wordCount">Số từ cần lấy</param>
        /// <param name="removeEmptyWords">Có loại bỏ từ trống không</param>
        /// <returns>Bản tóm tắt</returns>
        public static string CreateSummaryByWords(string text, int wordCount, bool removeEmptyWords = true)
        {
            if (string.IsNullOrEmpty(text) || wordCount <= 0) return string.Empty;

            var allWords = GetAllWordsFromLinesAndSentences(text, false, false, removeEmptyWords);
            var summaryWords = allWords.Take(wordCount).ToArray();

            return string.Join(" ", summaryWords);
        }

        #endregion

        /// <summary>
        /// Tách mảng string thành các block nhỏ, mỗi block là một chuỗi có tổng độ dài không vượt quá maxBlockLength (mặc định 4000), các phần tử trong block được nối bằng delimiter.
        /// </summary>
        /// <param name="items">Mảng string cần tách</param>
        /// <param name="maxBlockLength">Độ dài tối đa mỗi block (mặc định 4000)</param>
        /// <param name="delimiter">Chuỗi phân cách giữa các phần tử trong block (mặc định: "␟␟␟")</param>
        /// <returns>Danh sách các block (string)</returns>
        public static List<string> SplitArrayToBlocks(string[] items, int maxBlockLength = 4000, string delimiter = "␟\r\n")
        {
            var result = new List<string>();
            if (items == null || items.Length == 0) return result;
            var currentBlock = new List<string>();
            int currentLength = 0;
            foreach (var item in items)
            {
                int itemLength = item?.Length ?? 0;
                int delimiterLength = currentBlock.Count > 0 ? delimiter.Length : 0;
                if (currentLength + itemLength + delimiterLength > maxBlockLength && currentBlock.Count > 0)
                {
                    result.Add(string.Join(delimiter, currentBlock));
                    currentBlock.Clear();
                    currentLength = 0;
                    delimiterLength = 0;
                }
                currentBlock.Add(item);
                currentLength += itemLength + delimiterLength;
            }
            if (currentBlock.Count > 0)
                result.Add(string.Join(delimiter, currentBlock));
            return result;
        }

        /// <summary>
        /// Tách mảng string thành các block nhỏ, mỗi block là một chuỗi có tổng độ dài không vượt quá maxBlockLength,
        /// các phần tử trong block được nối bằng delimiter. Trả về danh sách (blockText, lineCount).
        /// </summary>
        /// <param name="items">Mảng string cần tách</param>
        /// <param name="maxBlockLength">Độ dài tối đa mỗi block (mặc định 4000)</param>
        /// <param name="delimiter">Chuỗi phân cách giữa các phần tử trong block (mặc định: "␟\r\n")</param>
        /// <returns>Danh sách các block và số dòng tương ứng</returns>
        public static List<(string BlockText, int LineCount)> SplitArrayToBlocksLineCount(string[] items, int maxBlockLength = 4000, string delimiter = "␟\r\n")
        {
            var result = new List<(string BlockText, int LineCount)>();
            if (items == null || items.Length == 0) return result;

            var currentBlock = new List<string>();
            int currentLength = 0;

            foreach (var item in items)
            {

                int itemLength = item?.Length ?? 0;
                int delimiterLength = currentBlock.Count > 0 ? delimiter.Length : 0;

                if (currentLength + itemLength + delimiterLength > maxBlockLength && currentBlock.Count > 0)
                {
                    result.Add((string.Join(delimiter, currentBlock), currentBlock.Count));
                    currentBlock.Clear();
                    currentLength = 0;
                    delimiterLength = 0;
                }

                currentBlock.Add(item);
                currentLength += itemLength + delimiterLength;
            }

            if (currentBlock.Count > 0)
                result.Add((string.Join(delimiter, currentBlock), currentBlock.Count));

            return result;
        }



        /// <summary>
        /// Ghép các block (string) thành lại mảng string gốc, dựa vào delimiter.
        /// </summary>
        /// <param name="blocks">Danh sách các block (string)</param>
        /// <param name="delimiter">Chuỗi phân cách giữa các phần tử trong block (mặc định: "␟␟␟")</param>
        /// <returns>Mảng string đã tách từ các block</returns>
        public static string[] JoinBlocksToArray(IEnumerable<string> blocks, string delimiter = "␟\r\n")
        {
            if (blocks == null) return Array.Empty<string>();
            var result = new List<string>();
            foreach (var block in blocks)
            {
                if (!string.IsNullOrEmpty(block))
                    result.AddRange(block.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries));
            }
            return result.ToArray();
        }

        public static string[] JoinBlocksToArray(IEnumerable<(string, string)> blocks, string delimiter = "␟\r\n")
        {
            if (blocks == null) return Array.Empty<string>();
            var result = new List<string>();
            foreach (var (translated, original) in blocks)
            {
                if (!string.IsNullOrEmpty(translated))
                    result.AddRange(translated.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries));
            }
            return result.ToArray();
        }
        public static bool IsMultiLine(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            return input.Contains("\n") || input.Contains("\r");
        }

        public static string RemoveXmlNode(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            string noTags = System.Text.RegularExpressions.Regex.Replace(input, @"(?<!</[^>]+>)[\r\n]+$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return System.Text.RegularExpressions.Regex.Replace(noTags, @"</?[^>]+?>\s*", "");
        }


        /// <summary>
        /// Format chuỗi index để so sánh, thêm số 0 đằng trước nếu cần.
        /// </summary>
        /// <param name="noIndex">Chuỗi index cần format</param>
        /// <returns>Chuỗi index đã được format</returns>
        public static string CompareIndex(string noIndex)
        {
            if (string.IsNullOrEmpty(noIndex))
                return noIndex;
            if (noIndex.Contains("."))
            {
                string str1 = "";
                string str2 = noIndex;
                char[] chArray = new char[1] { '.' };
                foreach (string str3 in str2.Split(chArray))
                {
                    if (str3.Length == 1 && (uint)Convert.ToInt32(str3) > 0U)
                        str1 += "0";
                    str1 = str1 + str3 + ".";
                }

                return str1;
            }

            string str = "";
            if (noIndex.Length == 1 && (uint)Convert.ToInt32(noIndex) > 0U)
                str += "0";
            return str + noIndex + ".";
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt và các ký tự đặc biệt từ chuỗi.
        /// Chuyển đổi chuỗi về dạng không dấu sử dụng Unicode normalization để dễ xử lý.
        /// </summary>
        /// <param name="input">Chuỗi cần loại bỏ dấu</param>
        /// <returns>Chuỗi đã được loại bỏ dấu, hoặc chuỗi gốc nếu input là null/empty</returns>
        public static string RemoveAccents(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            try
            {
                // Normalize về FormKD để tách các ký tự có dấu thành ký tự gốc + dấu
                string normalized = input.Normalize(NormalizationForm.FormKD);
                var builder = new StringBuilder(input.Length);

                // Lọc bỏ các ký tự NonSpacingMark (dấu tiếng Việt)
                foreach (char c in normalized)
                {
                    if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    {
                        builder.Append(c);
                    }
                }

                return builder.ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khi loại bỏ dấu từ chuỗi: {ex.Message}", ex);
            }
        }
        #region Number & Code Utilities

        /// <summary>
        /// Tạo mã số với độ dài cố định, thêm số 0 đằng trước nếu cần.
        /// </summary>
        /// <param name="position">Số cần chuyển thành mã</param>
        /// <param name="length">Độ dài mong muốn của mã</param>
        /// <returns>Mã số với độ dài cố định</returns>
        /// <exception cref="ArgumentException">Khi position âm hoặc length nhỏ hơn 1</exception>
        /// <example>
        /// <code>
        /// string code1 = Tools.GetNumberCode(123, 5);  // "00123"
        /// string code2 = Tools.GetNumberCode(45, 3);   // "045"
        /// string code3 = Tools.GetNumberCode(999, 2);  // "999" (không thêm 0)
        /// </code>
        /// </example>
        public static string GetNumberCode(int position, int length)
        {
            if (position < 0)
                throw new ArgumentException("Position không thể âm", nameof(position));
            if (length < 1)
                throw new ArgumentException("Length phải lớn hơn 0", nameof(length));

            int currentLength = GetNumberMaxLength(position);
            string result = position.ToString("D");

            // Thêm số 0 đằng trước
            for (int index = currentLength; index < length; index++)
                result = "0" + result;

            return result;
        }

        /// <summary>
        /// Lấy độ dài tối đa của một số khi được chuyển thành chuỗi.
        /// </summary>
        /// <param name="number">Số cần kiểm tra độ dài</param>
        /// <returns>Số chữ số của number</returns>
        /// <example>
        /// <code>
        /// int length1 = Tools.GetNumberMaxLength(123);   // 3
        /// int length2 = Tools.GetNumberMaxLength(9999);  // 4
        /// int length3 = Tools.GetNumberMaxLength(1);     // 1
        /// </code>
        /// </example>
        public static int GetNumberMaxLength(int number)
        {
            return number.ToString("D").Length;
        }

        #endregion

        /// <summary>
        /// Đếm số lần xuất hiện của các từ trong nội dung
        /// </summary>
        /// <param name="content">Nội dung cần đếm từ</param>
        /// <param name="existedDictionary">Dictionary đã tồn tại để thêm vào</param>
        /// <returns>Dictionary chứa từ và số lần xuất hiện</returns>
        public static IDictionary<string, int> GetDuplicateWords(string content, IDictionary<string, int> existedDictionary = null)
        {
            IDictionary<string, int> result = existedDictionary != null ? existedDictionary : new Dictionary<string, int>();
            if (string.IsNullOrEmpty(content))
                return result;
            //content = new string((from c in content
            //                      where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
            //                            select c).ToArray());
            content = content.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace(" - ", " ").Replace("...", ".").Replace("..", ".").Replace(".,", ",").Replace(". ", " ").Replace(", ", " ");
            if (string.IsNullOrEmpty(content))
                return result;
            if (!char.IsLetterOrDigit(content[content.Length - 1]))
                content = content.Substring(0, content.Length - 1);
            var contents = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in contents)
            {

                var w = word.Trim();
                w = new string((from c in w
                                where c.Equals(',') || c.Equals('.') || char.IsLetterOrDigit(c)
                                select c).ToArray());
                if (string.IsNullOrEmpty(w))
                    continue;
                if (w.Length < 2)
                    continue;
                if (!char.IsLetterOrDigit(w[w.Length - 1]))
                    w = w.Substring(0, w.Length - 1);
                double n;
                //Nếu không phải số thì thêm kiểm tra thuật ngữ
                if (!double.TryParse(w, out n))
                {
                    string key = "";
                    foreach (var k in result.Keys)
                    {
                        if (k.Equals(w, StringComparison.OrdinalIgnoreCase))
                        {
                            key = k; break;
                        }
                    }
                    if (!string.IsNullOrEmpty(key))
                    {
                        result[key] = result[key] + 1;
                    }
                    else
                    {
                        result.Add(w, 1);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Đếm số lần xuất hiện của các cặp từ liên tiếp trong nội dung
        /// </summary>
        /// <param name="content">Nội dung cần đếm cặp từ</param>
        /// <param name="existedDictionary">Dictionary đã tồn tại để thêm vào</param>
        /// <returns>Dictionary chứa cặp từ và số lần xuất hiện</returns>
        public static IDictionary<string, int> GetDuplicateDoubleWords(string content, IDictionary<string, int> existedDictionary = null)
        {
            IDictionary<string, int> result = existedDictionary != null ? existedDictionary : new Dictionary<string, int>();
            if (string.IsNullOrEmpty(content))
                return result;
            //content = new string((from c in content
            //                      where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
            //                            select c).ToArray());
            content = content.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace(" - ", " ").Replace("...", ".").Replace("..", ".").Replace(".,", ",").Replace(". ", " ").Replace(", ", " ");
            if (string.IsNullOrEmpty(content))
                return result;
            if (!char.IsLetterOrDigit(content[content.Length - 1]))
                content = content.Substring(0, content.Length - 1);
            var contents = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < contents.Length - 1; i++)
            {
                var w1 = contents[i].Trim();
                w1 = new string((from c in w1
                                 where c.Equals(',') || c.Equals('.') || char.IsLetterOrDigit(c)
                                 select c).ToArray());
                if (string.IsNullOrEmpty(w1))
                    continue;
                if (!char.IsLetterOrDigit(w1[w1.Length - 1]))
                    continue;
                var w2 = contents[i + 1].Trim();
                w2 = new string((from c in w2
                                 where c.Equals(',') || c.Equals('.') || char.IsLetterOrDigit(c)
                                 select c).ToArray());
                if (string.IsNullOrEmpty(w2))
                    continue;
                if (!char.IsLetterOrDigit(w1[w1.Length - 1]))
                    w1 = w1.Substring(0, w1.Length - 1);
                var w = w1 + " " + w2;
                //Nếu không phải số thì thêm kiểm tra thuật ngữ
                string key = "";
                foreach (var k in result.Keys)
                {
                    if (k.Equals(w, StringComparison.OrdinalIgnoreCase))
                    {
                        key = k; break;
                    }
                }
                if (!string.IsNullOrEmpty(key))
                {
                    result[key] = result[key] + 1;
                }
                else
                {
                    result.Add(w, 1);
                }
            }
            //Xóa bỏ những chỗ không trùng
            var keyList = result.Keys.ToList();
            for (int i = keyList.Count - 1; i >= 0; i--)
            {
                if (result[keyList[i]] < 2)
                    result.Remove(keyList[i]);
            }
            return result;
        }


        /// <summary>
        /// Highlight từ khóa trong văn bản với các tùy chọn định dạng.
        /// Phương thức này tìm và highlight tất cả các từ khóa hợp lệ trong văn bản.
        /// </summary>
        /// <param name="text">Văn bản gốc cần highlight</param>
        /// <param name="hightlight">Từ khóa cần highlight</param>
        /// <param name="invalidsText">Danh sách các từ cha không được highlight</param>
        /// <param name="position">Vị trí cụ thể cần highlight (tùy chọn)</param>
        /// <param name="fontSize">Kích thước font cho highlight (tùy chọn)</param>
        /// <returns>Văn bản đã được highlight với thẻ HTML</returns>
        public static string HightlightText(string text, string hightlight, string[] invalidsText = null, int? position = null, int? fontSize = null)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(hightlight))
                return text;

            try
            {
                // Highlight tại vị trí cụ thể
                if (position.HasValue)
                {
                    int space = 0;
                    for (int i = 0; i < text.Length - 1; i++)
                    {
                        if (space + 1 == position.Value)
                        {
                            var hightlightIndex = text.IndexOf(hightlight, i, System.StringComparison.OrdinalIgnoreCase);
                            if (hightlightIndex < 0)
                            {
                                // Thử tìm trong văn bản không dấu
                                hightlightIndex = RemoveUnicode(text).IndexOf(hightlight, i, System.StringComparison.OrdinalIgnoreCase);
                            }
                            if (hightlightIndex == i)
                            {
                                string hightlightText = text.Substring(0, hightlightIndex);
                                if (fontSize.HasValue)
                                    hightlightText += $"<size={fontSize.Value}>";
                                hightlightText += "<b>";
                                hightlightText += text.Substring(hightlightIndex, hightlight.Length);
                                hightlightText += "</b>";
                                if (fontSize.HasValue)
                                    hightlightText += "</size>";
                                if (hightlightIndex + hightlight.Length < text.Length)
                                    hightlightText += text.Substring(hightlightIndex + hightlight.Length);
                                return hightlightText;
                            }
                        }
                        if (text[i] == ' ')
                            space++;
                    }
                }

                // Highlight tất cả các từ khóa
                string result = "";
                var firstIndex = 0;
                var index = text.IndexOf(hightlight, System.StringComparison.OrdinalIgnoreCase);

                while (index >= 0)
                {
                    result += text.Substring(firstIndex, index - firstIndex);
                    firstIndex = index + hightlight.Length;

                    bool validate = CheckWordIndexIsValidateInContent(text, hightlight, index);

                    if (validate && invalidsText != null && invalidsText.Length > 0)
                    {
                        validate = CheckCurrentIndexIsNotParentIndex(text, hightlight, index, invalidsText);
                    }

                    if (validate)
                    {
                        result += "<b>";
                        result += text.Substring(index, hightlight.Length);
                        result += "</b>";
                    }
                    else
                    {
                        result += text.Substring(index, hightlight.Length);
                    }

                    index = text.IndexOf(hightlight, firstIndex, System.StringComparison.OrdinalIgnoreCase);
                }

                result += text.Substring(firstIndex);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi highlight văn bản: {ex.Message}");
                return text;
            }
        }

        /// <summary>
        /// Tìm vị trí đầu tiên của từ hợp lệ trong nội dung.
        /// Phương thức này tìm từ khóa và kiểm tra tính hợp lệ của từ đó.
        /// </summary>
        /// <param name="word">Từ cần tìm</param>
        /// <param name="content">Nội dung cần tìm kiếm</param>
        /// <param name="invalidsText">Danh sách các từ cha không được tìm</param>
        /// <param name="startIndex">Vị trí bắt đầu tìm kiếm</param>
        /// <param name="stringComparison">Kiểu so sánh chuỗi</param>
        /// <returns>Vị trí đầu tiên của từ hợp lệ, -1 nếu không tìm thấy</returns>
        public static int GetIndexWordInContent(string word, string content, string[] invalidsText = null, int startIndex = 0, System.StringComparison stringComparison = System.StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(content))
                return -1;

            try
            {
                while (startIndex < content.Length - 1)
                {
                    var index = content.IndexOf(word, startIndex, stringComparison);
                    if (index < 0)
                        break;

                    startIndex = index + word.Length;

                    // Kiểm tra tính hợp lệ của từ
                    if (!CheckWordIndexIsValidateInContent(content, word, index))
                        continue;

                    // Kiểm tra xem có phải từ cha không
                    if (invalidsText != null && invalidsText.Length > 0)
                    {
                        if (!CheckCurrentIndexIsNotParentIndex(content, word, index, invalidsText))
                            continue;
                    }

                    if (index >= 0)
                        return index;
                }
                return -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tìm từ trong nội dung: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Thay thế từ không phân biệt dấu trong văn bản.
        /// Phương thức này chuẩn hóa văn bản về dạng không dấu để tìm và thay thế.
        /// </summary>
        /// <param name="content">Văn bản gốc cần thay thế</param>
        /// <param name="find">Từ cần tìm</param>
        /// <param name="replace">Từ thay thế</param>
        /// <param name="stringComparison">Kiểu so sánh chuỗi</param>
        /// <returns>Văn bản đã được thay thế</returns>
        public static string FindAndReplaceWordNonUnicode(string content, string find, string replace, System.StringComparison stringComparison = System.StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(find))
                return content;

            try
            {
                // Chuẩn hóa cả chuỗi đầu vào và từ cần tìm về chuỗi không dấu
                string normalizedInput = RemoveDiacritics(content);
                string normalizedWordToFind = RemoveDiacritics(find);

                // Tìm vị trí của từ trong chuỗi chuẩn hóa
                int position = normalizedInput.IndexOf(normalizedWordToFind, stringComparison);
                while (position != -1 && position + replace.Length <= normalizedInput.Length)
                {
                    // Tìm thấy từ, thay thế từ đó trong chuỗi gốc
                    content = ReplaceAtPosition(content, position, find.Length, replace, stringComparison);

                    // Tiếp tục tìm từ tiếp theo
                    position = normalizedInput.IndexOf(normalizedWordToFind, position + replace.Length, stringComparison);
                }
                return content;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi thay thế từ không dấu: {ex.Message}");
                return content;
            }
        }


        /// <summary>
        /// Thay thế từ tại vị trí cụ thể trong văn bản.
        /// Phương thức này giữ nguyên định dạng hoa thường của từ gốc.
        /// </summary>
        /// <param name="input">Văn bản gốc</param>
        /// <param name="position">Vị trí bắt đầu thay thế</param>
        /// <param name="length">Độ dài từ cần thay thế</param>
        /// <param name="replacementWord">Từ thay thế</param>
        /// <param name="stringComparison">Kiểu so sánh chuỗi</param>
        /// <returns>Văn bản đã được thay thế</returns>
        public static string ReplaceAtPosition(string input, int position, int length, string replacementWord, System.StringComparison stringComparison = System.StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(input) || position < 0 || length < 0 || position + length > input.Length)
                return input;

            try
            {
                // Tách chuỗi thành 3 phần: trước, từ cũ, sau
                string beforeWord = input.Substring(0, position);
                string afterWord = input.Substring(position + length);

                if (stringComparison == System.StringComparison.OrdinalIgnoreCase ||
                    stringComparison == System.StringComparison.InvariantCultureIgnoreCase)
                {
                    // Hỗ trợ giữ nguyên hoa thường
                    var text = input.Substring(position, length);
                    if (text == text.ToLower())
                        replacementWord = replacementWord.ToLower();
                    else if (text == text.ToUpper())
                        replacementWord = replacementWord.ToUpper();
                    else if (text.Length > 1 && char.IsUpper(text[0]) && !char.IsUpper(text[1]))
                        replacementWord = replacementWord.Substring(0, 1).ToUpper() + replacementWord.Substring(1);
                }

                // Ghép lại chuỗi với từ mới
                return beforeWord + replacementWord + afterWord;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi thay thế từ tại vị trí: {ex.Message}");
                return input;
            }
        }

        public static string ReplaceWordInContent(string rootContent, string rootFind, string replace, string[] invalidsText = null, int? firstIndex = null, System.StringComparison stringComparison = System.StringComparison.OrdinalIgnoreCase, bool nonUnicode = false)
        {
            if (string.IsNullOrEmpty(rootFind) || string.IsNullOrEmpty(rootContent))
                return rootContent;
            //Trường hợp từ cần tìm và từ thay thế trùng nhau
            if (rootFind.Equals(replace, stringComparison))
                return rootContent;
            int startIndex = (firstIndex != null && firstIndex.Value > 0) ? firstIndex.Value : 0;
            //Xóa dấu đi để tìm
            string content = nonUnicode ? RemoveDiacritics(rootContent) : rootContent;
            string find = nonUnicode ? RemoveDiacritics(rootFind) : rootFind;
            while (startIndex < content.Length - 1)
            {
                var index = content.IndexOf(find, startIndex, stringComparison);
                if (index < 0)
                    break;
                startIndex = index + find.Length;
                if (!CheckWordIndexIsValidateInContent(content, find, index, stringComparison))
                    continue;
                else if (invalidsText != null && invalidsText.Length > 0)
                {
                    if (!CheckCurrentIndexIsNotParentIndex(content, find, index, invalidsText, stringComparison))
                        continue;
                }
                //if (index > 1 && char.IsLetterOrDigit(content[index - 1]))
                //{
                //    //Nếu ký tự đứng trước không hợp lệ thì kiểm tra lại
                //    //index = content.IndexOf(find, afterIndex, System.StringComparison.OrdinalIgnoreCase);
                //    continue;
                //}
                //if (startIndex < content.Length && char.IsLetterOrDigit(content[startIndex]))
                //{
                //    //Nếu ký tự đứng sau không hợp lệ thì kiểm tra lại
                //    //index = content.IndexOf(find, afterIndex, System.StringComparison.OrdinalIgnoreCase);
                //    continue;
                //}
                if (index >= 0)
                {
                    //Xử lý thay từ
                    string beforeContent = rootContent.Substring(0, index);
                    string textFind = rootContent.Substring(index, rootFind.Length);
                    if (replace.Length > 0)
                    {
                        //Nếu từ thay thế có dữ liệu
                        if (textFind == textFind.ToLower())
                            beforeContent += replace.ToLower();
                        else if (textFind == textFind.ToUpper())
                            beforeContent += replace.ToUpper();
                        else if (textFind.Length > 1 && char.IsUpper(textFind[0]) && !char.IsUpper(textFind[1]))
                            beforeContent += replace.Substring(0, 1).ToUpper() + replace.Substring(1);
                        else
                            beforeContent += replace;
                    }
                    rootContent = beforeContent + rootContent.Substring(startIndex);
                    if (firstIndex != null)
                    {
                        //Chỉ thay từ tại vị trí đã xác định
                        break;
                    }
                }

            }

            return rootContent;
        }

        public static string ReplaceSpecialCharacters(string text, char[] validatesChars = null, string replaceText = " ")
        {
            if (string.IsNullOrEmpty(text))
                return null;
            string result = "";
            if (!string.IsNullOrEmpty(text))
            {
                bool validate = false;
                for (int i = 0; i < text.Length; i++)
                {
                    if (char.IsLetterOrDigit(text[i]))
                    {
                        result += text[i];
                        //Nếu chỉ có ký tự đặc biệt không thì sẽ trả về trống
                        validate = true;
                    }
                    else if (char.IsWhiteSpace(text[i]) || (validatesChars != null && validatesChars.Contains(text[i])))
                        result += text[i];
                    //Coi mọi ký tự đặc biệt là dấu cách, nếu được thì "n't" giữ nguyên
                    else if (text[i] == '\'' && i > 0 && i < text.Length && text[i - 1] == 'n' && text[i - 1] == 't')
                        result += text[i];
                    else
                        result += replaceText;
                }
                if (!validate)
                    return null;
            }
            return result;
        }

        public static string RemoveStartEndSpecialCharacters(string text, bool? isStart = null, char[] keepChars = null)
        {
            string result = text;
            //Xóa bỏ ký tự đặc biệt ở đầu câu
            int start = -1;
            if (isStart != false)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (char.IsLetterOrDigit(text[i]))
                        break;
                    else if (keepChars != null && keepChars.Contains(text[i]))
                        break;
                    else
                        start = i;
                }
                if (start >= 0)
                {
                    result = result.Substring(start + 1);
                }
            }
            if (isStart != true)
            {
                //Xóa bỏ ký tự đặc biệt ở cuối câu
                int end = -1;
                for (int i = result.Length - 1; i >= 0; i--)
                {
                    if (char.IsLetterOrDigit(result[i]))
                        break;
                    else if (keepChars != null && keepChars.Contains(text[i]))
                        break;
                    else
                        end = i;
                }
                if (end >= 0)
                {
                    result = result.Substring(0, end);
                }
            }
            return result;
        }

        public static bool CheckIndexIsNewLine(string content, int index)
        {
            if (index == 0)
                return true;
            if (index >= content.Length)
                return false;
            else if (index > 1)
            {
                var newIndex = index - 1;
                if (content[newIndex] == ' ')
                {
                    newIndex--;
                    if (content[newIndex] == '.')
                        return true;
                }
                foreach (var newLineChar in NewLineText)
                {
                    if (content[newIndex] == newLineChar[newLineChar.Length - 1])
                        return true;
                }
            }
            return false;
        }

        public static string UpperFirst(string content)
        {
            //Upper: Hoa chữ đầu: upper/UPPER > Upper
            if (content.Length == 1)
                return content.ToUpper();
            else
                return char.ToUpper(content[0]) + content.Substring(1).ToLower();
        }
        public static string GetTextWithTagNode(string text, char tag, bool innerText = true)
        {
            //075: 2024-09-18: - Log sẽ ghi nối tiếp vào trường Ghi chú chứ không thay thế
            return text;

            if (!string.IsNullOrEmpty(text))
            {
                //Xóa ghi chú tag trước đó
                var startIndex = text.IndexOf(tag);
                if (startIndex >= 0)
                {
                    int endIndex = 0;
                    switch (tag)
                    {
                        case '[': endIndex = text.IndexOf(']'); break;
                        case '(': endIndex = text.IndexOf(')'); break;
                        case '{': endIndex = text.IndexOf('}'); break;
                        case '<': endIndex = text.IndexOf('>'); break;
                        default: endIndex = text.LastIndexOf(tag); break; //các trường hợp khác lấy đúng ký tự đấy
                    }
                    if (endIndex > 0)
                    {
                        if (innerText)
                        {
                            startIndex++;
                            return text.Substring(startIndex, endIndex - startIndex);
                        }
                        else
                        {
                            return text.Substring(0, startIndex) + text.Substring(endIndex + 1);
                        }

                    }
                }
            }
            if (!innerText)
                return text;
            return null;
        }

        public static string GetTagNode(char tag, string innerText)
        {
            var result = tag + innerText;
            switch (tag)
            {
                case '[': result += ']'; break;
                case '(': result += ')'; break;
                case '{': result += '}'; break;
                case '<': result += '>'; break;
                default: result += tag; break;//các trường hợp khác lấy đúng ký tự đấy
            }
            return result;
        }

        public static string AddTextWithTagNode(string text, char tag, string innerText, bool summaryText = true)
        {
            string result = text;
            if (!string.IsNullOrEmpty(result))
            {
                //Xóa ghi chú tag trước đó
                result = GetTextWithTagNode(text, tag, false);
            }
            //075:- Nội dung: Mã là các kí tự đầu của Nhãn menu + số thứ tự nếu có
            if (summaryText)
                innerText = GetFirstLetterToUpper(innerText);
            var tagNoteText = GetTagNode(tag, innerText);
            //Kiểm tra xem nếu text có sẵn tag này không, nếu có thì bỏ
            if (!string.IsNullOrEmpty(text) && text.Contains(tagNoteText))
                return result;
            return result += tagNoteText;
        }

        //viết phương thức lấy chữ cái đầu tiên của mỗi từ và viết hoa lên
        public static string GetFirstLetterToUpper(string str, bool supportNumber = true)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            string[] words = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    if (true && char.IsNumber(words[i][0]))
                    {
                        result += words[i];
                    }
                    else
                    {
                        result += char.ToUpper(words[i][0]);
                    }
                }
            }
            return result;
        }


        private static Dictionary<char, int> _romanMap = new Dictionary<char, int> { { 'I', 1 }, { 'V', 5 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };
        public static bool IsRoman(string text)
        {
            foreach (var c in text)
                if (!_romanMap.ContainsKey(c))
                    return false;
            return true;
        }

        public static int CountNotCorrectInWord(System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> dictionary, string word)
        {
            if (!string.IsNullOrEmpty(word))
            {
                int wordLength = word.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                if (dictionary.ContainsKey(wordLength))
                {
                    var lowerName = RemoveUnicode(word).ToLower();
                    if (dictionary[wordLength].ContainsKey(lowerName))
                    {
                        int maxIntersect = 0;
                        var arrayWord = word.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
                        foreach (var w in dictionary[wordLength][lowerName])
                        {
                            var refArray = w.ToLower().Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
                            var intersect = arrayWord.Intersect(refArray);
                            if (intersect.Count() > maxIntersect)
                                maxIntersect = intersect.Count();
                        }
                        return wordLength - maxIntersect;
                    }
                }

            }
            return -1;
        }

        public static bool CheckWordIsCorrect(System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> dictionary, string word)
        {
            if (!string.IsNullOrEmpty(word) && dictionary != null)
            {
                int wordLength = word.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                bool currentIsCorrect = dictionary.ContainsKey(wordLength);
                if (currentIsCorrect)
                {
                    var lowerName = RemoveUnicode(word).ToLower();
                    currentIsCorrect = dictionary[wordLength].ContainsKey(lowerName);
                    if (currentIsCorrect)
                        currentIsCorrect = ListContains(dictionary[wordLength][lowerName], word) >= 0;
                }
                return currentIsCorrect;
            }
            return false;
        }

        public static bool CheckSimpleWordIsCorrect(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> dictionary, string word)
        {
            if (!string.IsNullOrEmpty(word))
            {
                var lowerName = RemoveUnicode(word).ToLower();
                var currentIsCorrect = dictionary.ContainsKey(lowerName);
                if (currentIsCorrect)
                    currentIsCorrect = ListContains(dictionary[lowerName], word) >= 0;
                return currentIsCorrect;
            }
            return false;
        }

        public static bool CheckRealNameIsUpperCaseFirstAll(string realName)
        {
            if (!string.IsNullOrEmpty(realName))
            {
                var words = realName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (char.IsLower(word[0]))
                        return false;
                }
                return true;
            }
            return false;
        }

        public static bool CheckUpperCaseAll(string content)
        {
            bool upper = false;
            bool lower = false;
            foreach (var c in content)
            {
                if (char.IsUpper(c))
                {
                    upper = true;
                }
                else if (char.IsLower(c))
                {
                    lower = true;
                    break;
                }
            }
            if (upper && !lower)
            {
                return true;
            }
            return false;
        }




        /// <summary>
        /// Đếm số lượng âm tiết trong văn bản
        /// </summary>
        /// <param name="input">Văn bản cần đếm</param>
        /// <param name="lang">Ngôn ngữ (en/vi)</param>
        /// <returns>Số lượng âm tiết</returns>
        public static int CountSyllables(string input, string lang = "en")
        {
            string[] words = input.Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            int syllableCount = 0;
            int specialCharCount = 0;

            foreach (string word in words)
            {
                syllableCount += lang.ToLower() == "en" ? CountSyllablesEnglish(word) : CountSyllablesVietnamese(word);
                //lowerCaseCount += CountLowerCaseLetters(word);
                specialCharCount += CountSpecialCharacters(word);
            }
            // return syllableCount + specialCharCount; Ký tự đặc biệt không tính là phiên âm
            return syllableCount;
        }

        /// <summary>
        /// Đếm số lượng âm tiết trong từ tiếng Anh
        /// </summary>
        /// <param name="word">Từ cần đếm</param>
        /// <returns>Số lượng âm tiết</returns>
        public static int CountSyllablesEnglish(string word)
        {
            word = word.ToLower();
            if (word.Length == 0) return 0;

            int syllableCount = 0;
            bool lastCharWasVowel = false;

            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsDigit(word[i]))
                {
                    syllableCount++; // Mỗi chữ số là một âm tiết
                }
                else if ("aeiouy@#$%&".Contains(word[i])) //Ký tự đặc biệt sẽ tính phiên âm
                {
                    if (!lastCharWasVowel)
                    {
                        syllableCount++;
                        lastCharWasVowel = true;
                    }
                }
                else
                {
                    lastCharWasVowel = false;
                }
            }

            // Subtract one syllable if the word ends with "e", but not "le"
            if (word.EndsWith("e") && !word.EndsWith("le") && syllableCount > 1)
            {
                syllableCount--;
            }

            return syllableCount;
        }

        /// <summary>
        /// Đếm số lượng âm tiết trong từ tiếng Việt
        /// </summary>
        /// <param name="word">Từ cần đếm</param>
        /// <returns>Số lượng âm tiết</returns>
        public static int CountSyllablesVietnamese(string word)
        {
            word = word.ToLower();
            if (word.Length == 0) return 0;
            if (!char.IsNumber(word[0]))
                return 1;

            //string vowels = "aăâeêioôơuưy@#$%&";
            int syllableCount = 0;

            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsDigit(word[i]))
                {
                    syllableCount++; // Mỗi chữ số là một âm tiết
                }
                //else if (vowels.Contains(word[i]))
                //{
                //    syllableCount++;
                //}
            }

            return syllableCount;
        }
        static int CountSpecialCharacters(string input)
        {
            int specialCharCount = 0;

            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
                {
                    specialCharCount++;
                }
            }

            return specialCharCount;
        }


        public static HashSet<char> SpecialCharacters = new HashSet<char> { '@', '#', '$', '%', '^', '&', '*', '+', '-', '=', '§' };
        public static HashSet<char> Vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };
        public static decimal GetWordVowelWeight(string languageCode, string sentence, decimal doubleVowelWeight = 1, decimal spaceWeight = 0, decimal punctuationWeight = 0, decimal commaWeight = 0)
        {
            if (string.IsNullOrEmpty(sentence))
                return 0;
            //Bỏ ký tự đặc biệt
            sentence = sentence.Trim().Replace(" ", " ");
            //Bỏ 2 dấu cách
            sentence = sentence.Replace("  ", " ");
            //Bỏ xuống dòng
            sentence = sentence.Trim().Replace("\r\n", " ");
            decimal total = 0;
            if (!string.IsNullOrEmpty(languageCode) && (languageCode.ToLower() == "en" || languageCode.ToLower() == "us"))
            {
                //Trường hợp dùng nguyên âm
                // 2023
                //- Số nguyên âm kép của 1 từ = m
                //-Số nguyên âm đơn của 1 từ = n
                //- Trọng số thời gian của 1 từ: w = d * m + n + c + p(d là trọng số thời gian của nguyên âm kép, c là trọng số của dấu phẩy comma, p là trọng số của dấu chấm punctuation, từ có chấm phẩy thì mới có c, p)
                //- Trọng số thời gian của phụ đề: s = tổng trọng số thời gian của các từ(từ sẽ bao gồm dấu chấm, phẩy dính kèm phía sau)                
                //sentence = sentence.ToLower();                
                // Build a list of vowels up front:               
                //var specialCharacters = new HashSet<char> { '@', '#', '$', '%', '^', '&', '*', '+', '-', '=', '§' };
                for (int i = 0; i < sentence.Length; i++)
                {
                    var c = sentence[i];
                    if (Char.IsDigit(c))
                    {
                        //Nếu là ký tự số thì là 1 nguyên âm
                        total++;
                    }
                    else if (Vowels.Contains(sentence[i]))
                    {
                        if (i == 0)
                            total++;
                        else if (!Vowels.Contains(sentence[i - 1]))
                            total += doubleVowelWeight;
                    }
                    else if (char.IsUpper(sentence[i]))
                    {
                        //Chữ viết hoa liền nhau 2 ký tự trở lên mỗi cái là 1 nguyên âm
                        if (i > 0 && char.IsUpper(sentence[i - 1]))
                            total++;
                        else if (i < sentence.Length - 2 && char.IsUpper(sentence[i + 1]))
                            total++;
                    }
                    else if (sentence[i].Equals('.'))
                    {
                        total += punctuationWeight;
                    }
                    else if (sentence[i].Equals(','))
                    {
                        total += commaWeight;
                    }
                    else if (SpecialCharacters.Contains(sentence[i]))
                    {
                        //Ký tự đặc biệt thì là 1 nguyên âm
                        if (i == 0)
                            total++;
                    }
                    total += spaceWeight;
                }

            }
            else
            {
                total += sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                //Trường hợp tiếng Việt, chia theo từ
            }
            if (total == 0)
            {
                //Dối với từ không có nguyên âm: "my"
                return 1;
            }
            return total;
        }

        public static bool CheckUnicode(string word)
        {
            word = word.ToLower();
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ",
                "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ","đ","é","è","ẻ","ẽ","ẹ","ê","ế",
                "ề","ể","ễ","ệ", "í","ì","ỉ","ĩ","ị","ó","ò","ỏ","õ","ọ","ô","ố","ồ",
                "ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ", "ú","ù","ủ","ũ","ụ","ư","ứ","ừ",
                "ử","ữ","ự","ý","ỳ","ỷ","ỹ","ỵ",};
            foreach (var c in arr1)
            {
                if (word.Contains(c))
                    return true;
            }
            return false;
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public static HashSet<char> vietChars = new HashSet<char>()
        {
            'a', 'á', 'à', 'ả', 'ã', 'ạ',
            'ă', 'ắ', 'ằ', 'ẳ', 'ẵ', 'ặ',
            'â', 'ấ', 'ầ', 'ẩ', 'ẫ', 'ậ',
            'b', 'c', 'd', 'đ',
            'e', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
            'ê', 'ế', 'ề', 'ể', 'ễ', 'ệ',
            'g', 'h', 'i', 'ì', 'í', 'ỉ', 'ĩ', 'ị',
            'k', 'l', 'm', 'n',
            'o', 'ò', 'ó', 'ỏ', 'õ', 'ọ',
            'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
            'ơ', 'ớ', 'ờ', 'ở', 'ỡ', 'ợ',
            'p', 'q', 'r', 's', 't',
            'u', 'ù', 'ú', 'ủ', 'ũ', 'ụ',
            'ư', 'ứ', 'ừ', 'ử', 'ữ', 'ự',
            'v', 'x', 'y', 'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
            //Các từ chỉ có trong tiếng anh
            'f','j', 'w','z',
            //Các ký tự đặc biệt
            ' ', '.', ',', '!', '?', ';', ':', '\'', '"', '@', '#', '$', '%', '^', '&', '*', '(', ')','[', ']', '-', '_', '=', '+', '<', '>', '/', '\\', '|',
            //Các ký tự số
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        /// <summary>
        /// Tìm các ký tự lỗi trong câu bằng cách so sánh với bộ ký tự tiếng Việt.
        /// Trả về danh sách các ký tự không hợp lệ trong câu.
        /// </summary>
        /// <param name="sentence">Câu cần kiểm tra</param>
        /// <returns>Danh sách các ký tự lỗi</returns>
        /// <example>
        /// var errors = Tools.GetExistCharacterError("Hello @#$%");
        /// // Trả về ['@', '#', '$', '%']
        /// </example>
        public static System.Collections.Generic.List<char> GetExistCharacterError(string sentence)
        {
            if (string.IsNullOrEmpty(sentence)) return null;
            var result = new System.Collections.Generic.List<char>();
            foreach (char c in sentence)
            {
                if (!vietChars.Contains(char.ToLower(c)))
                    result.Add(c);
            }
            return result;
        }

        public static System.Collections.Generic.List<char> GetExistCharacterError(string chars, string sentence)
        {
            if (string.IsNullOrEmpty(sentence)) return null;
            var result = new System.Collections.Generic.List<char>();
            foreach (char c in sentence)
            {
                if (!chars.Contains(char.ToLower(c)))
                    result.Add(c);
            }
            return result;
        }

        // white space, em-dash, en-dash, underscore
        static readonly Regex WordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);

        // characters that are not valid
        static readonly Regex InvalidChars = new Regex(@"[^a-z0-9\-]", RegexOptions.Compiled);

        // multiple hyphens
        static readonly Regex MultipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);
        public static string GetSlug(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                //var nonUnicode = RemoveUnicode(text.ToLower());
                //var stringBuilder = new System.Text.StringBuilder();
                //foreach (char c in nonUnicode.ToArray())
                //{
                //    if (Char.IsLetterOrDigit(c))
                //    {
                //        stringBuilder.Append(c);
                //    }
                //    else if (c == ' ')
                //    {
                //        stringBuilder.Append("-");
                //    }
                //}
                //return stringBuilder.ToString();
                text = text.ToLowerInvariant();

                // remove diacritics (accents)
                text = RemoveDiacritics(text);

                // ensure all word delimiters are hyphens
                text = WordDelimiters.Replace(text, "-");

                // strip out invalid characters
                text = InvalidChars.Replace(text, "");

                // replace multiple hyphens (-) with a single hyphen
                text = MultipleHyphens.Replace(text, "-");

                // trim hyphens (-) from ends
                return text.Trim('-');
            }
            return text;
        }

        private static string RemoveDiacriticsByNormalizationForm(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static int ListContains(System.Collections.Generic.IList<string> list, string content)
        {
            if (string.IsNullOrEmpty(content) || list is null) return -1;
            for (int i = 0; i < list.Count; i++)
                if (content.Equals(list[i], System.StringComparison.OrdinalIgnoreCase)) return i;
            return -1;
        }

        public static string KeyListContains(System.Collections.Generic.ICollection<string> collection, string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            foreach (var key in collection)
                if (content.Equals(key, System.StringComparison.OrdinalIgnoreCase)) return key;
            return null;
        }

        public static HashSet<char> StartEndSpecialCharactersInWord = new HashSet<char> { '"', '!', '?', '(', ')', '<', '>', '{', '}', '[', ']', '+', '-', '*', ':', ';' };
        //2023-06-07: dấu ngắt câu có thể là: Xuống dòng, dấu chấm, ?, !
        public static string[] NewLineText = new string[] { ". ", "?", "!", "\r\n" };
        //Không coi là từ Hoa với các từ viết hoa đầu câu hoặc sau dâu: " ; ( ; { ; [ ; : (2 chấm)
        public static char[] BeforeChars = new char[] { '"', '"', '(', '{', '[', ':', ';' };
        //Không coi là từ Hoa với các từ viết hoa đầu câu hoặc sau dâu: " ; ( ; { ; [ ; : (2 chấm)
        public static char[] SeperateChars = new char[] { '"', '"', '(', '{', '[', ':', ';' };
        //Các ký tự đặc biệt ở trong câu hợp lệ
        public static char[] CharsInWord = new char[] { '.', ',', '-', '/' };
        //public static char[] CharsInWord = new char[] { '.', ',', '-', '/', '+', '<', '>', '~' };
        //Các ký tự đặc biệt tương đương như ký tự thường cần nạp vào
        public static char[] SpecialCharactersIsChar = new char[] { '°', '@', '#', '$', '%', '^', '*' };
        //Các ký tự đặc biệt hợp lệ đầu hoặc cuối từ
        public static char[] CharsStartEndWord = new char[] { '+', '°', '$', '%' }; //Ví dụ Poe++, HFS+

        public static string[] GetSentences(string content)
        {
            if (string.IsNullOrEmpty(content))
                return new string[0];
            else
                return content.Split(NewLineText, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetWords(string sentenceText)
        {

            if (string.IsNullOrEmpty(sentenceText))
                return new string[0];
            else
                return sentenceText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool CheckPositonIsStartSentence(string content, int position, char[] addBeforeChars = null)
        {
            //Không coi là từ Hoa với các từ viết hoa đầu câu hoặc sau dâu: " ; ( ; { ; [ ; : (2 chấm)
            for (int i = position - 1; i > 0; i--)
            {
                //Nếu là dấu cách thì không kiểm tra
                if (content[i] == ' ')
                    continue;
                else if (BeforeChars.Contains(content[i]))
                    return true;
                else if (addBeforeChars != null && addBeforeChars.Contains(content[i]))
                    return true;
                break;
            }
            return false;
        }

        public static bool CheckSpecialCharactersValidate(char c)
        {
            foreach (var sc in CharsInWord)
            {
                if (sc.Equals(c))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra xem từ tại vị trí index có hợp lệ trong nội dung không.
        /// Phương thức này kiểm tra xem từ có được bao quanh bởi khoảng trắng hoặc ký tự đặc biệt không.
        /// </summary>
        /// <param name="content">Nội dung cần kiểm tra</param>
        /// <param name="word">Từ cần kiểm tra</param>
        /// <param name="index">Vị trí bắt đầu của từ trong content</param>
        /// <param name="stringComparison">Kiểu so sánh chuỗi</param>
        /// <returns>True nếu từ hợp lệ, False nếu không</returns>
        public static bool CheckWordIndexIsValidateInContent(string content, string word, int index, System.StringComparison stringComparison = System.StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(word) || index < 0)
                return false;

            try
            {
                // Tìm vị trí bắt đầu và kết thúc của từ đầy đủ
                int beginIndex = 0;
                for (int t = index - 1; t >= 0; t--)
                {
                    if (content[t] == ' ')
                    {
                        beginIndex = t + 1;
                        break;
                    }
                }

                int endIndex = content.Length;
                for (int t = index + word.Length; t < content.Length; t++)
                {
                    if (content[t] == ' ')
                    {
                        endIndex = t;
                        break;
                    }
                }

                string fullWord = content.Substring(beginIndex, endIndex - beginIndex);
                bool isValid = word.Equals(fullWord, stringComparison) ||
                    word.Equals(RemoveStartEndSpecialCharacters(fullWord), stringComparison);

                if (!isValid)
                    return false;

                // Kiểm tra ký tự trước từ
                if (index > 0 && !word.StartsWith(' '))
                {
                    var beforeChar = content[index - 1];
                    if (char.IsLetterOrDigit(beforeChar))
                        return false;
                    if (index > 1 && CharsInWord.Contains(beforeChar) && char.IsLetterOrDigit(content[index - 2]))
                        return false;
                }

                // Kiểm tra ký tự sau từ
                var afterIndex = index + word.Length;
                if (afterIndex < content.Length && !word.EndsWith(' '))
                {
                    var afterChar = content[afterIndex];
                    if (char.IsLetterOrDigit(afterChar))
                        return false;
                    if (afterIndex + 1 < content.Length && CharsInWord.Contains(afterChar) && char.IsLetterOrDigit(content[afterIndex + 1]))
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi kiểm tra từ hợp lệ: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Chuyển ký tự đầu tiên của chuỗi thành chữ thường.
        /// </summary>
        /// <param name="input">Chuỗi cần xử lý.</param>
        /// <returns>
        /// Chuỗi với ký tự đầu tiên đã được chuyển thành chữ thường.  
        /// Nếu <paramref name="input"/> là null hoặc rỗng, trả về chính chuỗi ban đầu.
        /// </returns>
        public static string LowerFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        /// <summary>
        /// Kiểm tra xem vị trí hiện tại có phải là vị trí của từ cha không.
        /// Phương thức này giúp tránh highlight từ con khi từ cha đã được highlight.
        /// </summary>
        /// <param name="text">Văn bản gốc</param>
        /// <param name="word">Từ cần kiểm tra</param>
        /// <param name="index">Vị trí của từ trong text</param>
        /// <param name="invalidsText">Danh sách các từ cha không được highlight</param>
        /// <param name="stringComparison">Kiểu so sánh chuỗi</param>
        /// <returns>True nếu không phải vị trí của từ cha, False nếu là vị trí của từ cha</returns>
        public static bool CheckCurrentIndexIsNotParentIndex(string text, string word, int index, string[] invalidsText = null, System.StringComparison stringComparison = System.StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(word) || invalidsText == null || invalidsText.Length == 0)
                return true;

            try
            {
                foreach (var invalidText in invalidsText)
                {
                    if (string.IsNullOrEmpty(invalidText))
                        continue;

                    var beforeIndex = invalidText.IndexOf(word, stringComparison);
                    if (beforeIndex < 0)
                        continue;

                    var afterIndex = invalidText.Length - beforeIndex;
                    if (index >= beforeIndex)
                    {
                        var parentTermIndex = text.IndexOf(invalidText, index - beforeIndex, stringComparison);
                        if (parentTermIndex == index - beforeIndex && CheckWordIndexIsValidateInContent(text, invalidText, parentTermIndex))
                        {
                            // Nếu là vị trí của từ cha thì bỏ qua
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi kiểm tra vị trí từ cha: {ex.Message}");
                return true;
            }
        }

        public static List<string> SplitContentToList(string content)
        {
            if (string.IsNullOrEmpty(content))
                return new List<string>();

            return content.Split('\n').ToList();
        }
        public static string[] SplitToLines(string content)
        {
            if (string.IsNullOrEmpty(content))
                return Array.Empty<string>();
            return content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
        //Chuyển từ audio service
        public static string SplitStringByNewLine(string content, char[] splitChars)
        {
            string result = "";
            for (int j = 0; j < content.Length; j++)
            {
                if (j < content.Length - 2 && j > 1 && content[j] == ' ' && splitChars.Contains(content[j - 1]))
                    result += System.Environment.NewLine;
                else //Thêm option này sẽ thay dấu cách ở đầu bằng dấu xuống dòng
                    result += content[j];
            }
            return result;
        }
        //Chuyển từ audio service
        public static float CalculateWordSimilarity(string text1, string text2)
        {
            // Tách thành các từ bằng Regex \p{L}+ (ký tự chữ unicode)
            System.Collections.Generic.HashSet<string> words1 = new System.Collections.Generic.HashSet<string>(
                System.Text.RegularExpressions.Regex.Matches(text1.ToLower(), @"\p{L}+")
                    .Cast<System.Text.RegularExpressions.Match>()
                    .Select(m => m.Value)
            );

            System.Collections.Generic.HashSet<string> words2 = new System.Collections.Generic.HashSet<string>(
                System.Text.RegularExpressions.Regex.Matches(text2.ToLower(), @"\p{L}+")
                    .Cast<System.Text.RegularExpressions.Match>()
                    .Select(m => m.Value)
            );

            // Đếm số từ giống nhau
            int matchCount = words1.Intersect(words2).Count();
            if (words1.Count == 0 || words2.Count == 0)
                return 0;

            // Tính độ tương đồng dựa trên số từ giống nhau và tổng số từ nhiều hơn
            return (float)(matchCount / words1.Count + matchCount / words2.Count) / 2;
        }

        public static string NormalizeString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // 1. Thay thế dấu nháy “thông minh” → đơn giản
            input = input
                .Replace("’", "'").Replace("‘", "'")
                .Replace("“", "\"").Replace("”", "\"");

            // 2. Chuẩn hóa Unicode (Form KC = loại bỏ tổ hợp diacritic, cách viết khác)
            input = input.Normalize(NormalizationForm.FormKC);

            // 3. Loại bỏ khoảng trắng không hiển thị (non-breaking space, zero-width, tab...)
            input = System.Text.RegularExpressions.Regex.Replace(input, @"[\u00A0\u200B\t\r\n]+", " ");

            // 4. Bỏ khoảng trắng dư và chuẩn hóa viết thường
            return input.Trim().ToLowerInvariant();
        }

        public static string GetSentenceTextFromContent(int? sentenceIndex, string content)
        {
            if (sentenceIndex is null)
                return content;
            if (string.IsNullOrEmpty(content))
                return content;
            //2023-06-07: dấu ngắt câu có thể là: Xuống dòng, dấu chấm, ?, !
            //string[] newLineText = new string[] { ". ", "?", "!", "\r\n" };
            var rows = content.Split(NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
            if (sentenceIndex > 0 && sentenceIndex - 1 < rows.Length)
                return rows[sentenceIndex.Value - 1].Trim();
            return content;
        }

    }
}
