using System;
using System.Text;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper xử lý Base64 encoding/decoding
    /// </summary>
    public static class Base64Helper
    {
        /// <summary>
        /// Chuyển đổi string thành base64
        /// </summary>
        /// <param name="input">String cần encode</param>
        /// <param name="encoding">Encoding sử dụng (mặc định UTF8)</param>
        /// <returns>Base64 string</returns>
        public static string Encode(string input, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            encoding ??= Encoding.UTF8;
            var bytes = encoding.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Chuyển đổi base64 thành string
        /// </summary>
        /// <param name="base64String">Base64 string cần decode</param>
        /// <param name="encoding">Encoding sử dụng (mặc định UTF8)</param>
        /// <returns>String đã decode</returns>
        public static string Decode(string base64String, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            try
            {
                encoding ??= Encoding.UTF8;
                var bytes = Convert.FromBase64String(base64String);
                return encoding.GetString(bytes);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi decode base64: {ex.Message}", nameof(base64String));
            }
        }

        /// <summary>
        /// Chuyển đổi byte array thành base64
        /// </summary>
        /// <param name="bytes">Byte array cần encode</param>
        /// <returns>Base64 string</returns>
        public static string EncodeBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Chuyển đổi base64 thành byte array
        /// </summary>
        /// <param name="base64String">Base64 string cần decode</param>
        /// <returns>Byte array đã decode</returns>
        public static byte[] DecodeBytes(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi decode base64 thành bytes: {ex.Message}", nameof(base64String));
            }
        }

        /// <summary>
        /// Chuyển đổi base64 thành base64 URL safe (thay thế + thành -, / thành _)
        /// </summary>
        /// <param name="base64String">Base64 string thường</param>
        /// <returns>Base64 URL safe string</returns>
        public static string ToUrlSafe(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            return base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        /// <summary>
        /// Chuyển đổi base64 URL safe thành base64 thường
        /// </summary>
        /// <param name="urlSafeBase64">Base64 URL safe string</param>
        /// <returns>Base64 string thường</returns>
        public static string FromUrlSafe(string urlSafeBase64)
        {
            if (string.IsNullOrEmpty(urlSafeBase64))
                return string.Empty;

            var base64 = urlSafeBase64.Replace('-', '+').Replace('_', '/');
            
            // Thêm padding nếu cần
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return base64;
        }

        /// <summary>
        /// Kiểm tra xem string có phải là base64 hợp lệ không
        /// </summary>
        /// <param name="input">String cần kiểm tra</param>
        /// <returns>True nếu là base64 hợp lệ</returns>
        public static bool IsValidBase64(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            try
            {
                Convert.FromBase64String(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra xem string có phải là base64 URL safe hợp lệ không
        /// </summary>
        /// <param name="input">String cần kiểm tra</param>
        /// <returns>True nếu là base64 URL safe hợp lệ</returns>
        public static bool IsValidUrlSafeBase64(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            try
            {
                var base64 = FromUrlSafe(input);
                Convert.FromBase64String(base64);
                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// Chuyển đổi base64 thành data URL
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <param name="mimeType">MIME type (mặc định application/octet-stream)</param>
        /// <returns>Data URL</returns>
        public static string ToDataUrl(string base64String, string mimeType = "application/octet-stream")
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            return $"data:{mimeType};base64,{base64String}";
        }

        /// <summary>
        /// Trích xuất base64 từ data URL
        /// </summary>
        /// <param name="dataUrl">Data URL</param>
        /// <returns>Base64 string</returns>
        public static string FromDataUrl(string dataUrl)
        {
            if (string.IsNullOrEmpty(dataUrl) || !dataUrl.StartsWith("data:"))
                throw new ArgumentException("Không phải data URL hợp lệ", nameof(dataUrl));

            try
            {
                var commaIndex = dataUrl.IndexOf(',');
                if (commaIndex == -1)
                    throw new ArgumentException("Data URL không có content", nameof(dataUrl));

                return dataUrl.Substring(commaIndex + 1);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi parse data URL: {ex.Message}", nameof(dataUrl));
            }
        }

        /// <summary>
        /// Lấy MIME type từ data URL
        /// </summary>
        /// <param name="dataUrl">Data URL</param>
        /// <returns>MIME type</returns>
        public static string GetMimeTypeFromDataUrl(string dataUrl)
        {
            if (string.IsNullOrEmpty(dataUrl) || !dataUrl.StartsWith("data:"))
                return null;

            try
            {
                var colonIndex = dataUrl.IndexOf(':');
                var commaIndex = dataUrl.IndexOf(',');
                if (colonIndex == -1 || commaIndex == -1)
                    return null;

                var mimePart = dataUrl.Substring(colonIndex + 1, commaIndex - colonIndex - 1);
                var semicolonIndex = mimePart.IndexOf(';');
                
                if (semicolonIndex != -1)
                    return mimePart.Substring(0, semicolonIndex);
                
                return mimePart;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Tính độ dài byte array từ base64 string (không decode)
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Độ dài byte array</returns>
        public static int GetByteLength(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return 0;

            var length = base64String.Length;
            var padding = 0;

            if (length > 0 && base64String[length - 1] == '=')
                padding++;
            if (length > 1 && base64String[length - 2] == '=')
                padding++;

            return (length * 3 / 4) - padding;
        }

        /// <summary>
        /// Chuyển đổi base64 thành hex string
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Hex string</returns>
        public static string ToHex(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            try
            {
                var bytes = DecodeBytes(base64String);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi chuyển đổi base64 thành hex: {ex.Message}", nameof(base64String));
            }
        }

        /// <summary>
        /// Chuyển đổi hex string thành base64
        /// </summary>
        /// <param name="hexString">Hex string</param>
        /// <returns>Base64 string</returns>
        public static string FromHex(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
                return string.Empty;

            try
            {
                var bytes = new byte[hexString.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }
                return EncodeBytes(bytes);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi chuyển đổi hex thành base64: {ex.Message}", nameof(hexString));
            }
        }

        /// <summary>
        /// Chuyển đổi base64 thành base64 với padding tùy chỉnh
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <param name="padding">Ký tự padding (mặc định '=')</param>
        /// <returns>Base64 string với padding tùy chỉnh</returns>
        public static string WithCustomPadding(string base64String, char padding = '=')
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            var length = base64String.Length;
            var remainder = length % 4;
            
            if (remainder == 0)
                return base64String;

            var neededPadding = 4 - remainder;
            return base64String + new string(padding, neededPadding);
        }

        /// <summary>
        /// Loại bỏ padding khỏi base64 string
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Base64 string không có padding</returns>
        public static string RemovePadding(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            return base64String.TrimEnd('=');
        }

        /// <summary>
        /// Chuyển đổi base64 thành base64 với padding chuẩn
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Base64 string với padding chuẩn</returns>
        public static string WithStandardPadding(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            return WithCustomPadding(base64String, '=');
        }

        /// <summary>
        /// Chuyển đổi base64 thành base64 với line breaks (76 ký tự mỗi dòng)
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <param name="lineLength">Độ dài mỗi dòng (mặc định 76)</param>
        /// <returns>Base64 string với line breaks</returns>
        public static string WithLineBreaks(string base64String, int lineLength = 76)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            var result = new StringBuilder();
            for (int i = 0; i < base64String.Length; i += lineLength)
            {
                var length = Math.Min(lineLength, base64String.Length - i);
                result.AppendLine(base64String.Substring(i, length));
            }

            return result.ToString().TrimEnd('\r', '\n');
        }

        /// <summary>
        /// Loại bỏ line breaks khỏi base64 string
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Base64 string không có line breaks</returns>
        public static string RemoveLineBreaks(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            return base64String.Replace("\r", "").Replace("\n", "");
        }

        /// <summary>
        /// Chuyển đổi base64 thành base64 với format PEM
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <param name="header">Header (mặc định "-----BEGIN DATA-----")</param>
        /// <param name="footer">Footer (mặc định "-----END DATA-----")</param>
        /// <returns>Base64 string với format PEM</returns>
        public static string ToPemFormat(string base64String, string header = "-----BEGIN DATA-----", string footer = "-----END DATA-----")
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            var withBreaks = WithLineBreaks(base64String);
            return $"{header}\n{withBreaks}\n{footer}";
        }

        /// <summary>
        /// Trích xuất base64 từ format PEM
        /// </summary>
        /// <param name="pemString">PEM string</param>
        /// <returns>Base64 string</returns>
        public static string FromPemFormat(string pemString)
        {
            if (string.IsNullOrEmpty(pemString))
                return string.Empty;

            var lines = pemString.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var base64Lines = lines.Where(line => !line.StartsWith("-----")).ToArray();
            
            return RemoveLineBreaks(string.Join("", base64Lines));
        }

        /// <summary>
        /// Chuyển đổi base64 thành base64 với encoding tùy chỉnh
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="encoding">Encoding sử dụng</param>
        /// <returns>Base64 string</returns>
        public static string EncodeWithEncoding(string input, Encoding encoding)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var bytes = encoding.GetBytes(input);
            return EncodeBytes(bytes);
        }

        /// <summary>
        /// Chuyển đổi base64 thành string với encoding tùy chỉnh
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <param name="encoding">Encoding sử dụng</param>
        /// <returns>String đã decode</returns>
        public static string DecodeWithEncoding(string base64String, Encoding encoding)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            try
            {
                var bytes = DecodeBytes(base64String);
                return encoding.GetString(bytes);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi decode base64: {ex.Message}", nameof(base64String));
            }
        }

        /// <summary>
        /// Chuyển đổi base64 thành base64 với charset tùy chỉnh
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <param name="charset">Charset (ví dụ: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/")</param>
        /// <returns>Base64 string với charset tùy chỉnh</returns>
        public static string WithCustomCharset(string base64String, string charset)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            if (string.IsNullOrEmpty(charset) || charset.Length != 64)
                throw new ArgumentException("Charset phải có đúng 64 ký tự", nameof(charset));

            var standardCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var result = new StringBuilder(base64String.Length);

            foreach (char c in base64String)
            {
                if (c == '=')
                {
                    result.Append(c);
                }
                else
                {
                    var index = standardCharset.IndexOf(c);
                    if (index >= 0)
                        result.Append(charset[index]);
                    else
                        result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Chuyển đổi base64 với charset tùy chỉnh về charset chuẩn
        /// </summary>
        /// <param name="base64String">Base64 string với charset tùy chỉnh</param>
        /// <param name="charset">Charset tùy chỉnh</param>
        /// <returns>Base64 string với charset chuẩn</returns>
        public static string FromCustomCharset(string base64String, string charset)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            if (string.IsNullOrEmpty(charset) || charset.Length != 64)
                throw new ArgumentException("Charset phải có đúng 64 ký tự", nameof(charset));

            var standardCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var result = new StringBuilder(base64String.Length);

            foreach (char c in base64String)
            {
                if (c == '=')
                {
                    result.Append(c);
                }
                else
                {
                    var index = charset.IndexOf(c);
                    if (index >= 0)
                        result.Append(standardCharset[index]);
                    else
                        result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Tạo base64 ngẫu nhiên với độ dài tùy chỉnh
        /// </summary>
        /// <param name="byteLength">Độ dài byte array</param>
        /// <returns>Base64 string ngẫu nhiên</returns>
        public static string GenerateRandom(int byteLength)
        {
            if (byteLength <= 0)
                throw new ArgumentException("Byte length phải lớn hơn 0", nameof(byteLength));

            var random = new Random();
            var bytes = new byte[byteLength];
            random.NextBytes(bytes);
            
            return EncodeBytes(bytes);
        }

        /// <summary>
        /// So sánh hai base64 string (bỏ qua padding và line breaks)
        /// </summary>
        /// <param name="base64String1">Base64 string thứ nhất</param>
        /// <param name="base64String2">Base64 string thứ hai</param>
        /// <returns>True nếu bằng nhau</returns>
        public static bool AreEqual(string base64String1, string base64String2)
        {
            if (string.IsNullOrEmpty(base64String1) && string.IsNullOrEmpty(base64String2))
                return true;

            if (string.IsNullOrEmpty(base64String1) || string.IsNullOrEmpty(base64String2))
                return false;

            var normalized1 = RemoveLineBreaks(RemovePadding(base64String1));
            var normalized2 = RemoveLineBreaks(RemovePadding(base64String2));

            return string.Equals(normalized1, normalized2, StringComparison.Ordinal);
        }

        /// <summary>
        /// Lấy hash của base64 string
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Hash string</returns>
        public static string GetHash(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            var bytes = DecodeBytes(base64String);
            if (bytes == null)
                return string.Empty;

            var hash = 0;
            foreach (byte b in bytes)
            {
                hash = (hash * 31 + b) & 0x7FFFFFFF;
            }

            return hash.ToString("X8");
        }

        /// <summary>
        /// Xác định file extension từ base64 string bằng cách phân tích magic bytes
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>File extension (ví dụ: .jpg, .png, .mp3, .pdf)</returns>
        public static string GetFileExtension(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            try
            {
                var bytes = DecodeBytes(base64String);
                if (bytes == null || bytes.Length < 4)
                    return string.Empty;

                // Kiểm tra các magic bytes để xác định file type
                return GetExtensionFromMagicBytes(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Xác định MIME type từ base64 string
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>MIME type (ví dụ: image/jpeg, audio/mp3, application/pdf)</returns>
        public static string GetMimeType(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            try
            {
                var bytes = DecodeBytes(base64String);
                if (bytes == null || bytes.Length < 4)
                    return "application/octet-stream";

                return GetMimeTypeFromMagicBytes(bytes);
            }
            catch
            {
                return "application/octet-stream";
            }
        }

        /// <summary>
        /// Xác định extension từ magic bytes
        /// </summary>
        /// <param name="bytes">Byte array</param>
        /// <returns>File extension</returns>
        private static string GetExtensionFromMagicBytes(byte[] bytes)
        {
            // Images
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xD8)
                return ".jpg";
            if (bytes.Length >= 8 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                return ".png";
            if (bytes.Length >= 6 && bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
                return ".gif";
            if (bytes.Length >= 2 && bytes[0] == 0x42 && bytes[1] == 0x4D)
                return ".bmp";
            if (bytes.Length >= 4 && bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0x01 && bytes[3] == 0x00)
                return ".ico";
            if (bytes.Length >= 4 && bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46)
                return ".webp";

            // Audio
            if (bytes.Length >= 4 && bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46)
            {
                if (bytes.Length >= 8 && bytes[4] == 0x57 && bytes[5] == 0x41 && bytes[6] == 0x56 && bytes[7] == 0x45)
                    return ".wav";
                if (bytes.Length >= 8 && bytes[4] == 0x41 && bytes[5] == 0x56 && bytes[6] == 0x49 && bytes[7] == 0x20)
                    return ".avi";
            }
            if (bytes.Length >= 3 && bytes[0] == 0x49 && bytes[1] == 0x44 && bytes[2] == 0x33)
                return ".mp3";
            if (bytes.Length >= 4 && bytes[0] == 0x4F && bytes[1] == 0x67 && bytes[2] == 0x67 && bytes[3] == 0x53)
                return ".ogg";
            if (bytes.Length >= 4 && bytes[0] == 0x66 && bytes[1] == 0x4C && bytes[2] == 0x61 && bytes[3] == 0x43)
                return ".flac";

            // Video
            if (bytes.Length >= 4 && bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0x01 && bytes[3] == 0xB3)
                return ".mp4";
            if (bytes.Length >= 4 && bytes[0] == 0x1A && bytes[1] == 0x45 && bytes[2] == 0xDF && bytes[3] == 0xA3)
                return ".mkv";
            if (bytes.Length >= 4 && bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46)
            {
                if (bytes.Length >= 8 && bytes[4] == 0x41 && bytes[5] == 0x56 && bytes[6] == 0x49 && bytes[7] == 0x20)
                    return ".avi";
            }

            // Documents
            if (bytes.Length >= 4 && bytes[0] == 0x25 && bytes[1] == 0x50 && bytes[2] == 0x44 && bytes[3] == 0x46)
                return ".pdf";
            if (bytes.Length >= 4 && bytes[0] == 0x50 && bytes[1] == 0x4B && bytes[2] == 0x03 && bytes[3] == 0x04)
                return ".zip";
            if (bytes.Length >= 4 && bytes[0] == 0x50 && bytes[1] == 0x4B && bytes[2] == 0x05 && bytes[3] == 0x06)
                return ".zip";
            if (bytes.Length >= 4 && bytes[0] == 0x50 && bytes[1] == 0x4B && bytes[2] == 0x07 && bytes[3] == 0x08)
                return ".zip";
            if (bytes.Length >= 2 && bytes[0] == 0x1F && bytes[1] == 0x8B)
                return ".gz";
            if (bytes.Length >= 4 && bytes[0] == 0x37 && bytes[1] == 0x7A && bytes[2] == 0xBC && bytes[3] == 0xAF)
                return ".7z";
            if (bytes.Length >= 4 && bytes[0] == 0x52 && bytes[1] == 0x61 && bytes[2] == 0x72 && bytes[3] == 0x21)
                return ".rar";

            // Office documents
            if (bytes.Length >= 8 && bytes[0] == 0xD0 && bytes[1] == 0xCF && bytes[2] == 0x11 && bytes[3] == 0xE0)
                return ".doc"; // .doc, .xls, .ppt
            if (bytes.Length >= 4 && bytes[0] == 0x50 && bytes[1] == 0x4B && bytes[2] == 0x03 && bytes[3] == 0x04)
            {
                // Kiểm tra thêm để phân biệt với ZIP
                if (bytes.Length >= 30)
                {
                    var content = System.Text.Encoding.ASCII.GetString(bytes, 0, Math.Min(100, bytes.Length));
                    if (content.Contains("[Content_Types].xml"))
                        return ".docx"; // .docx, .xlsx, .pptx
                }
            }

            // Text files
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return ".txt"; // UTF-8 with BOM
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
                return ".txt"; // UTF-16 LE
            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
                return ".txt"; // UTF-16 BE

            // XML/HTML
            if (bytes.Length >= 5 && bytes[0] == 0x3C && bytes[1] == 0x3F && bytes[2] == 0x78 && bytes[3] == 0x6D && bytes[4] == 0x6C)
                return ".xml";
            if (bytes.Length >= 6 && bytes[0] == 0x3C && bytes[1] == 0x68 && bytes[2] == 0x74 && bytes[3] == 0x6D && bytes[4] == 0x6C)
                return ".html";

            // JSON
            if (bytes.Length >= 1 && bytes[0] == 0x7B) // '{'
                return ".json";

            // CSV
            if (bytes.Length >= 1 && (bytes[0] == 0x2C || bytes[0] == 0x3B)) // ',' or ';'
                return ".csv";

            return string.Empty;
        }

        /// <summary>
        /// Xác định MIME type từ magic bytes
        /// </summary>
        /// <param name="bytes">Byte array</param>
        /// <returns>MIME type</returns>
        private static string GetMimeTypeFromMagicBytes(byte[] bytes)
        {
            var extension = GetExtensionFromMagicBytes(bytes);
            return GetMimeTypeFromExtension(extension);
        }

        /// <summary>
        /// Lấy MIME type từ extension
        /// </summary>
        /// <param name="extension">File extension</param>
        /// <returns>MIME type</returns>
        public static string GetMimeTypeFromExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return "application/octet-stream";

            return extension.ToLower() switch
            {
                // Images
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".ico" => "image/x-icon",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                ".tiff" => "image/tiff",

                // Audio
                ".mp3" => "audio/mpeg",
                ".wav" => "audio/wav",
                ".ogg" => "audio/ogg",
                ".flac" => "audio/flac",
                ".aac" => "audio/aac",
                ".m4a" => "audio/mp4",

                // Video
                ".mp4" => "video/mp4",
                ".avi" => "video/x-msvideo",
                ".mkv" => "video/x-matroska",
                ".mov" => "video/quicktime",
                ".wmv" => "video/x-ms-wmv",
                ".flv" => "video/x-flv",
                ".webm" => "video/webm",

                // Documents
                ".pdf" => "application/pdf",
                ".zip" => "application/zip",
                ".rar" => "application/x-rar-compressed",
                ".7z" => "application/x-7z-compressed",
                ".gz" => "application/gzip",
                ".tar" => "application/x-tar",

                // Office
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",

                // Text
                ".txt" => "text/plain",
                ".html" => "text/html",
                ".htm" => "text/html",
                ".xml" => "application/xml",
                ".json" => "application/json",
                ".csv" => "text/csv",
                ".css" => "text/css",
                ".js" => "application/javascript",

                // Other
                _ => "application/octet-stream"
            };
        }

        /// <summary>
        /// Tạo data URL với MIME type tự động xác định
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Data URL với MIME type phù hợp</returns>
        public static string ToDataUrlWithAutoMimeType(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            var mimeType = GetMimeType(base64String);
            return ToDataUrl(base64String, mimeType);
        }

        /// <summary>
        /// Tạo tên file với extension tự động xác định
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <param name="fileName">Tên file (không có extension)</param>
        /// <returns>Tên file với extension</returns>
        public static string GetFileNameWithExtension(string base64String, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = "file";

            var extension = GetFileExtension(base64String);
            return fileName + extension;
        }
    }
} 