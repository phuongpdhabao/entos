using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper xử lý file PDF cơ bản (chỉ dùng .NET chuẩn, không thư viện ngoài).
    /// </summary>
    public static class PDFHelper
    {
        /// <summary>
        /// Kiểm tra file có phải PDF không (dựa vào header).
        /// </summary>
        public static bool IsPdf(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            var buffer = new byte[4];
            fs.Read(buffer, 0, 4);
            return buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46; // %PDF
        }

        /// <summary>
        /// Đếm số trang PDF (dựa vào marker /Count hoặc /Type /Page).
        /// </summary>
        public static int CountPages(string path)
        {
            var text = ReadPdfRawText(path);
            // Cách 1: Đếm /Type /Page
            var matches = Regex.Matches(text, @"/Type\s*/Page[^s]");
            if (matches.Count > 0)
                return matches.Count;
            // Cách 2: Đếm /Count
            var countMatch = Regex.Match(text, @"/Count\s+(\d+)");
            if (countMatch.Success && int.TryParse(countMatch.Groups[1].Value, out int count))
                return count;
            return 0;
        }

        /// <summary>
        /// Trích xuất text đơn giản từ PDF (chỉ PDF không mã hóa, không nén, không phức tạp).
        /// </summary>
        public static string ExtractText(string path)
        {
            var text = ReadPdfRawText(path);
            var sb = new StringBuilder();
            // Tìm các đoạn text giữa ( ... )
            var matches = Regex.Matches(text, @"\(([^)]*)\)");
            foreach (Match m in matches)
            {
                sb.AppendLine(m.Groups[1].Value);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Kiểm tra PDF có bị mã hóa không (tìm marker /Encrypt).
        /// </summary>
        public static bool IsEncrypted(string path)
        {
            var text = ReadPdfRawText(path);
            return text.Contains("/Encrypt");
        }

        /// <summary>
        /// Tách file PDF thành các phần nhỏ (theo số byte, không phân trang logic).
        /// </summary>
        public static List<string> SplitPdfBySize(string path, long maxPartSizeBytes)
        {
            var result = new List<string>();
            var buffer = new byte[1024 * 1024];
            int part = 0;
            using var input = new FileStream(path, FileMode.Open, FileAccess.Read);
            while (input.Position < input.Length)
            {
                var partFile = $"{path}.part{++part:D3}.pdf";
                using var output = new FileStream(partFile, FileMode.Create, FileAccess.Write);
                long written = 0;
                while (written < maxPartSizeBytes)
                {
                    int toRead = (int)Math.Min(buffer.Length, maxPartSizeBytes - written);
                    int read = input.Read(buffer, 0, toRead);
                    if (read == 0) break;
                    output.Write(buffer, 0, read);
                    written += read;
                }
                result.Add(partFile);
            }
            return result;
        }

        /// <summary>
        /// Đọc toàn bộ nội dung PDF dạng text thô (không giải mã, không giải nén).
        /// </summary>
        private static string ReadPdfRawText(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(fs, Encoding.ASCII, true);
            return reader.ReadToEnd();
        }
    }
} 