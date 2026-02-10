using System;
using System.IO;
using System.Linq;
using System.Text;
namespace ENTOS.Module.Helpers;


/// <summary>
/// FileFormatHelper là một lớp tĩnh dùng để nhận diện định dạng tệp (đuôi tệp, extension)
/// dựa trên nội dung thực tế (magic bytes) của tệp.
/// </summary>
public static class FileFormatHelper
{
    /// <summary>
    /// Nhận diện đuôi tệp từ mảng byte.
    /// Trả về null nếu không thể xác định.
    /// </summary>
    public static string? DetectExtension(byte[] data)
    {
        if (data == null || data.Length < 4)
            return null;

        string ascii = Encoding.ASCII.GetString(data, 0, Math.Min(data.Length, 12));

        // ÂM THANH
        if (ascii.StartsWith("RIFF") && Encoding.ASCII.GetString(data, 8, 4) == "WAVE") return ".wav";
        if (data[0] == 0x49 && data[1] == 0x44 && data[2] == 0x33) return ".mp3"; // ID3
        if (data[0] == 0xFF && (data[1] & 0xE0) == 0xE0) return ".mp3";           // MPEG
        if (ascii.StartsWith("OggS")) return ".ogg";
        if (ascii.StartsWith("fLaC")) return ".flac";
        if (data[0] == 0xFF && (data[1] & 0xF6) == 0xF0) return ".aac";

        // HÌNH ẢNH
        if (data[0] == 0x89 && ascii.Contains("PNG")) return ".png";
        if (data[0] == 0xFF && data[1] == 0xD8) return ".jpg";
        if (ascii.StartsWith("GIF8")) return ".gif";
        if (ascii.StartsWith("BM")) return ".bmp";
        if (ascii.StartsWith("RIFF") && Encoding.ASCII.GetString(data, 8, 4) == "WEBP") return ".webp";
        if (data[0] == 0x00 && data[1] == 0x00 && data[2] == 0x01 && data[3] == 0x00) return ".ico";

        // VIDEO
        if (data.Length > 11 && data[4] == 0x66 && data[5] == 0x74 && data[6] == 0x79 && data[7] == 0x70) return ".mp4";
        if (ascii.StartsWith("RIFF") && Encoding.ASCII.GetString(data, 8, 4) == "AVI ") return ".avi";
        if (ascii.Contains("ftypwebm")) return ".webm";

        // TÀI LIỆU
        if (ascii.StartsWith("%PDF")) return ".pdf";
        if (ascii.StartsWith("{\\rtf")) return ".rtf";

        // NÉN & OFFICE
        if (data[0] == 0x50 && data[1] == 0x4B) return ".zip"; // zip, docx, xlsx, pptx
        if (data[0] == 0x52 && data[1] == 0x61 && data[2] == 0x72 && data[3] == 0x21) return ".rar";
        if (data[0] == 0x1F && data[1] == 0x8B) return ".gz";
        if (data[0] == 0x37 && data[1] == 0x7A && data[2] == 0xBC && data[3] == 0xAF) return ".7z";

        return null;
    }

    /// <summary>
    /// Nhận diện đuôi tệp từ stream.
    /// Trả về null nếu không xác định được hoặc stream không hợp lệ.
    /// </summary>
    public static string? DetectExtensionFromStream(Stream stream)
    {
        if (stream == null || !stream.CanRead)
            return null;

        byte[] header = new byte[32];
        stream.Read(header, 0, header.Length);
        stream.Seek(0, SeekOrigin.Begin); // Đặt lại stream về vị trí đầu
        return DetectExtension(header);
    }

    /// <summary>
    /// Nhận diện đuôi tệp từ đường dẫn tệp.
    /// Trả về null nếu file không tồn tại hoặc không thể đọc.
    /// </summary>
    public static string? DetectExtensionFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return DetectExtensionFromStream(fs);
    }

    public static string GetMimeType(string filePathOrExtension)
    {
        // Nếu truyền đường dẫn, lấy phần mở rộng
        string ext = Path.GetExtension(filePathOrExtension)?.ToLowerInvariant();

        return ext switch
        {
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
            ".mov" => "video/quicktime",
            ".wmv" => "video/x-ms-wmv",
            ".webm" => "video/webm",
            ".mkv" => "video/x-matroska",

            // Image
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            ".tiff" => "image/tiff",
            ".ico" => "image/vnd.microsoft.icon",

            // Text
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            ".html" or ".htm" => "text/html",
            ".css" => "text/css",
            ".js" => "text/javascript",
            ".json" => "application/json",
            ".xml" => "application/xml",

            // Document
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",

            // Archive
            ".zip" => "application/zip",
            ".rar" => "application/vnd.rar",
            ".7z" => "application/x-7z-compressed",
            ".tar" => "application/x-tar",
            ".gz" => "application/gzip",

            // Code
            ".cs" => "text/plain",
            ".java" => "text/plain",
            ".py" => "text/plain",
            ".cpp" => "text/plain",
            ".c" => "text/plain",
            ".jsonld" => "application/ld+json",

            // Default
            _ => "application/octet-stream"
        };
    }
}
