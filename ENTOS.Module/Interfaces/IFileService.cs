namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface xử lý các thao tác với file: đọc, ghi, tải lên/xuống, đổi tên, đổi đuôi file.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Tải file từ đường dẫn URL.
        /// </summary>
        Task<byte[]> DownloadFileAsync(string url);
        /// <summary>
        /// Tải file lên server qua URL.
        /// </summary>
        Task UploadFileAsync(string url, byte[] data);
        /// <summary>
        /// Ghi dữ liệu vào file.
        /// </summary>
        Task WriteFileAsync(string path, byte[] data);
        /// <summary>
        /// Đọc dữ liệu từ file.
        /// </summary>
        Task<byte[]> ReadFileAsync(string path);
        /// <summary>
        /// Đọc nội dung text từ file.
        /// </summary>
        Task<string> ReadTextFileAsync(string path);
        /// <summary>
        /// Ghi nội dung text vào file.
        /// </summary>
        Task WriteTextFileAsync(string path, string content);
        /// <summary>
        /// Lấy tên file từ đường dẫn.
        /// </summary>
        string GetFileName(string path);
        /// <summary>
        /// Thay đổi phần mở rộng của file.
        /// </summary>
        string ReplaceFileExtension(string fileName, string newExtension);
    }
} 