using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace ENTOS.Module.Extensions
{
    /// <summary>
    /// Extension method mở rộng cho kiểu string.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Loại bỏ ký tự unicode khỏi chuỗi.
        /// </summary>
        public static string RemoveUnicode(this string text)
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
        /// Tìm vị trí đầu tiên của một chuỗi con, với tùy chọn bỏ qua dấu và phân biệt hoa thường.
        /// </summary>
        /// <param name="source">Chuỗi nguồn để tìm kiếm.</param>
        /// <param name="value">Chuỗi con cần tìm.</param>
        /// <param name="comparisonType">Tùy chọn so sánh chuỗi (hoa/thường).</param>
        /// <param name="ignoreDiacritics">True để bỏ qua dấu khi tìm kiếm.</param>
        /// <returns>Vị trí của chuỗi con, hoặc -1 nếu không tìm thấy.</returns>
        public static int IndexOfDiacriticsAware(this string source, string value, StringComparison comparisonType = StringComparison.Ordinal, bool ignoreDiacritics = false)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
            {
                return -1;
            }

            if (!ignoreDiacritics)
            {
                return source.IndexOf(value, comparisonType);
            }

            string normalizedSource = source.RemoveUnicode();
            string normalizedValue = value.RemoveUnicode();

            return normalizedSource.IndexOf(normalizedValue, comparisonType);
        }

        /// <summary>
        /// Kiểm tra xem chuỗi có chứa một chuỗi con hay không, với tùy chọn bỏ qua dấu và phân biệt hoa thường.
        /// </summary>
        /// <param name="source">Chuỗi nguồn để tìm kiếm.</param>
        /// <param name="value">Chuỗi con cần kiểm tra.</param>
        /// <param name="comparisonType">Tùy chọn so sánh chuỗi (hoa/thường).</param>
        /// <param name="ignoreDiacritics">True để bỏ qua dấu khi tìm kiếm.</param>
        /// <returns>True nếu chuỗi nguồn chứa chuỗi con, ngược lại là false.</returns>
        public static bool ContainsDiacriticsAware(this string source, string value, StringComparison comparisonType = StringComparison.Ordinal, bool ignoreDiacritics = false)
        {
            return source.IndexOfDiacriticsAware(value, comparisonType, ignoreDiacritics) >= 0;
        }

        /// <summary>
        /// Thay thế tất cả các lần xuất hiện của chuỗi con, với tùy chọn bỏ qua dấu và phân biệt hoa thường.
        /// </summary>
        /// <param name="source">Chuỗi nguồn.</param>
        /// <param name="oldValue">Chuỗi cần được thay thế.</param>
        /// <param name="newValue">Chuỗi mới để thay thế.</param>
        /// <param name="comparisonType">Tùy chọn so sánh chuỗi (hoa/thường).</param>
        /// <param name="ignoreDiacritics">True để bỏ qua dấu khi tìm kiếm.</param>
        /// <returns>Chuỗi mới sau khi đã thay thế.</returns>
        public static string ReplaceDiacriticsAware(this string source, string oldValue, string newValue, StringComparison comparisonType = StringComparison.Ordinal, bool ignoreDiacritics = false)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(oldValue))
            {
                return source;
            }

            if (!ignoreDiacritics)
            {
                return source.Replace(oldValue, newValue, comparisonType); // C# 6+
            }

            var result = new StringBuilder();
            int currentIndex = 0;
            int nextIndex;

            while ((nextIndex = source.IndexOfDiacriticsAware(oldValue, currentIndex, comparisonType, ignoreDiacritics)) != -1)
            {
                result.Append(source.Substring(currentIndex, nextIndex - currentIndex));
                result.Append(newValue);
                currentIndex = nextIndex + oldValue.Length;
            }

            result.Append(source.Substring(currentIndex));

            return result.ToString();
        }

        /// <summary>
        /// Phiên bản an toàn của IndexOfDiacriticsAware để tìm kiếm từ một vị trí bắt đầu.
        /// </summary>
        private static int IndexOfDiacriticsAware(this string source, string value, int startIndex, StringComparison comparisonType, bool ignoreDiacritics)
        {
            if (!ignoreDiacritics)
            {
                return source.IndexOf(value, startIndex, comparisonType);
            }

            int index = source.RemoveUnicode().IndexOf(value.RemoveUnicode(), startIndex, comparisonType);
            return index;
        }

        /// <summary>
        /// Tìm vị trí của một từ hoàn chỉnh, với tùy chọn bỏ qua dấu và phân biệt hoa thường.
        /// </summary>
        /// <param name="source">Chuỗi nguồn để tìm kiếm.</param>
        /// <param name="value">Từ cần tìm.</param>
        /// <param name="startIndex">Vị trí bắt đầu tìm kiếm.</param>
        /// <param name="comparisonType">Tùy chọn so sánh chuỗi (hoa/thường).</param>
        /// <param name="ignoreDiacritics">True để bỏ qua dấu khi tìm kiếm.</param>
        /// <returns>Vị trí của từ, hoặc -1 nếu không tìm thấy.</returns>
        public static int IndexOfWholeWord(this string source, string value, int startIndex, StringComparison comparisonType = StringComparison.Ordinal, bool ignoreDiacritics = false)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value) || startIndex < 0 || startIndex >= source.Length)
            {
                return -1;
            }

            int currentIndex = startIndex;
            while (currentIndex < source.Length)
            {
                int matchIndex = source.IndexOfDiacriticsAware(value, currentIndex, comparisonType, ignoreDiacritics);

                if (matchIndex == -1)
                {
                    return -1; // Không tìm thấy
                }

                // Kiểm tra ranh giới từ
                bool isStartBoundary = (matchIndex == 0) || !char.IsLetterOrDigit(source[matchIndex - 1]);
                bool isEndBoundary = (matchIndex + value.Length == source.Length) || !char.IsLetterOrDigit(source[matchIndex + value.Length]);

                if (isStartBoundary && isEndBoundary)
                {
                    return matchIndex; // Tìm thấy từ hoàn chỉnh
                }

                // Nếu không phải từ hoàn chỉnh, tìm kiếm ở vị trí tiếp theo
                currentIndex = matchIndex + 1;
            }

            return -1;
        }

        /// <summary>
        /// Tìm vị trí của một từ hoàn chỉnh từ đầu chuỗi.
        /// </summary>
        public static int IndexOfWholeWord(this string source, string value, StringComparison comparisonType = StringComparison.Ordinal, bool ignoreDiacritics = false)
        {
            return source.IndexOfWholeWord(value, 0, comparisonType, ignoreDiacritics);
        }

        /// <summary>
        /// Kiểm tra chuỗi có chứa một từ hoàn chỉnh hay không.
        /// </summary>
        public static bool ContainsWholeWord(this string source, string value, StringComparison comparisonType = StringComparison.Ordinal, bool ignoreDiacritics = false)
        {
            return source.IndexOfWholeWord(value, 0, comparisonType, ignoreDiacritics) >= 0;
        }

        /// <summary>
        /// Thay thế tất cả các từ hoàn chỉnh khớp với giá trị cho trước.
        /// </summary>
        public static string ReplaceWholeWord(this string source, string oldValue, string newValue, StringComparison comparisonType = StringComparison.Ordinal, bool ignoreDiacritics = false)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(oldValue))
            {
                return source;
            }

            var result = new StringBuilder();
            int currentIndex = 0;

            while (currentIndex < source.Length)
            {
                int nextMatch = source.IndexOfWholeWord(oldValue, currentIndex, comparisonType, ignoreDiacritics);

                if (nextMatch == -1)
                {
                    result.Append(source.Substring(currentIndex));
                    break;
                }

                result.Append(source.Substring(currentIndex, nextMatch - currentIndex));
                result.Append(newValue);
                currentIndex = nextMatch + oldValue.Length;
            }

            return result.ToString();
        }

        /// <summary>
        /// Loại bỏ ký tự đặc biệt khỏi chuỗi.
        /// </summary>
        public static string RemoveSpecialCharacters(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            return Regex.Replace(text, @"[^a-zA-Z0-9\s]", "");
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải toàn chữ hoa.
        /// </summary>
        public static bool IsAllUpper(this string text)
        {
            return !string.IsNullOrEmpty(text) && text.All(char.IsUpper);
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải toàn chữ thường.
        /// </summary>
        public static bool IsAllLower(this string text)
        {
            return !string.IsNullOrEmpty(text) && text.All(char.IsLower);
        }

        /// <summary>
        /// Viết hoa chữ cái đầu tiên của chuỗi.
        /// </summary>
        public static string CapitalizeFirstLetter(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        /// <summary>
        /// Viết hoa chữ cái đầu của từng từ trong chuỗi.
        /// </summary>
        public static string CapitalizeWords(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        /// <summary>
        /// Tách chuỗi thành mảng dòng.
        /// </summary>
        public static string[] SplitLines(this string text)
        {
            return text?.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Chuyển chuỗi sang số nguyên (int), trả về null nếu không hợp lệ.
        /// </summary>
        public static int? ToNullableInt(this string text)
        {
            if (int.TryParse(text, out int result))
                return result;
            return null;
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là email hợp lệ.
        /// </summary>
        public static bool IsValidEmail(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là số điện thoại Việt Nam hợp lệ.
        /// </summary>
        public static bool IsValidPhone(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, @"^(0|\+84)[0-9]{9,10}$");
        }

        /// <summary>
        /// Kiểm tra chuỗi null hoặc rỗng.
        /// </summary>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// Kiểm tra chuỗi null hoặc chỉ chứa khoảng trắng.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        /// <summary>
        /// Kiểm tra chuỗi con, không phân biệt hoa thường.
        /// </summary>
        public static bool ContainsIgnoreCase(this string text, string value)
        {
            return text?.IndexOf(value ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// So sánh chuỗi không phân biệt hoa thường.
        /// </summary>
        public static bool EqualsIgnoreCase(this string text, string value)
        {
            return string.Equals(text, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Cắt chuỗi về độ dài tối đa, thêm ... nếu bị cắt.
        /// </summary>
        public static string Truncate(this string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || maxLength <= 0) return string.Empty;
            return text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;
        }

        /// <summary>
        /// Đảo ngược chuỗi.
        /// </summary>
        public static string Reverse(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return new string(text.Reverse().ToArray());
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt (chuyên cho tiếng Việt).
        /// </summary>
        public static string RemoveDiacritics(this string text)
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
        /// Chuyển chuỗi thành dạng slug (dùng cho URL, SEO).
        /// </summary>
        public static string ToSlug(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var slug = text.RemoveDiacritics().ToLower();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-');
            return slug;
        }

        /// <summary>
        /// Lặp lại chuỗi n lần.
        /// </summary>
        public static string Repeat(this string text, int count)
        {
            if (string.IsNullOrEmpty(text) || count <= 0) return string.Empty;
            return string.Concat(Enumerable.Repeat(text, count));
        }

        /// <summary>
        /// Lấy chuỗi con an toàn, không lỗi nếu vượt quá độ dài.
        /// </summary>
        public static string SafeSubstring(this string text, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(text) || startIndex >= text.Length) return string.Empty;
            if (startIndex + length > text.Length) length = text.Length - startIndex;
            return text.Substring(startIndex, length);
        }

        /// <summary>
        /// Chuyển chuỗi sang camelCase.
        /// </summary>
        public static string ToCamelCase(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var pascal = text.ToPascalCase();
            return char.ToLower(pascal[0]) + pascal.Substring(1);
        }

        /// <summary>
        /// Chuyển chuỗi sang PascalCase.
        /// </summary>
        public static string ToPascalCase(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var words = Regex.Split(text, "[^a-zA-Z0-9]").Where(w => !string.IsNullOrEmpty(w)).ToArray();
            return string.Concat(words.Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
        }

        /// <summary>
        /// Chuyển chuỗi sang snake_case.
        /// </summary>
        public static string ToSnakeCase(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var words = Regex.Split(text, "[^a-zA-Z0-9]").Where(w => !string.IsNullOrEmpty(w)).ToArray();
            return string.Join("_", words).ToLower();
        }

        /// <summary>
        /// Chuyển chuỗi sang kebab-case.
        /// </summary>
        public static string ToKebabCase(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var words = Regex.Split(text, "[^a-zA-Z0-9]").Where(w => !string.IsNullOrEmpty(w)).ToArray();
            return string.Join("-", words).ToLower();
        }

        /// <summary>
        /// Loại bỏ toàn bộ thẻ HTML khỏi chuỗi.
        /// </summary>
        public static string RemoveHtmlTags(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return Regex.Replace(text, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Lấy ra chuỗi chỉ chứa số.
        /// </summary>
        public static string OnlyDigits(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return new string(text.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Lấy ra chuỗi chỉ chứa chữ cái.
        /// </summary>
        public static string OnlyLetters(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return new string(text.Where(char.IsLetter).ToArray());
        }

        /// <summary>
        /// Đếm số lần xuất hiện của một chuỗi con.
        /// </summary>
        public static int CountOccurrences(this string text, string sub)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(sub)) return 0;
            int count = 0, index = 0;
            while ((index = text.IndexOf(sub, index, StringComparison.Ordinal)) != -1)
            {
                count++;
                index += sub.Length;
            }
            return count;
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là JSON hợp lệ không (chuẩn .NET, không dùng thư viện ngoài).
        /// </summary>
        public static bool IsJson(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            text = text.Trim();
            if ((text.StartsWith("{") && text.EndsWith("}")) || (text.StartsWith("[") && text.EndsWith("]")))
            {
                try
                {
                    System.Text.Json.JsonDocument.Parse(text);
                    return true;
                }
                catch { return false; }
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là Base64 hợp lệ không.
        /// </summary>
        public static bool IsBase64(this string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            Span<byte> buffer = new Span<byte>(new byte[text.Length]);
            return Convert.TryFromBase64String(text, buffer, out _);
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là GUID hợp lệ không.
        /// </summary>
        public static bool IsGuid(this string text)
        {
            return Guid.TryParse(text, out _);
        }

        /// <summary>
        /// Ẩn một phần chuỗi (ví dụ: che số điện thoại, email).
        /// </summary>
        public static string Mask(this string text, int start, int length, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(text) || start >= text.Length || length <= 0) return text;
            int maskLength = Math.Min(length, text.Length - start);
            return text.Substring(0, start) + new string(maskChar, maskLength) + text.Substring(start + maskLength);
        }

        /// <summary>
        /// Chuyển chuỗi sang Enum (generic, an toàn).
        /// </summary>
        public static T ToEnum<T>(this string text, T defaultValue = default) where T : struct
        {
            if (Enum.TryParse<T>(text, true, out var result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// Loại bỏ các khoảng trắng lặp lại, chỉ giữ một khoảng trắng giữa các từ.
        /// </summary>
        public static string RemoveDuplicateSpaces(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return Regex.Replace(text, @"\s+", " ").Trim();
        }

        /// <summary>
        /// Tách chuỗi thành mảng các từ (bỏ ký tự đặc biệt, giữ lại chữ và số).
        /// </summary>
        public static string[] ToWords(this string text)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();
            return Regex.Matches(text, "[a-zA-Z0-9]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();
        }

        /// <summary>
        /// Băm chuỗi sang MD5.
        /// </summary>
        public static string ToMd5(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Băm chuỗi sang SHA256.
        /// </summary>
        public static string ToSha256(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Kiểm tra chuỗi có khớp với biểu thức chính quy (Regex) không.
        /// </summary>
        public static bool IsMatch(this string text, string pattern, RegexOptions options = RegexOptions.None)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern)) return false;
            return Regex.IsMatch(text, pattern, options);
        }

        /// <summary>
        /// Loại bỏ toàn bộ ký tự không phải ASCII.
        /// </summary>
        public static string RemoveNonAscii(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return new string(text.Where(c => c <= 127).ToArray());
        }

        /// <summary>
        /// Viết hoa chữ cái đầu của mỗi từ (chuẩn hóa kiểu tiêu đề).
        /// </summary>
        public static string ToTitleCase(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        /// <summary>
        /// Chuyển chuỗi sang mảng byte với encoding tùy chọn.
        /// </summary>
        public static byte[] ToByteArray(this string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<byte>();
            return (encoding ?? Encoding.UTF8).GetBytes(text);
        }

        /// <summary>
        /// Chuyển chuỗi Base64 sang chuỗi gốc.
        /// </summary>
        public static string FromBase64(this string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var bytes = Convert.FromBase64String(text);
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }

        /// <summary>
        /// Chuyển chuỗi sang Base64.
        /// </summary>
        public static string ToBase64(this string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var bytes = (encoding ?? Encoding.UTF8).GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Đảm bảo chuỗi bắt đầu bằng tiền tố cho trước.
        /// </summary>
        public static string EnsureStartsWith(this string text, string prefix)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(prefix)) return text;
            return text.StartsWith(prefix) ? text : prefix + text;
        }

        /// <summary>
        /// Đảm bảo chuỗi kết thúc bằng hậu tố cho trước.
        /// </summary>
        public static string EnsureEndsWith(this string text, string suffix)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(suffix)) return text;
            return text.EndsWith(suffix) ? text : text + suffix;
        }

        /// <summary>
        /// Loại bỏ một chuỗi con ở đầu nếu có.
        /// </summary>
        public static string RemoveStart(this string text, string value)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(value)) return text;
            return text.StartsWith(value) ? text.Substring(value.Length) : text;
        }

        /// <summary>
        /// Loại bỏ một chuỗi con ở cuối nếu có.
        /// </summary>
        public static string RemoveEnd(this string text, string value)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(value)) return text;
            return text.EndsWith(value) ? text.Substring(0, text.Length - value.Length) : text;
        }

        /// <summary>
        /// Thay thế lần xuất hiện đầu tiên của một chuỗi con.
        /// </summary>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(search)) return text;
            int pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0) return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /// <summary>
        /// Thay thế lần xuất hiện cuối cùng của một chuỗi con.
        /// </summary>
        public static string ReplaceLast(this string text, string search, string replace)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(search)) return text;
            int pos = text.LastIndexOf(search, StringComparison.Ordinal);
            if (pos < 0) return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /// <summary>
        /// Đếm số từ trong chuỗi.
        /// </summary>
        public static int WordCount(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return Regex.Matches(text, "[a-zA-Z0-9]+", RegexOptions.Multiline).Count;
        }

        /// <summary>
        /// Chuyển chuỗi thành MemoryStream.
        /// </summary>
        public static MemoryStream ToStream(this string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text)) return new MemoryStream();
            var bytes = (encoding ?? Encoding.UTF8).GetBytes(text);
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Chuyển chuỗi sang int, hỗ trợ nhiều format số, trả về giá trị mặc định nếu không hợp lệ.
        /// </summary>
        public static int ToInt(this string text, int defaultValue = 0, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
        {
            provider ??= new CultureInfo("vi-VN");
            if (int.TryParse(text, style, provider, out int result))
                return result;
            // Thử lại với format US nếu thất bại
            if (int.TryParse(text, style, CultureInfo.InvariantCulture, out result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// Chuyển chuỗi sang double, hỗ trợ nhiều format số, trả về giá trị mặc định nếu không hợp lệ.
        /// </summary>
        public static double ToDouble(this string text, double defaultValue = 0, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
        {
            provider ??= new CultureInfo("vi-VN");
            if (double.TryParse(text, style, provider, out double result))
                return result;
            // Thử lại với format US nếu thất bại
            if (double.TryParse(text, style, CultureInfo.InvariantCulture, out result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// Chuyển chuỗi sang DateTime, hỗ trợ nhiều format ngày tháng, trả về giá trị mặc định nếu không hợp lệ.
        /// </summary>
        public static DateTime ToDateTime(this string text, DateTime defaultValue = default, string[] formats = null, IFormatProvider provider = null, DateTimeStyles styles = DateTimeStyles.None)
        {
            provider ??= new CultureInfo("vi-VN");
            formats ??= new[]
            {
                "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy", "dd.MM.yyyy", "d.M.yyyy",
                "yyyy-MM-dd", "yyyy/MM/dd", "yyyy.MM.dd",
                "dd/MM/yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "dd.MM.yyyy HH:mm:ss",
                "yyyy-MM-dd HH:mm:ss", "yyyy/MM/dd HH:mm:ss", "yyyy.MM.dd HH:mm:ss",
                "dd/MM/yyyy HH:mm", "dd-MM-yyyy HH:mm", "dd.MM.yyyy HH:mm",
                "yyyy-MM-dd HH:mm", "yyyy/MM/dd HH:mm", "yyyy.MM.dd HH:mm"
            };
            if (DateTime.TryParseExact(text, formats, provider, styles, out DateTime result))
                return result;
            // Thử lại với format US nếu thất bại
            if (DateTime.TryParse(text, CultureInfo.InvariantCulture, styles, out result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là palindrome (đối xứng) không.
        /// </summary>
        public static bool IsPalindrome(this string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            var clean = Regex.Replace(text.ToLower(), "[^a-z0-9]", "");
            return clean.SequenceEqual(clean.Reverse());
        }

        /// <summary>
        /// Chuyển chuỗi sang bool, hỗ trợ nhiều giá trị tiếng Việt.
        /// </summary>
        public static bool ToBool(this string text, bool defaultValue = false)
        {
            if (string.IsNullOrWhiteSpace(text)) return defaultValue;
            var trueValues = new[] { "true", "1", "yes", "có", "y", "ok" };
            var falseValues = new[] { "false", "0", "no", "không", "n" };
            var val = text.Trim().ToLower();
            if (trueValues.Contains(val)) return true;
            if (falseValues.Contains(val)) return false;
            if (bool.TryParse(val, out bool result)) return result;
            return defaultValue;
        }

        /// <summary>
        /// Chuyển chuỗi sang decimal, hỗ trợ nhiều format số.
        /// </summary>
        public static decimal ToDecimal(this string text, decimal defaultValue = 0, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
        {
            provider ??= new CultureInfo("vi-VN");
            if (decimal.TryParse(text, style, provider, out decimal result))
                return result;
            if (decimal.TryParse(text, style, CultureInfo.InvariantCulture, out result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// Bổ sung ký tự về bên trái cho đủ độ dài, không lỗi nếu chuỗi null.
        /// </summary>
        public static string PadLeftSafe(this string text, int totalWidth, char paddingChar = ' ')
        {
            return (text ?? string.Empty).PadLeft(totalWidth, paddingChar);
        }

        /// <summary>
        /// Bổ sung ký tự về bên phải cho đủ độ dài, không lỗi nếu chuỗi null.
        /// </summary>
        public static string PadRightSafe(this string text, int totalWidth, char paddingChar = ' ')
        {
            return (text ?? string.Empty).PadRight(totalWidth, paddingChar);
        }

        /// <summary>
        /// Loại bỏ tất cả các ký tự thuộc một tập ký tự cho trước.
        /// </summary>
        public static string RemoveAll(this string text, params char[] chars)
        {
            if (string.IsNullOrEmpty(text) || chars == null || chars.Length == 0) return text;
            var set = new HashSet<char>(chars);
            return new string(text.Where(c => !set.Contains(c)).ToArray());
        }

        /// <summary>
        /// Thay thế nhiều chuỗi con cùng lúc.
        /// </summary>
        public static string ReplaceMany(this string text, Dictionary<string, string> replacements)
        {
            if (string.IsNullOrEmpty(text) || replacements == null || replacements.Count == 0) return text;
            var result = text;
            foreach (var kv in replacements)
            {
                result = result.Replace(kv.Key, kv.Value);
            }
            return result;
        }

        /// <summary>
        /// Tính khoảng cách Levenshtein giữa hai chuỗi.
        /// </summary>
        public static int LevenshteinDistance(this string source, string target)
        {
            if (string.IsNullOrEmpty(source)) return target?.Length ?? 0;
            if (string.IsNullOrEmpty(target)) return source.Length;
            var d = new int[source.Length + 1, target.Length + 1];
            for (int i = 0; i <= source.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= target.Length; j++) d[0, j] = j;
            for (int i = 1; i <= source.Length; i++)
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = source[i - 1] == target[j - 1] ? 0 : 1;
                    d[i, j] = new[]
                    {
                        d[i - 1, j] + 1,
                        d[i, j - 1] + 1,
                        d[i - 1, j - 1] + cost
                    }.Min();
                }
            return d[source.Length, target.Length];
        }

        /// <summary>
        /// Tính phần trăm tương đồng giữa hai chuỗi (0-1).
        /// </summary>
        public static double Similarity(this string source, string target)
        {
            if (string.IsNullOrEmpty(source) && string.IsNullOrEmpty(target)) return 1.0;
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) return 0.0;
            int distance = source.LevenshteinDistance(target);
            return 1.0 - (double)distance / Math.Max(source.Length, target.Length);
        }

        /// <summary>
        /// Định dạng số thành chuỗi tiền tệ Việt Nam.
        /// </summary>
        public static string ToCurrencyString(this string text, string culture = "vi-VN")
        {
            if (decimal.TryParse(text, NumberStyles.Any, new CultureInfo(culture), out decimal value))
                return value.ToString("C0", new CultureInfo(culture));
            return text;
        }

        /// <summary>
        /// Chuyển số thành chữ tiếng Việt (ví dụ: 1234 → "một nghìn hai trăm ba mươi bốn").
        /// </summary>
        public static string ToWordsVN(this string text)
        {
            if (!long.TryParse(text, out long number)) return text;
            if (number == 0) return "không";
            string[] dv = { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };
            string[] cs = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string s = "";
            int i = 0;
            while (number > 0)
            {
                int n = (int)(number % 1000);
                if (n != 0)
                {
                    string str = "";
                    int tram = n / 100;
                    int chuc = (n % 100) / 10;
                    int donvi = n % 10;
                    if (tram > 0) str += cs[tram] + " trăm";
                    if (chuc > 1) str += (str == "" ? "" : " ") + cs[chuc] + " mươi";
                    else if (chuc == 1) str += (str == "" ? "" : " ") + "mười";
                    if (donvi > 0)
                    {
                        if (chuc == 0 && tram != 0) str += " lẻ";
                        if (chuc > 1 && donvi == 1) str += " mốt";
                        else if (chuc > 0 && donvi == 5) str += " lăm";
                        else str += " " + cs[donvi];
                    }
                    s = str.Trim() + dv[i] + (s == "" ? "" : " ") + s;
                }
                number /= 1000;
                i++;
            }
            return s.Trim();
        }

        /// <summary>
        /// Băm chuỗi sang SHA1.
        /// </summary>
        public static string ToSha1(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            using var sha = SHA1.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Băm chuỗi sang SHA512.
        /// </summary>
        public static string ToSha512(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            using var sha = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Mã hóa base64 dạng URL-safe.
        /// </summary>
        public static string ToBase64Url(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        /// <summary>
        /// Chuyển Dictionary<string, string> sang query string (dùng cho API).
        /// </summary>
        public static string ToQueryString(this Dictionary<string, string> dict)
        {
            if (dict == null || dict.Count == 0) return string.Empty;
            var array = dict.Select(kv => $"{System.Web.HttpUtility.UrlEncode(kv.Key)}={System.Web.HttpUtility.UrlEncode(kv.Value)}");
            return string.Join("&", array);
        }

        /// <summary>
        /// Chuyển object sang JSON string (dùng cho log, API).
        /// </summary>
        public static string ToJsonString(this object obj, bool indented = false)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                WriteIndented = indented
            });
        }

        /// <summary>
        /// Trim đầu/cuối chuỗi, không lỗi nếu chuỗi null.
        /// </summary>
        public static string TrimSafe(this string text)
        {
            return text?.Trim();
        }

        /// <summary>
        /// Loại bỏ tất cả khoảng trắng trong chuỗi.
        /// </summary>
        public static string TrimAll(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return Regex.Replace(text, @"\s+", "");
        }

        /// <summary>
        /// Trim và trả về null nếu chuỗi rỗng sau khi trim.
        /// </summary>
        public static string TrimToNull(this string text)
        {
            var t = text?.Trim();
            return string.IsNullOrEmpty(t) ? null : t;
        }

        /// <summary>
        /// Kiểm tra tên đăng nhập hợp lệ: chỉ chứa chữ, số, dấu gạch dưới (_), dấu chấm (.), độ dài 3-32, không bắt đầu/kết thúc bằng dấu chấm/gạch dưới, không có hai dấu chấm/gạch dưới liên tiếp.
        /// </summary>
        public static bool IsValidUsername(this string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (text.Length < 3 || text.Length > 32) return false;
            if (!Regex.IsMatch(text, @"^[a-zA-Z0-9._]+$")) return false;
            if (text.StartsWith(".") || text.StartsWith("_") || text.EndsWith(".") || text.EndsWith("_")) return false;
            if (text.Contains("..") || text.Contains("__") || text.Contains("._") || text.Contains("_")) return false;
            return true;
        }

        /// <summary>
        /// Kiểm tra chuỗi bắt đầu bằng ký tự thường. Có thể bỏ qua khoảng trắng đầu nếu ignoreWhitespace=true.
        /// </summary>
        public static bool StartsWithLower(this string text, bool ignoreWhitespace = false)
        {
            if (string.IsNullOrEmpty(text)) return false;
            int idx = 0;
            if (ignoreWhitespace)
            {
                while (idx < text.Length && char.IsWhiteSpace(text[idx])) idx++;
                if (idx >= text.Length) return false;
            }
            return char.IsLower(text[idx]);
        }

        /// <summary>
        /// Kiểm tra chuỗi kết thúc bằng ký tự thường. Có thể bỏ qua khoảng trắng cuối nếu ignoreWhitespace=true.
        /// </summary>
        public static bool EndsWithLower(this string text, bool ignoreWhitespace = false)
        {
            if (string.IsNullOrEmpty(text)) return false;
            int idx = text.Length - 1;
            if (ignoreWhitespace)
            {
                while (idx >= 0 && char.IsWhiteSpace(text[idx])) idx--;
                if (idx < 0) return false;
            }
            return char.IsLower(text[idx]);
        }

        /// <summary>
        /// Kiểm tra chuỗi bắt đầu bằng ký tự hoa. Có thể bỏ qua khoảng trắng đầu nếu ignoreWhitespace=true.
        /// </summary>
        public static bool StartsWithUpper(this string text, bool ignoreWhitespace = false)
        {
            if (string.IsNullOrEmpty(text)) return false;
            int idx = 0;
            if (ignoreWhitespace)
            {
                while (idx < text.Length && char.IsWhiteSpace(text[idx])) idx++;
                if (idx >= text.Length) return false;
            }
            return char.IsUpper(text[idx]);
        }

        /// <summary>
        /// Kiểm tra chuỗi kết thúc bằng ký tự hoa. Có thể bỏ qua khoảng trắng cuối nếu ignoreWhitespace=true.
        /// </summary>
        public static bool EndsWithUpper(this string text, bool ignoreWhitespace = false)
        {
            if (string.IsNullOrEmpty(text)) return false;
            int idx = text.Length - 1;
            if (ignoreWhitespace)
            {
                while (idx >= 0 && char.IsWhiteSpace(text[idx])) idx--;
                if (idx < 0) return false;
            }
            return char.IsUpper(text[idx]);
        }

        /// <summary>
        /// Kiểm tra chuỗi bắt đầu bằng số. Có thể bỏ qua khoảng trắng đầu nếu ignoreWhitespace=true.
        /// </summary>
        public static bool StartsWithDigit(this string text, bool ignoreWhitespace = false)
        {
            if (string.IsNullOrEmpty(text)) return false;
            int idx = 0;
            if (ignoreWhitespace)
            {
                while (idx < text.Length && char.IsWhiteSpace(text[idx])) idx++;
                if (idx >= text.Length) return false;
            }
            return char.IsDigit(text[idx]);
        }

        /// <summary>
        /// Kiểm tra chuỗi kết thúc bằng số. Có thể bỏ qua khoảng trắng cuối nếu ignoreWhitespace=true.
        /// </summary>
        public static bool EndsWithDigit(this string text, bool ignoreWhitespace = false)
        {
            if (string.IsNullOrEmpty(text)) return false;
            int idx = text.Length - 1;
            if (ignoreWhitespace)
            {
                while (idx >= 0 && char.IsWhiteSpace(text[idx])) idx--;
                if (idx < 0) return false;
            }
            return char.IsDigit(text[idx]);
        }

        /// <summary>
        /// Kiểm tra chuỗi bắt đầu bằng ký tự đặc biệt (không phải chữ/số). Có thể bỏ qua khoảng trắng đầu nếu ignoreWhitespace=true.
        /// </summary>
        public static bool StartsWithSymbol(this string text, bool ignoreWhitespace = false)
        {
            if (string.IsNullOrEmpty(text)) return false;
            int idx = 0;
            if (ignoreWhitespace)
            {
                while (idx < text.Length && char.IsWhiteSpace(text[idx])) idx++;
                if (idx >= text.Length) return false;
            }
            return !char.IsLetterOrDigit(text[idx]);
        }

        /// <summary>
        /// Kiểm tra chuỗi kết thúc bằng ký tự đặc biệt (không phải chữ/số). Có thể bỏ qua khoảng trắng cuối nếu ignoreWhitespace=true.
        /// </summary>
        public static bool EndsWithSymbol(this string text, bool ignoreWhitespace = false)
        {
            if (string.IsNullOrEmpty(text)) return false;
            int idx = text.Length - 1;
            if (ignoreWhitespace)
            {
                while (idx >= 0 && char.IsWhiteSpace(text[idx])) idx--;
                if (idx < 0) return false;
            }
            return !char.IsLetterOrDigit(text[idx]);
        }

        /// <summary>
        /// Kiểm tra chuỗi có nhiều dấu cách liên tiếp.
        /// </summary>
        public static bool HasMultipleSpaces(this string text)
        {
            return !string.IsNullOrEmpty(text) && Regex.IsMatch(text, " {2,}");
        }

        /// <summary>
        /// Kiểm tra chuỗi có ký tự ngắt câu (.,!?:;…).
        /// </summary>
        public static bool HasPunctuation(this string text)
        {
            return !string.IsNullOrEmpty(text) && text.Any(c => char.IsPunctuation(c));
        }

        /// <summary>
        /// Kiểm tra chuỗi có dấu phẩy.
        /// </summary>
        public static bool HasComma(this string text)
        {
            return !string.IsNullOrEmpty(text) && text.Contains(",");
        }

        /// <summary>
        /// Kiểm tra chuỗi có ký tự ngoài bảng mã ASCII.
        /// </summary>
        public static bool HasNonAscii(this string text)
        {
            return !string.IsNullOrEmpty(text) && text.Any(c => c > 127);
        }

        /// <summary>
        /// Kiểm tra chuỗi chỉ toàn khoảng trắng hoặc dấu câu.
        /// </summary>
        public static bool IsAllWhitespaceOrPunctuation(this string text)
        {
            return !string.IsNullOrEmpty(text) && text.All(c => char.IsWhiteSpace(c) || char.IsPunctuation(c));
        }

        /// <summary>
        /// Kiểm tra chuỗi chỉ toàn ký tự đặc biệt (không có chữ/số).
        /// </summary>
        public static bool IsAllSymbols(this string text)
        {
            return !string.IsNullOrEmpty(text) && text.All(c => !char.IsLetterOrDigit(c));
        }

        /// <summary>
        /// Kiểm tra chuỗi chỉ toàn số hoặc chữ.
        /// </summary>
        public static bool IsAllDigitsOrLetters(this string text)
        {
            return !string.IsNullOrEmpty(text) && text.All(c => char.IsLetterOrDigit(c));
        }

        /// <summary>
        /// Kiểm tra chuỗi chỉ là một từ (không có khoảng trắng).
        /// </summary>
        public static bool IsSingleWord(this string text)
        {
            return !string.IsNullOrEmpty(text) && !text.Any(char.IsWhiteSpace);
        }

        /// <summary>
        /// Kiểm tra chuỗi có dạng giống email (dùng cho gợi ý).
        /// </summary>
        public static bool IsEmailLike(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        /// <summary>
        /// Viết hoa chữ cái đầu mỗi câu.
        /// </summary>
        public static string ToSentenceCase(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sentences = Regex.Split(text, @"([.!?]\s*)");
            var sb = new StringBuilder();
            foreach (var s in sentences)
            {
                if (string.IsNullOrWhiteSpace(s)) continue;
                var trimmed = s.TrimStart();
                if (trimmed.Length > 0)
                    sb.Append(char.ToUpper(trimmed[0]) + trimmed.Substring(1));
                else
                    sb.Append(trimmed);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Tách chuỗi thành các câu (dựa vào dấu chấm, chấm hỏi, chấm than).
        /// </summary>
        public static string[] SplitSentences(this string text)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();
            return Regex.Split(text, @"(?<=[.!?])\s+").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }

        /// <summary>
        /// Đếm số từ (loại bỏ số, ký tự đặc biệt).
        /// </summary>
        public static int CountWords(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return Regex.Matches(text, "[a-zA-ZÀ-ỹ0-9]+", RegexOptions.Multiline).Count;
        }

        /// <summary>
        /// Lấy chữ cái đầu của từng từ (dùng cho tạo username, avatar).
        /// </summary>
        public static string GetInitials(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            return string.Concat(text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(w => char.ToUpper(w[0])));
        }

        /// <summary>
        /// Kiểm tra chuỗi có emoji không.
        /// </summary>
        public static bool ContainsEmoji(this string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            return Regex.IsMatch(text, @"[\uD83C-\uDBFF\uDC00-\uDFFF]+| -]+", RegexOptions.Compiled);
        }

        /// <summary>
        /// Loại bỏ emoji khỏi chuỗi.
        /// </summary>
        public static string RemoveEmoji(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return Regex.Replace(text, @"[\uD83C-\uDBFF\uDC00-\uDFFF]+", "");
        }

        /// <summary>
        /// Kiểm tra chuỗi có ký tự điều khiển (control char) không.
        /// </summary>
        public static bool ContainsControlChars(this string text)
        {
            return !string.IsNullOrEmpty(text) && text.Any(char.IsControl);
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là biển số xe Việt Nam hợp lệ.
        /// </summary>
        public static bool IsValidLicensePlateVN(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, @"^\d{2}[A-Z]-\d{3,4}\.[0-9]{2}$");
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là số tài khoản ngân hàng Việt Nam hợp lệ (8-16 số).
        /// </summary>
        public static bool IsValidBankAccountVN(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, @"^\d{8,16}$");
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là số điện thoại Việt Nam hợp lệ (theo đầu số mới nhất).
        /// </summary>
        public static bool IsValidPhoneVN(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, @"^(0|\+84)[0-9]{9,10}$");
        }

        /// <summary>
        /// Kiểm tra chuỗi con theo kiểu fuzzy (cho phép sai lệch ký tự).
        /// </summary>
        public static bool FuzzyContains(this string text, string pattern, int maxDistance = 2)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern)) return false;
            for (int i = 0; i <= text.Length - pattern.Length; i++)
            {
                var sub = text.Substring(i, pattern.Length);
                if (sub.LevenshteinDistance(pattern) <= maxDistance)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Tô đậm từ khóa trong chuỗi (dùng cho UI, HTML).
        /// </summary>
        public static string HighlightKeyword(this string text, string keyword, string tag = "<b>", string endTag = "</b>")
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword)) return text;
            return Regex.Replace(text, Regex.Escape(keyword), m => tag + m.Value + endTag, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Lấy tất cả số trong chuỗi (dùng cho phân tích dữ liệu).
        /// </summary>
        public static List<string> ExtractNumbers(this string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            return Regex.Matches(text, @"\d+").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Lấy tất cả ngày tháng trong chuỗi (dùng cho báo cáo, lịch sử).
        /// </summary>
        public static List<string> ExtractDates(this string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy", "dd.MM.yyyy", "d.M.yyyy", "yyyy-MM-dd" };
            var matches = Regex.Matches(text, @"\b\d{1,2}[/-]\d{1,2}[/-]\d{2,4}\b|\b\d{4}-\d{2}-\d{2}\b");
            return matches.Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Loại bỏ hoặc chuyển đổi các thực thể HTML (&amp;, &lt;, &gt;, ...).
        /// </summary>
        public static string RemoveHtmlEntities(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return WebUtility.HtmlDecode(text);
        }

        /// <summary>
        /// Chuẩn hóa chuỗi Unicode về dạng NFC (hoặc NFD nếu truyền option).
        /// </summary>
        public static string NormalizeUnicode(this string text, NormalizationForm form = NormalizationForm.FormC)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Normalize(form);
        }

        /// <summary>
        /// Loại bỏ dấu các ngôn ngữ khác, giữ lại dấu tiếng Việt.
        /// </summary>
        public static string RemoveDiacriticsExceptVietnamese(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            // Loại bỏ dấu các ngôn ngữ khác, giữ lại tiếng Việt (giả lập bằng cách loại bỏ các ký tự Unicode không phải tiếng Việt)
            return Regex.Replace(text, @"[^a-zA-Z0-9\sáàạảãâấầậẩẫăắằặẳẵéèẹẻẽêếềệểễóòọỏõôốồộổỗơớờợởỡúùụủũưứừựửữíìịỉĩđýỳỵỷỹÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴÉÈẸẺẼÊẾỀỆỂỄÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠÚÙỤỦŨƯỨỪỰỬỮÍÌỊỈĨĐÝỲỴỶỸ]", "");
        }

        /// <summary>
        /// Định dạng chuỗi số thành số điện thoại Việt Nam.
        /// </summary>
        public static string FormatAsPhoneNumber(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var digits = new string(text.Where(char.IsDigit).ToArray());
            if (digits.Length == 10)
                return $"{digits.Substring(0, 4)} {digits.Substring(4, 3)} {digits.Substring(7, 3)}";
            if (digits.Length == 11)
                return $"{digits.Substring(0, 4)} {digits.Substring(4, 4)} {digits.Substring(8, 3)}";
            return text;
        }

        /// <summary>
        /// Định dạng chuỗi số thành số thẻ (4-4-4-4).
        /// </summary>
        public static string FormatAsCardNumber(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var digits = new string(text.Where(char.IsDigit).ToArray());
            return string.Join(" ", Enumerable.Range(0, digits.Length / 4 + (digits.Length % 4 == 0 ? 0 : 1)).Select(i => digits.Substring(i * 4, Math.Min(4, digits.Length - i * 4))));
        }

        /// <summary>
        /// Ẩn một phần thông tin nhạy cảm (số thẻ, email, ...).
        /// </summary>
        public static string MaskSensitiveInfo(this string text, int showStart = 2, int showEnd = 2, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(text) || text.Length <= showStart + showEnd) return text;
            return text.Substring(0, showStart) + new string(maskChar, text.Length - showStart - showEnd) + text.Substring(text.Length - showEnd);
        }

        /// <summary>
        /// Mã hóa chuỗi sang base32 (OTP, bảo mật).
        /// </summary>
        public static string ToBase32(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var bytes = Encoding.UTF8.GetBytes(text);
            var sb = new StringBuilder();
            int bits = 0, value = 0;
            foreach (var b in bytes)
            {
                value = (value << 8) | b;
                bits += 8;
                while (bits >= 5)
                {
                    sb.Append(alphabet[(value >> (bits - 5)) & 31]);
                    bits -= 5;
                }
            }
            if (bits > 0)
                sb.Append(alphabet[(value << (5 - bits)) & 31]);
            return sb.ToString();
        }

        /// <summary>
        /// Chuyển chuỗi sang dạng hex.
        /// </summary>
        public static string ToHexString(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var bytes = Encoding.UTF8.GetBytes(text);
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Chuyển hex về chuỗi gốc.
        /// </summary>
        public static string FromHexString(this string hex)
        {
            if (string.IsNullOrEmpty(hex)) return hex;
            var bytes = Enumerable.Range(0, hex.Length / 2).Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16)).ToArray();
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Loại bỏ query string khỏi URL.
        /// </summary>
        public static string RemoveQueryString(this string url)
        {
            if (string.IsNullOrEmpty(url)) return url;
            int idx = url.IndexOf('?');
            return idx >= 0 ? url.Substring(0, idx) : url;
        }

        /// <summary>
        /// Lấy giá trị tham số từ query string.
        /// </summary>
        public static string GetQueryStringValue(this string url, string key)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key)) return string.Empty;
            var uri = new UriBuilder(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query.Get(key) ?? string.Empty;
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là URL hợp lệ.
        /// </summary>
        public static bool IsValidUrl(this string text)
        {
            return Uri.TryCreate(text, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Lấy tất cả email trong chuỗi.
        /// </summary>
        public static List<string> ExtractEmails(this string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            return Regex.Matches(text, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Lấy tất cả URL trong chuỗi.
        /// </summary>
        public static List<string> ExtractUrls(this string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            return Regex.Matches(text, @"https?://[\w\-\.\?\,\'/\\\+&%\$#_=]+", RegexOptions.IgnoreCase).Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Lấy các từ có độ dài nhất định.
        /// </summary>
        public static List<string> ExtractWordsByLength(this string text, int length)
        {
            if (string.IsNullOrEmpty(text) || length <= 0) return new List<string>();
            return Regex.Matches(text, $@"\b\w{{{length},}}\b").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Loại bỏ stop words khỏi chuỗi (tiếng Việt).
        /// </summary>
        public static string RemoveStopWords(this string text, IEnumerable<string> stopWords)
        {
            if (string.IsNullOrEmpty(text) || stopWords == null) return text;
            var words = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words.Where(w => !stopWords.Contains(w, StringComparer.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Lấy n-grams từ chuỗi (dùng cho phân tích ngôn ngữ, AI).
        /// </summary>
        public static List<string> GetNGrams(this string text, int n)
        {
            if (string.IsNullOrEmpty(text) || n <= 0) return new List<string>();
            var words = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var ngrams = new List<string>();
            for (int i = 0; i <= words.Length - n; i++)
                ngrams.Add(string.Join(" ", words.Skip(i).Take(n)));
            return ngrams;
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là tên file hợp lệ.
        /// </summary>
        public static bool IsValidFileName(this string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            char[] invalidChars = Path.GetInvalidFileNameChars();
            return !text.Any(c => invalidChars.Contains(c));
        }

        /// <summary>
        /// Chuẩn hóa tên file, loại bỏ ký tự không hợp lệ.
        /// </summary>
        public static string SanitizeFileName(this string text, char replaceChar = '_')
        {
            if (string.IsNullOrEmpty(text)) return text;
            char[] invalidChars = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(text.Length);
            foreach (var c in text)
                sb.Append(invalidChars.Contains(c) ? replaceChar : c);
            return sb.ToString();
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là số căn cước công dân (CCCD) Việt Nam hợp lệ (12 số, bắt đầu bằng mã tỉnh/thành).
        /// </summary>
        public static bool IsValidCccdVN(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            // CCCD: 12 số, 3 số đầu là mã tỉnh (001-096), 1 số thế hệ, 8 số ngẫu nhiên
            if (!Regex.IsMatch(text, @"^\d{12}$")) return false;
            var provinceCode = int.Parse(text.Substring(0, 3));
            return provinceCode >= 1 && provinceCode <= 96;
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là số giấy phép lái xe Việt Nam hợp lệ (12 số hoặc 10 số, theo quy định mới nhất).
        /// </summary>
        public static bool IsValidDriverLicenseVN(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            // GPLX mới: 12 số, GPLX cũ: 10 số
            return Regex.IsMatch(text, @"^(\d{12}|\d{10})$");
        }

        /// <summary>
        /// Sinh mã QR từ chuỗi, trả về base64 PNG (cần thư viện QRCoder, chỉ là ví dụ placeholder).
        /// </summary>
        public static string ToQrCode(this string text, int pixelsPerModule = 20)
        {
            // Để sử dụng thực tế, cần cài QRCoder (nuget) và bỏ comment các dòng dưới:
            /*
            if (string.IsNullOrEmpty(text)) return string.Empty;
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrData);
            using var bitmap = qrCode.GetGraphic(pixelsPerModule);
            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return Convert.ToBase64String(ms.ToArray());
            */
            // Placeholder nếu chưa cài QRCoder:
            return "[QR_CODE_BASE64_PLACEHOLDER]";
        }

        /// <summary>
        /// Kiểm tra chuỗi đối xứng, không phân biệt hoa thường.
        /// </summary>
        public static bool IsPalindromeIgnoreCase(this string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            var clean = Regex.Replace(text.ToLower(), "[^a-z0-9]", "");
            return clean.SequenceEqual(clean.Reverse());
        }

        /// <summary>
        /// Kiểm tra hai chuỗi là hoán vị của nhau.
        /// </summary>
        public static bool IsAnagram(this string text, string other)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(other)) return false;
            var t1 = new string(text.ToLower().OrderBy(c => c).ToArray());
            var t2 = new string(other.ToLower().OrderBy(c => c).ToArray());
            return t1 == t2;
        }

        /// <summary>
        /// Loại bỏ các từ trùng lặp trong chuỗi.
        /// </summary>
        public static string RemoveDuplicateWords(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            var words = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words.Distinct(StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Xáo trộn thứ tự các từ trong chuỗi.
        /// </summary>
        public static string ShuffleWords(this string text)
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
        /// Lấy tất cả giá trị phần trăm trong chuỗi.
        /// </summary>
        public static List<string> ExtractPercentages(this string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            return Regex.Matches(text, @"\d+(\.\d+)?%?").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Lấy tất cả số điện thoại Việt Nam trong chuỗi.
        /// </summary>
        public static List<string> ExtractPhoneNumbers(this string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            return Regex.Matches(text, @"(0|\+84)(3[2-9]|5[6|8|9]|7[0|6-9]|8[1-5]|9[0-9])[0-9]{7}").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Định dạng số thành chuỗi tiền tệ với nhiều loại tiền (VNĐ, USD, EUR...).
        /// </summary>
        public static string FormatAsCurrency(this string text, string culture = "vi-VN", string currencySymbol = null)
        {
            if (decimal.TryParse(text, NumberStyles.Any, new CultureInfo(culture), out decimal value))
            {
                var ci = new CultureInfo(culture);
                if (!string.IsNullOrEmpty(currencySymbol))
                    ci.NumberFormat.CurrencySymbol = currencySymbol;
                return value.ToString("C0", ci);
            }
            return text;
        }

        /// <summary>
        /// Mã hóa chuỗi sang base64 sau khi nén Gzip.
        /// </summary>
        public static string ToBase64Gzip(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var bytes = Encoding.UTF8.GetBytes(text);
            using var ms = new MemoryStream();
            using (var gzip = new GZipStream(ms, CompressionMode.Compress))
            {
                gzip.Write(bytes, 0, bytes.Length);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Giải mã base64 và giải nén Gzip về chuỗi gốc.
        /// </summary>
        public static string FromBase64Gzip(this string base64)
        {
            if (string.IsNullOrEmpty(base64)) return base64;
            var bytes = Convert.FromBase64String(base64);
            using var ms = new MemoryStream(bytes);
            using var gzip = new GZipStream(ms, CompressionMode.Decompress);
            using var outMs = new MemoryStream();
            gzip.CopyTo(outMs);
            return Encoding.UTF8.GetString(outMs.ToArray());
        }

        /// <summary>
        /// Lấy tất cả thẻ anchor trong chuỗi HTML.
        /// </summary>
        public static List<string> GetAnchorTags(this string html)
        {
            if (string.IsNullOrEmpty(html)) return new List<string>();
            return Regex.Matches(html, "<a [^>]*href=[\"']?([^'\" >]+)[\"']?[^>]*>(.*?)</a>", RegexOptions.IgnoreCase)
                .Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Lấy tất cả URL ảnh trong chuỗi HTML.
        /// </summary>
        public static List<string> ExtractImageUrls(this string html)
        {
            if (string.IsNullOrEmpty(html)) return new List<string>();
            return Regex.Matches(html, "<img [^>]*src=[\"']?([^'\" >]+)[\"']?[^>]*>", RegexOptions.IgnoreCase)
                .Cast<Match>().Select(m =>
                {
                    var srcMatch = Regex.Match(m.Value, "src=[\"']?([^'\" >]+)");
                    return srcMatch.Success ? srcMatch.Groups[1].Value : string.Empty;
                }).Where(s => !string.IsNullOrEmpty(s)).ToList();
        }

        /// <summary>
        /// Đếm tần suất xuất hiện của từng từ trong chuỗi.
        /// </summary>
        public static Dictionary<string, int> GetWordFrequency(this string text)
        {
            var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(text)) return dict;
            var words = Regex.Matches(text.ToLower(), "[a-zA-ZÀ-ỹ0-9]+", RegexOptions.Multiline).Cast<Match>().Select(m => m.Value);
            foreach (var w in words)
                if (dict.ContainsKey(w)) dict[w]++;
                else dict[w] = 1;
            return dict;
        }

        /// <summary>
        /// Tính điểm cảm xúc đơn giản (dựa trên từ khóa tích cực/tiêu cực truyền vào).
        /// </summary>
        public static int GetSentimentScore(this string text, IEnumerable<string> positiveWords, IEnumerable<string> negativeWords)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            int score = 0;
            var words = Regex.Matches(text.ToLower(), "[a-zA-ZÀ-ỹ0-9]+", RegexOptions.Multiline).Cast<Match>().Select(m => m.Value);
            foreach (var w in words)
            {
                if (positiveWords != null && positiveWords.Contains(w)) score++;
                if (negativeWords != null && negativeWords.Contains(w)) score--;
            }
            return score;
        }

        /// <summary>
        /// Lấy tên file từ đường dẫn.
        /// </summary>
        public static string GetFileNameFromPath(this string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Lấy thư mục cha từ đường dẫn.
        /// </summary>
        public static string GetDirectoryFromPath(this string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Kiểm tra số BHXH Việt Nam hợp lệ (10 số).
        /// </summary>
        public static bool IsValidSocialSecurityNumberVN(this string text)
        {
            return !string.IsNullOrWhiteSpace(text) && Regex.IsMatch(text, @"^\d{10}$");
        }

        /// <summary>
        /// Kiểm tra số hóa đơn thuế hợp lệ (mẫu: 01AA/19E, 10 ký tự).
        /// </summary>
        public static bool IsValidTaxInvoiceNumberVN(this string text)
        {
            return !string.IsNullOrWhiteSpace(text) && Regex.IsMatch(text, @"^\d{2}[A-Z]{2}/\d{2}[A-Z]$");
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là số La Mã.
        /// </summary>
        public static bool IsRomanNumeral(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, @"^(?=[MDCLXVI])M{0,4}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Lấy danh sách các block Unicode xuất hiện trong chuỗi.
        /// </summary>
        public static List<string> ExtractUnicodeBlocks(this string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            var blocks = new HashSet<string>();
            foreach (var c in text)
            {
                var block = CharUnicodeInfo.GetUnicodeCategory(c).ToString();
                blocks.Add(block);
            }
            return blocks.ToList();
        }

        /// <summary>
        /// Sinh slug với ký tự phân tách tùy ý (dùng cho SEO, URL động).
        /// </summary>
        public static string ToSlugWithSeparator(this string text, string separator = "-")
        {
            if (string.IsNullOrEmpty(text)) return text;
            var slug = text.RemoveDiacritics().ToLower();
            slug = Regex.Replace(slug, @"[^a-z0-9\s]", "");
            slug = Regex.Replace(slug, @"\s+", separator).Trim(separator.ToCharArray());
            return slug;
        }

        /// <summary>
        /// Lấy tất cả số tài khoản IBAN trong chuỗi.
        /// </summary>
        public static List<string> ExtractIBANs(this string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            return Regex.Matches(text, @"[A-Z]{2}\d{2}[A-Z0-9]{11,30}").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Lấy tất cả số thẻ tín dụng hợp lệ trong chuỗi (dùng thuật toán Luhn).
        /// </summary>
        public static List<string> ExtractCreditCardNumbers(this string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            var matches = Regex.Matches(text, @"\b\d{13,19}\b").Cast<Match>().Select(m => m.Value);
            return matches.Where(LuhnCheck).ToList();
        }
        private static bool LuhnCheck(string number)
        {
            int sum = 0;
            bool alternate = false;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(number[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }

        /// <summary>
        /// Mã hóa chuỗi sang base36 (dùng cho shortlink, mã hóa nhẹ).
        /// </summary>
        public static string ToBase36(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var bytes = Encoding.UTF8.GetBytes(text);
            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(Convert.ToString(b, 36));
            return sb.ToString();
        }

        /// <summary>
        /// Chuyển chuỗi sang mã Morse.
        /// </summary>
        public static string ToMorseCode(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var morse = new Dictionary<char, string>
            {
                {'A', ".-"}, {'B', "-..."}, {'C', "-.-."}, {'D', "-.."}, {'E', "."}, {'F', "..-."},
                {'G', "--."}, {'H', "...."}, {'I', ".."}, {'J', ".---"}, {'K', "-.-"}, {'L', ".-.."},
                {'M', "--"}, {'N', "-."}, {'O', "---"}, {'P', ".--."}, {'Q', "--.-"}, {'R', ".-."},
                {'S', "..."}, {'T', "-"}, {'U', "..-"}, {'V', "...-"}, {'W', ".--"}, {'X', "-..-"},
                {'Y', "-.--"}, {'Z', "--.."}, {'0', "-----"}, {'1', ".----"}, {'2', "..---"},
                {'3', "...--"}, {'4', "....-"}, {'5', "....."}, {'6', "-...."}, {'7', "--..."},
                {'8', "---.."}, {'9', "----."}, {' ', "/"}
            };
            var sb = new StringBuilder();
            foreach (var c in text.ToUpper())
                sb.Append(morse.ContainsKey(c) ? morse[c] + " " : "");
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Chuyển mã Morse về chuỗi gốc.
        /// </summary>
        public static string FromMorseCode(this string morseCode)
        {
            if (string.IsNullOrEmpty(morseCode)) return morseCode;
            var morse = new Dictionary<string, char>
            {
                {".-", 'A'}, {"-...", 'B'}, {"-.-.", 'C'}, {"-..", 'D'}, {".", 'E'}, {"..-.", 'F'},
                {"--.", 'G'}, {"....", 'H'}, {"..", 'I'}, {".---", 'J'}, {"-.-", 'K'}, {".-..", 'L'},
                {"--", 'M'}, {"-.", 'N'}, {"---", 'O'}, {".--.", 'P'}, {"--.-", 'Q'}, {".-.", 'R'},
                {"...", 'S'}, {"-", 'T'}, {"..-", 'U'}, {"...-", 'V'}, {".--", 'W'}, {"-..-", 'X'},
                {"-.--", 'Y'}, {"--..", 'Z'}, {"-----", '0'}, {".----", '1'}, {"..---", '2'},
                {"...--", '3'}, {"....-", '4'}, {".....", '5'}, {"-....", '6'}, {"--...", '7'},
                {"---..", '8'}, {"----.", '9'}, {"/", ' '}
            };
            var words = morseCode.Split(' ');
            var sb = new StringBuilder();
            foreach (var w in words)
                sb.Append(morse.ContainsKey(w) ? morse[w].ToString() : "");
            return sb.ToString();
        }

        /// <summary>
        /// Lấy tất cả thẻ meta trong HTML.
        /// </summary>
        public static List<string> ExtractMetaTags(this string html)
        {
            if (string.IsNullOrEmpty(html)) return new List<string>();
            return Regex.Matches(html, @"<meta [^>]*>", RegexOptions.IgnoreCase).Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Lấy tất cả thẻ script trong HTML.
        /// </summary>
        public static List<string> ExtractScriptTags(this string html)
        {
            if (string.IsNullOrEmpty(html)) return new List<string>();
            return Regex.Matches(html, @"<script[^>]*>.*?</script>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Lấy dữ liệu bảng (table) từ HTML (dạng thô).
        /// </summary>
        public static List<string> ExtractTableData(this string html)
        {
            if (string.IsNullOrEmpty(html)) return new List<string>();
            return Regex.Matches(html, @"<table[^>]*>.*?</table>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Lấy câu dài nhất trong chuỗi.
        /// </summary>
        public static string GetLongestSentence(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sentences = Regex.Split(text, @"(?<=[.!?])\s+");
            return sentences.OrderByDescending(s => s.Length).FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Lấy câu ngắn nhất trong chuỗi.
        /// </summary>
        public static string GetShortestSentence(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sentences = Regex.Split(text, @"(?<=[.!?])\s+");
            return sentences.Where(s => !string.IsNullOrWhiteSpace(s)).OrderBy(s => s.Length).FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Tính độ dài trung bình của từ trong chuỗi.
        /// </summary>
        public static double GetAverageWordLength(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            var words = Regex.Matches(text, "[a-zA-ZÀ-ỹ0-9]+", RegexOptions.Multiline).Cast<Match>().Select(m => m.Value);
            int count = words.Count();
            int total = words.Sum(w => w.Length);
            return count == 0 ? 0 : (double)total / count;
        }

        /// <summary>
        /// Kiểm tra đường dẫn hợp lệ cho Windows.
        /// </summary>
        public static bool IsValidWindowsPath(this string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            char[] invalidChars = Path.GetInvalidPathChars();
            return !text.Any(c => invalidChars.Contains(c)) && Regex.IsMatch(text, @"^[a-zA-Z]:\\");
        }

        /// <summary>
        /// Kiểm tra đường dẫn hợp lệ cho Linux.
        /// </summary>
        public static bool IsValidLinuxPath(this string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            char[] invalidChars = Path.GetInvalidPathChars();
            return !text.Any(c => invalidChars.Contains(c)) && text.StartsWith("/");
        }

        /// <summary>
        /// Kiểm tra chuỗi có phù hợp để sinh QR code (không quá dài, không chứa ký tự lỗi).
        /// </summary>
        public static bool IsValidQRCodeContent(this string text, int maxLength = 2953)
        {
            if (string.IsNullOrEmpty(text)) return false;
            // QR code chuẩn (version 40, error correction L) tối đa 2953 ký tự (byte mode)
            return text.Length <= maxLength && !text.Any(c => char.IsControl(c) && c != '\n' && c != '\r');
        }

        /// <summary>
        /// Kiểm tra số BIN ngân hàng Việt Nam hợp lệ (6 số đầu của thẻ ATM).
        /// </summary>
        public static bool IsValidBankBinVN(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, @"^\d{6}$");
        }

        /// <summary>
        /// Phát hiện chuỗi có chứa tiếng Việt hay không.
        /// </summary>
        public static bool DetectVietnamese(this string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            return Regex.IsMatch(text, "[àáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ]", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Chuẩn hóa tiếng Việt: loại bỏ khoảng trắng thừa, chuẩn hóa dấu.
        /// </summary>
        public static string NormalizeVietnamese(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            text = Regex.Replace(text, @"\s+", " ").Trim();
            // Có thể bổ sung chuẩn hóa dấu nếu cần
            return text;
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt (chỉ giữ ký tự Latin).
        /// </summary>
        public static string RemoveVietnameseTone(this string text)
        {
            return text.RemoveDiacritics();
        }

        /// <summary>
        /// Chuyển tiếng Việt sang mã Telex.
        /// </summary>
        public static string ToTelex(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            // Bảng chuyển đổi giản lược, không đầy đủ hết trường hợp
            var dict = new Dictionary<char, string> {
                {'à', "af"}, {'á', "as"}, {'ả', "ar"}, {'ã', "ax"}, {'ạ', "aj"},
                {'â', "aa"}, {'ầ', "aaf"}, {'ấ', "aas"}, {'ẩ', "aar"}, {'ẫ', "aax"}, {'ậ', "aaj"},
                {'ă', "aw"}, {'ằ', "awf"}, {'ắ', "aws"}, {'ẳ', "awr"}, {'ẵ', "awx"}, {'ặ', "awj"},
                {'è', "ef"}, {'é', "es"}, {'ẻ', "er"}, {'ẽ', "ex"}, {'ẹ', "ej"},
                {'ê', "ee"}, {'ề', "eef"}, {'ế', "ees"}, {'ể', "eer"}, {'ễ', "eex"}, {'ệ', "eej"},
                {'ì', "if"}, {'í', "is"}, {'ỉ', "ir"}, {'ĩ', "ix"}, {'ị', "ij"},
                {'ò', "of"}, {'ó', "os"}, {'ỏ', "or"}, {'õ', "ox"}, {'ọ', "oj"},
                {'ô', "oo"}, {'ồ', "oof"}, {'ố', "oos"}, {'ổ', "oor"}, {'ỗ', "oox"}, {'ộ', "ooj"},
                {'ơ', "ow"}, {'ờ', "owf"}, {'ớ', "ows"}, {'ở', "owr"}, {'ỡ', "owx"}, {'ợ', "owj"},
                {'ù', "uf"}, {'ú', "us"}, {'ủ', "ur"}, {'ũ', "ux"}, {'ụ', "uj"},
                {'ư', "uw"}, {'ừ', "uwf"}, {'ứ', "uws"}, {'ử', "uwr"}, {'ữ', "uwx"}, {'ự', "uwj"},
                {'ỳ', "yf"}, {'ý', "ys"}, {'ỷ', "yr"}, {'ỹ', "yx"}, {'ỵ', "yj"},
                {'đ', "dd"}
            };
            var sb = new StringBuilder();
            foreach (var c in text)
                sb.Append(dict.ContainsKey(c) ? dict[c] : c.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// Chuyển tiếng Việt sang mã VNI.
        /// </summary>
        public static string ToVni(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            var dict = new Dictionary<char, string> {
                {'à', "a`"}, {'á', "a'"}, {'ả', "a?"}, {'ã', "a~"}, {'ạ', "a."},
                {'â', "a^"}, {'ầ', "a^`"}, {'ấ', "a^'"}, {'ẩ', "a^?"}, {'ẫ', "a^~"}, {'ậ', "a^."},
                {'ă', "a("}, {'ằ', "a(`"}, {'ắ', "a('"}, {'ẳ', "a(?"}, {'ẵ', "a(~"}, {'ặ', "a(."},
                {'è', "e`"}, {'é', "e'"}, {'ẻ', "e?"}, {'ẽ', "e~"}, {'ẹ', "e."},
                {'ê', "e^"}, {'ề', "e^`"}, {'ế', "e^'"}, {'ể', "e^?"}, {'ễ', "e^~"}, {'ệ', "e^."},
                {'ì', "i`"}, {'í', "i'"}, {'ỉ', "i?"}, {'ĩ', "i~"}, {'ị', "i."},
                {'ò', "o`"}, {'ó', "o'"}, {'ỏ', "o?"}, {'õ', "o~"}, {'ọ', "o."},
                {'ô', "o^"}, {'ồ', "o^`"}, {'ố', "o^'"}, {'ổ', "o^?"}, {'ỗ', "o^~"}, {'ộ', "o^."},
                {'ơ', "o+"}, {'ờ', "o+`"}, {'ớ', "o+'"}, {'ở', "o+?"}, {'ỡ', "o+~"}, {'ợ', "o+."},
                {'ù', "u`"}, {'ú', "u'"}, {'ủ', "u?"}, {'ũ', "u~"}, {'ụ', "u."},
                {'ư', "u+"}, {'ừ', "u+`"}, {'ứ', "u+'"}, {'ử', "u+?"}, {'ữ', "u+~"}, {'ự', "u+."},
                {'ỳ', "y`"}, {'ý', "y'"}, {'ỷ', "y?"}, {'ỹ', "y~"}, {'ỵ', "y."},
                {'đ', "d9"}
            };
            var sb = new StringBuilder();
            foreach (var c in text)
                sb.Append(dict.ContainsKey(c) ? dict[c] : c.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// Tách từ tiếng Việt đơn giản (dựa vào dấu cách, không NLP nâng cao).
        /// </summary>
        public static List<string> VietnameseWordSegmentation(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return text.Trim().Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Kiểm tra tên có phải tên người Việt Nam.
        /// </summary>
        public static bool IsVietnameseName(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            // Đơn giản: kiểm tra có dấu và >= 2 từ
            return text.DetectVietnamese() && text.Trim().Split(' ').Length >= 2;
        }

        /// <summary>
        /// Kiểm tra từ có phải từ tiếng Việt.
        /// </summary>
        public static bool IsVietnameseWord(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return text.DetectVietnamese();
        }

        /// <summary>
        /// Đếm số âm tiết tiếng Việt trong chuỗi.
        /// </summary>
        public static int VietnameseSyllableCount(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return text.Trim().Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>
        /// Đếm số từ tiếng Việt (tách theo dấu cách).
        /// </summary>
        public static int VietnameseWordCount(this string text)
        {
            return text.VietnameseSyllableCount();
        }

        /// <summary>
        /// Đếm số câu tiếng Việt.
        /// </summary>
        public static int VietnameseSentenceCount(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return Regex.Split(text, @"[.!?…]+[""]?\s+").Count(s => !string.IsNullOrWhiteSpace(s));
        }

        /// <summary>
        /// Lấy chữ cái đầu của từng từ tiếng Việt (viết tắt tên).
        /// </summary>
        public static string VietnameseInitials(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            return string.Concat(text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(w => char.ToUpper(w[0])));
        }

        /// <summary>
        /// Xác định loại dấu tiếng Việt (sắc, huyền, hỏi, ngã, nặng, ngang).
        /// </summary>
        public static string VietnameseAccentType(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "ngang";
            if (Regex.IsMatch(text, "[áéíóúýắấếốớứ]")) return "sắc";
            if (Regex.IsMatch(text, "[àèìòùỳằầềồờừ]")) return "huyền";
            if (Regex.IsMatch(text, "[ảẻỉỏủỷẳẩểổởử]")) return "hỏi";
            if (Regex.IsMatch(text, "[ãẽĩõũỹẵẫễỗỡữ]")) return "ngã";
            if (Regex.IsMatch(text, "[ạẹịọụỵặậệộợự]")) return "nặng";
            return "ngang";
        }

        /// <summary>
        /// Chuyển tên tiếng Việt sang tên tiếng Anh (bỏ dấu, đảo thứ tự nếu cần).
        /// </summary>
        public static string VietnameseToEnglishName(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            var parts = text.RemoveDiacritics().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) return text.RemoveDiacritics();
            return parts.Last() + " " + string.Join(" ", parts.Take(parts.Length - 1));
        }

        /// <summary>
        /// Loại bỏ tên đệm trong tên người Việt.
        /// </summary>
        public static string RemoveVietnameseMiddleName(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            var parts = text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length <= 2) return text;
            return parts.First() + " " + parts.Last();
        }

        /// <summary>
        /// Kiểm tra địa chỉ có phải địa chỉ Việt Nam hợp lệ.
        /// </summary>
        public static bool IsValidVietnameseAddress(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            // Đơn giản: có tên tỉnh/thành phố
            return VietnameseProvinces.Any(p => text.Contains(p, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Trích xuất tên tỉnh/thành phố Việt Nam trong chuỗi.
        /// </summary>
        public static List<string> ExtractVietnameseProvinces(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return VietnameseProvinces.Where(p => text.Contains(p, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Trích xuất tên quận/huyện Việt Nam trong chuỗi (demo, cần mở rộng danh sách).
        /// </summary>
        public static List<string> ExtractVietnameseDistricts(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return VietnameseDistricts.Where(d => text.Contains(d, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Trích xuất số điện thoại Việt Nam trong chuỗi.
        /// </summary>
        public static List<string> ExtractVietnamesePhone(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"(0|\+84)[0-9]{9,10}").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Trích xuất số tiền Việt Nam trong chuỗi.
        /// </summary>
        public static List<string> ExtractVietnameseCurrency(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"\d{1,3}(\.\d{3})*(\s?VNĐ|\s?đ|\s?VND)?").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Trích xuất ngày/tháng/năm kiểu Việt Nam (dd/MM/yyyy).
        /// </summary>
        public static List<string> ExtractVietnameseDate(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"\b\d{1,2}/\d{1,2}/\d{4}\b").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Trích xuất số CMND/CCCD Việt Nam trong chuỗi.
        /// </summary>
        public static List<string> ExtractVietnameseIdCard(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"\b\d{9}\b|\b\d{12}\b").Cast<Match>().Select(m => m.Value).ToList();
        }

        /// <summary>
        /// Trích xuất mã số thuế Việt Nam trong chuỗi.
        /// </summary>
        public static List<string> ExtractVietnameseTaxCode(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"\b\d{10}\b|\b\d{10}-\d{3}\b").Cast<Match>().Select(m => m.Value).ToList();
        }

        // Danh sách tỉnh/thành phố Việt Nam (rút gọn demo)
        private static readonly List<string> VietnameseProvinces = new List<string>
        {
            "Hà Nội", "Hồ Chí Minh", "Đà Nẵng", "Hải Phòng", "Cần Thơ", "An Giang", "Bà Rịa - Vũng Tàu", "Bắc Giang", "Bắc Kạn", "Bạc Liêu", "Bắc Ninh", "Bến Tre", "Bình Định", "Bình Dương", "Bình Phước", "Bình Thuận", "Cà Mau", "Cao Bằng", "Đắk Lắk", "Đắk Nông", "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Giang", "Hà Nam", "Hà Tĩnh", "Hải Dương", "Hậu Giang", "Hòa Bình", "Hưng Yên", "Khánh Hòa", "Kiên Giang", "Kon Tum", "Lai Châu", "Lâm Đồng", "Lạng Sơn", "Lào Cai", "Long An", "Nam Định", "Nghệ An", "Ninh Bình", "Ninh Thuận", "Phú Thọ", "Phú Yên", "Quảng Bình", "Quảng Nam", "Quảng Ngãi", "Quảng Ninh", "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh", "Thái Bình", "Thái Nguyên", "Thanh Hóa", "Thừa Thiên Huế", "Tiền Giang", "Trà Vinh", "Tuyên Quang", "Vĩnh Long", "Vĩnh Phúc", "Yên Bái"
        };

        // Danh sách quận/huyện demo (cần mở rộng thực tế)
        private static readonly List<string> VietnameseDistricts = new List<string>
        {
            "Ba Đình", "Hoàn Kiếm", "Đống Đa", "Hai Bà Trưng", "Thanh Xuân", "Cầu Giấy", "Long Biên", "Nam Từ Liêm", "Bắc Từ Liêm", "Tây Hồ", "Hoàng Mai", "Thanh Trì", "Gia Lâm", "Đông Anh", "Sóc Sơn"
        };

        /// <summary>
        /// Sinh mã chứng từ tự động theo tiền tố, ngày/tháng, số thứ tự.
        /// </summary>
        public static string GenerateDocumentCode(this string prefix, DateTime date, int sequence, string format = "yyyyMMdd")
        {
            return $"{prefix}{date.ToString(format)}{sequence:D4}";
        }

        /// <summary>
        /// Định dạng số tiền theo chuẩn ERP Việt Nam.
        /// </summary>
        public static string ToERPAmountString(this decimal amount)
        {
            return amount.ToString("#,##0", new CultureInfo("vi-VN"));
        }

        /// <summary>
        /// Chuyển số thành chữ tiếng Việt (cho phiếu chi/phiếu thu, placeholder).
        /// </summary>
        public static string ToVietnameseWords(this decimal number)
        {
            // Placeholder: cần logic chuyển số thành chữ tiếng Việt
            return number.ToString();
        }

        /// <summary>
        /// Kiểm tra ngày có thuộc kỳ kế toán không.
        /// </summary>
        public static bool IsInAccountingPeriod(this DateTime date, DateTime periodStart, DateTime periodEnd)
        {
            return date >= periodStart && date <= periodEnd;
        }

        /// <summary>
        /// Sinh mã hàng hóa tự động.
        /// </summary>
        public static string GenerateProductCode(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D8}";
        }

        /// <summary>
        /// Kiểm tra định dạng mã hợp đồng.
        /// </summary>
        public static bool IsValidContractCode(this string text)
        {
            return Regex.IsMatch(text, @"^HD\d{6,}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Trích xuất số tài khoản ngân hàng Việt Nam trong chuỗi.
        /// </summary>
        public static List<string> ExtractBankAccountNumbers(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"\b\d{8,16}\b").Cast<Match>().Select(m => m.Value).ToList();
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
        /// Trích xuất số hợp đồng trong chuỗi.
        /// </summary>
        public static List<string> ExtractContractNumbers(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Matches(text, @"HD\d{6,}").Cast<Match>().Select(m => m.Value).ToList();
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

        /// <summary>
        /// Kiểm tra số điện thoại Việt Nam hợp lệ (dành cho ERP).
        /// </summary>
        public static bool IsValidERPPhone(this string text)
        {
            return Regex.IsMatch(text, @"^(0|\+84)[0-9]{9,10}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Kiểm tra email hợp lệ (dành cho ERP).
        /// </summary>
        public static bool IsValidERPEmail(this string text)
        {
            return Regex.IsMatch(text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Trích xuất thông tin khách hàng (mã, tên, điện thoại, email) từ chuỗi (giản lược).
        /// </summary>
        public static (string Code, string Name, string Phone, string Email) ExtractCustomerInfo(this string text)
        {
            string code = Regex.Match(text, @"KH\d{6,}").Value;
            string phone = Regex.Match(text, @"(0|\+84)[0-9]{9,10}").Value;
            string email = Regex.Match(text, @"[^@\s]+@[^@\s]+\.[^@\s]+").Value;
            string name = text.Replace(code, "").Replace(phone, "").Replace(email, "").Trim();
            return (code, name, phone, email);
        }

        /// <summary>
        /// Sinh mã bút toán tự động.
        /// </summary>
        public static string GenerateJournalEntryCode(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D7}";
        }

        /// <summary>
        /// Định dạng số dư, số phát sinh theo chuẩn ERP.
        /// </summary>
        public static string ToERPBalanceString(this decimal amount)
        {
            return amount.ToString("#,##0.00", new CultureInfo("vi-VN"));
        }

        /// <summary>
        /// Sinh chuỗi kỳ kế toán (tháng/năm/quý).
        /// </summary>
        public static string ToAccountingPeriodString(this DateTime date, string type = "month")
        {
            if (type == "month") return $"{date:MM/yyyy}";
            if (type == "quarter") return $"Q{((date.Month - 1) / 3 + 1)}/{date.Year}";
            return date.Year.ToString();
        }

        /// <summary>
        /// Kiểm tra ngày có phải là ngày cuối tháng/quý/năm.
        /// </summary>
        public static bool IsEndOfPeriod(this DateTime date, string type = "month")
        {
            if (type == "month") return date.AddDays(1).Month != date.Month;
            if (type == "quarter") return date.Month % 3 == 0 && date.AddDays(1).Month != date.Month;
            if (type == "year") return date.Month == 12 && date.Day == 31;
            return false;
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

        /// <summary>
        /// Sinh mã khách hàng tự động.
        /// </summary>
        public static string GenerateCustomerCodeHR(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D6}";
        }

        /// <summary>
        /// Sinh mã nhân viên tự động.
        /// </summary>
        public static string GenerateEmployeeCodeHR(this string prefix, int sequence)
        {
            return $"{prefix}{sequence:D5}";
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
        /// Kiểm tra hợp lệ số tài khoản ngân hàng Việt Nam (8-16 số).
        /// </summary>
        public static bool IsValidBankAccountNumber(this string text)
        {
            return Regex.IsMatch(text, @"^\d{8,16}$", RegexOptions.Compiled);
        }
    }
}