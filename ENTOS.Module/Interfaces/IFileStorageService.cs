using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface dịch vụ lưu trữ file (local hoặc cloud). Hỗ trợ lưu, tải, xóa, kiểm tra tồn tại, lấy metadata, liệt kê, copy/move, đọc/ghi text/byte[], tạo thư mục, xoá thư mục, tạo link truy cập, download từ url.
    /// Dễ mở rộng cho các backend như: ổ đĩa, S3, Azure Blob, Google Cloud Storage...
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Lưu file từ stream vào kho lưu trữ.
        /// </summary>
        /// <param name="path">Đường dẫn lưu (key)</param>
        /// <param name="stream">Stream dữ liệu</param>
        /// <param name="overwrite">Ghi đè nếu đã tồn tại</param>
        Task SaveAsync(string path, Stream stream, bool overwrite = true);

        /// <summary>
        /// Tải file về dạng stream.
        /// </summary>
        /// <param name="path">Đường dẫn file (key)</param>
        /// <returns>Stream dữ liệu file</returns>
        Task<Stream> GetAsync(string path);

        /// <summary>
        /// Xóa file khỏi kho lưu trữ.
        /// </summary>
        /// <param name="path">Đường dẫn file (key)</param>
        Task DeleteAsync(string path);

        /// <summary>
        /// Kiểm tra file có tồn tại không.
        /// </summary>
        /// <param name="path">Đường dẫn file (key)</param>
        /// <returns>true nếu tồn tại</returns>
        Task<bool> ExistsAsync(string path);

        /// <summary>
        /// Lấy metadata file (kích thước, ngày tạo, v.v.).
        /// </summary>
        /// <param name="path">Đường dẫn file (key)</param>
        /// <returns>Dictionary metadata</returns>
        Task<IDictionary<string, object>> GetMetadataAsync(string path);

        /// <summary>
        /// Liệt kê file trong thư mục.
        /// </summary>
        Task<IEnumerable<string>> ListFilesAsync(string folder, string searchPattern = "*", bool recursive = false);

        /// <summary>
        /// Liệt kê thư mục con trong thư mục.
        /// </summary>
        Task<IEnumerable<string>> ListDirectoriesAsync(string folder, bool recursive = false);

        /// <summary>
        /// Tạo thư mục mới.
        /// </summary>
        Task CreateDirectoryAsync(string folder);

        /// <summary>
        /// Copy file trong kho lưu trữ.
        /// </summary>
        Task CopyAsync(string sourcePath, string destPath, bool overwrite = true);

        /// <summary>
        /// Di chuyển file trong kho lưu trữ.
        /// </summary>
        Task MoveAsync(string sourcePath, string destPath, bool overwrite = true);

        /// <summary>
        /// Đọc toàn bộ file thành mảng byte.
        /// </summary>
        Task<byte[]> ReadAllBytesAsync(string path);

        /// <summary>
        /// Đọc toàn bộ file thành string.
        /// </summary>
        Task<string> ReadAllTextAsync(string path, Encoding encoding = null);

        /// <summary>
        /// Lưu file từ mảng byte.
        /// </summary>
        Task SaveAsync(string path, byte[] data, bool overwrite = true);

        /// <summary>
        /// Lưu file từ string.
        /// </summary>
        Task SaveAsync(string path, string text, Encoding encoding = null, bool overwrite = true);

        /// <summary>
        /// Xoá thư mục (có thể xoá đệ quy).
        /// </summary>
        Task DeleteDirectoryAsync(string folder, bool recursive = true);

        /// <summary>
        /// Lấy link truy cập file (local: trả về path, cloud: trả về url).
        /// </summary>
        Task<string> GetFileUrlAsync(string path);

        /// <summary>
        /// Download file từ url về kho lưu trữ.
        /// </summary>
        Task DownloadFromUrlAsync(string url, string destPath, bool overwrite = true);

        /// <summary>
        /// Lưu file vào đường dẫn nhiều cấp, tự động tạo thư mục nếu chưa có.
        /// Có thể truyền các phần folder (objectType, objectId, subFolder, fileName), phần nào rỗng sẽ bỏ qua.
        /// Có thể thêm tuỳ chọn quản lý theo năm/tháng.
        /// </summary>
        /// <param name="fileName">Tên file</param>
        /// <param name="stream">Stream dữ liệu</param>
        /// <param name="objectType">Loại đối tượng (ví dụ: video, audio, ...)</param>
        /// <param name="objectId">ID đối tượng (có thể rỗng)</param>
        /// <param name="subFolder">Thư mục con (có thể rỗng)</param>
        /// <param name="useYearMonth">Nếu true sẽ tự động thêm thư mục năm/tháng</param>
        /// <param name="overwrite">Ghi đè nếu đã tồn tại</param>
        Task SaveObjectAsync(string fileName, Stream stream, string objectType = null, string objectId = null, string subFolder = null, bool useYearMonth = false, bool overwrite = true);
    }
} 