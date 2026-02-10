using ENTOS.Module.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Security;

namespace ENTOS.Module.SystemServices
{
    /// <summary>
    /// Dịch vụ lưu trữ file trên ổ đĩa local. Hỗ trợ lưu, tải, xóa, kiểm tra tồn tại, lấy metadata.
    /// Dễ mở rộng cho cloud (S3, Azure Blob, ...).
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly string _normalizedBasePath;

        /// <summary>
        /// Khởi tạo dịch vụ lưu trữ file với thư mục gốc.
        /// </summary>
        /// <param name="basePath">Thư mục gốc lưu trữ</param>
        public FileStorageService(string basePath)
        {
            _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
            
            // Chuẩn hóa đường dẫn gốc để so sánh an toàn
            _normalizedBasePath = Path.GetFullPath(_basePath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        /// <summary>
        /// Lưu file từ stream vào kho lưu trữ.
        /// </summary>
        public async Task SaveAsync(string path, Stream stream, bool overwrite = true)
        {
            var fullPath = GetFullPath(path);
            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (File.Exists(fullPath) && !overwrite)
                throw new IOException($"File đã tồn tại: {path}");
            using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fs);
        }

        /// <summary>
        /// Tải file về dạng stream.
        /// </summary>
        public async Task<Stream> GetAsync(string path)
        {
            var fullPath = GetFullPath(path);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file: {path}");
            var ms = new MemoryStream();
            using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                await fs.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Xóa file khỏi kho lưu trữ.
        /// </summary>
        public Task DeleteAsync(string path)
        {
            var fullPath = GetFullPath(path);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Kiểm tra file có tồn tại không.
        /// </summary>
        public Task<bool> ExistsAsync(string path)
        {
            var fullPath = GetFullPath(path);
            return Task.FromResult(File.Exists(fullPath));
        }

        /// <summary>
        /// Lấy metadata file (kích thước, ngày tạo, v.v.).
        /// </summary>
        public Task<IDictionary<string, object>> GetMetadataAsync(string path)
        {
            var fullPath = GetFullPath(path);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file: {path}");
            var info = new FileInfo(fullPath);
            var dict = new Dictionary<string, object>
            {
                ["FullPath"] = info.FullName,
                ["Length"] = info.Length,
                ["Created"] = info.CreationTimeUtc,
                ["Modified"] = info.LastWriteTimeUtc,
                ["Extension"] = info.Extension
            };
            return Task.FromResult((IDictionary<string, object>)dict);
        }

        /// <summary>
        /// Lấy đường dẫn đầy đủ từ key và kiểm tra bảo mật.
        /// </summary>
        private string GetFullPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            
            // Loại bỏ các ký tự path traversal nguy hiểm
            var cleanPath = path.Replace('/', Path.DirectorySeparatorChar)
                               .Replace('\\', Path.DirectorySeparatorChar);
            
            // Chuẩn hóa đường dẫn
            var fullPath = Path.GetFullPath(Path.Combine(_basePath, cleanPath));
            var normalizedFullPath = fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            
            // Kiểm tra xem đường dẫn có nằm trong thư mục gốc không
            if (!normalizedFullPath.StartsWith(_normalizedBasePath, StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityException($"Truy cập bị từ chối: Đường dẫn '{path}' nằm ngoài thư mục gốc '{_basePath}'");
            }
            
            return fullPath;
        }

        /// <summary>
        /// Liệt kê file trong thư mục.
        /// </summary>
        public Task<IEnumerable<string>> ListFilesAsync(string folder, string searchPattern = "*", bool recursive = false)
        {
            var fullPath = GetFullPath(folder);
            if (!Directory.Exists(fullPath))
                return Task.FromResult<IEnumerable<string>>(new List<string>());
            var files = Directory.GetFiles(fullPath, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            return Task.FromResult<IEnumerable<string>>(files);
        }

        /// <summary>
        /// Liệt kê thư mục con trong thư mục.
        /// </summary>
        public Task<IEnumerable<string>> ListDirectoriesAsync(string folder, bool recursive = false)
        {
            var fullPath = GetFullPath(folder);
            if (!Directory.Exists(fullPath))
                return Task.FromResult<IEnumerable<string>>(new List<string>());
            var dirs = Directory.GetDirectories(fullPath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            return Task.FromResult<IEnumerable<string>>(dirs);
        }

        /// <summary>
        /// Tạo thư mục mới.
        /// </summary>
        public Task CreateDirectoryAsync(string folder)
        {
            var fullPath = GetFullPath(folder);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Copy file trong kho lưu trữ.
        /// </summary>
        public Task CopyAsync(string sourcePath, string destPath, bool overwrite = true)
        {
            var src = GetFullPath(sourcePath);
            var dst = GetFullPath(destPath);
            
            if (!File.Exists(src))
                throw new FileNotFoundException($"Không tìm thấy file nguồn: {sourcePath}");
                
            var dir = Path.GetDirectoryName(dst);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.Copy(src, dst, overwrite);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Di chuyển file trong kho lưu trữ.
        /// </summary>
        public Task MoveAsync(string sourcePath, string destPath, bool overwrite = true)
        {
            var src = GetFullPath(sourcePath);
            var dst = GetFullPath(destPath);
            
            if (!File.Exists(src))
                throw new FileNotFoundException($"Không tìm thấy file nguồn: {sourcePath}");
                
            var dir = Path.GetDirectoryName(dst);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (File.Exists(dst) && overwrite)
                File.Delete(dst);
            File.Move(src, dst);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Đọc toàn bộ file thành mảng byte.
        /// </summary>
        public async Task<byte[]> ReadAllBytesAsync(string path)
        {
            var fullPath = GetFullPath(path);
            return await File.ReadAllBytesAsync(fullPath);
        }

        /// <summary>
        /// Đọc toàn bộ file thành string.
        /// </summary>
        public async Task<string> ReadAllTextAsync(string path, Encoding encoding = null)
        {
            var fullPath = GetFullPath(path);
            encoding ??= Encoding.UTF8;
            return await File.ReadAllTextAsync(fullPath, encoding);
        }

        /// <summary>
        /// Lưu file từ mảng byte.
        /// </summary>
        public async Task SaveAsync(string path, byte[] data, bool overwrite = true)
        {
            var fullPath = GetFullPath(path);
            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (File.Exists(fullPath) && !overwrite)
                throw new IOException($"File đã tồn tại: {path}");
            await File.WriteAllBytesAsync(fullPath, data);
        }

        /// <summary>
        /// Lưu file từ string.
        /// </summary>
        public async Task SaveAsync(string path, string text, Encoding encoding = null, bool overwrite = true)
        {
            var fullPath = GetFullPath(path);
            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (File.Exists(fullPath) && !overwrite)
                throw new IOException($"File đã tồn tại: {path}");
            encoding ??= Encoding.UTF8;
            await File.WriteAllTextAsync(fullPath, text, encoding);
        }

        /// <summary>
        /// Xoá thư mục (có thể xoá đệ quy).
        /// </summary>
        public Task DeleteDirectoryAsync(string folder, bool recursive = true)
        {
            var fullPath = GetFullPath(folder);
            // Không cho phép xóa thư mục gốc
            if (string.Equals(fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                              _normalizedBasePath, StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityException("Không được phép xóa thư mục gốc lưu trữ!");
            }
            if (Directory.Exists(fullPath))
                Directory.Delete(fullPath, recursive);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Lấy link truy cập file (local: trả về path tuyệt đối).
        /// </summary>
        public Task<string> GetFileUrlAsync(string path)
        {
            var fullPath = GetFullPath(path);
            return Task.FromResult(fullPath);
        }

        /// <summary>
        /// Download file từ url về kho lưu trữ.
        /// </summary>
        public async Task DownloadFromUrlAsync(string url, string destPath, bool overwrite = true)
        {
            var fullPath = GetFullPath(destPath);
            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (File.Exists(fullPath) && !overwrite)
                throw new IOException($"File đã tồn tại: {fullPath}");
            using var httpClient = new HttpClient();
            var bytes = await httpClient.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(fullPath, bytes);
        }

        /// <summary>
        /// Sinh path lưu trữ theo đối tượng: ObjectType/ObjectId/SubFolder/FileName. Nếu objectId null sẽ bỏ qua.
        /// </summary>
        /// <param name="objectType">Loại đối tượng (ví dụ: User, Document, Project)</param>
        /// <param name="objectId">ID đối tượng (có thể null)</param>
        /// <param name="subFolder">Thư mục con (có thể null)</param>
        /// <param name="fileName">Tên file</param>
        /// <returns>Chuỗi path chuẩn</returns>
        public static string BuildObjectPath(string objectType, object objectId, string subFolder, string fileName)
        {
            if (string.IsNullOrWhiteSpace(objectType))
                throw new ArgumentNullException(nameof(objectType));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));
            var parts = new List<string> { objectType.Trim() };
            if (objectId != null && !string.IsNullOrWhiteSpace(objectId.ToString()))
                parts.Add(objectId.ToString().Trim());
            if (!string.IsNullOrWhiteSpace(subFolder))
                parts.Add(subFolder.Trim());
            parts.Add(fileName.Trim());
            return string.Join(Path.DirectorySeparatorChar, parts);
        }

        /// <summary>
        /// Sinh path thư mục đối tượng: ObjectType/ObjectId/SubFolder. Nếu objectId null sẽ bỏ qua.
        /// </summary>
        public static string BuildObjectFolder(string objectType, object objectId, string subFolder = null)
        {
            if (string.IsNullOrWhiteSpace(objectType))
                throw new ArgumentNullException(nameof(objectType));
            var parts = new List<string> { objectType.Trim() };
            if (objectId != null && !string.IsNullOrWhiteSpace(objectId.ToString()))
                parts.Add(objectId.ToString().Trim());
            if (!string.IsNullOrWhiteSpace(subFolder))
                parts.Add(subFolder.Trim());
            return string.Join(Path.DirectorySeparatorChar, parts);
        }

        /// <summary>
        /// Sinh path thư mục cache: Cache/cacheName/SubFolder
        /// </summary>
        /// <param name="cacheName">Tên cache (bắt buộc)</param>
        /// <param name="subFolder">Thư mục con (có thể null)</param>
        /// <returns>Chuỗi path cache chuẩn</returns>
        public static string BuildCacheFolder(string cacheName, string subFolder = null)
        {
            if (string.IsNullOrWhiteSpace(cacheName))
                throw new ArgumentNullException(nameof(cacheName));
            var parts = new List<string> { "Cache", cacheName.Trim() };
            if (!string.IsNullOrWhiteSpace(subFolder))
                parts.Add(subFolder.Trim());
            return string.Join(Path.DirectorySeparatorChar, parts);
        }

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
        public async Task SaveObjectAsync(string fileName, Stream stream, string objectType = null, string objectId = null, string subFolder = null, bool useYearMonth = false, bool overwrite = true)
        {
            var now = DateTime.UtcNow;
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(objectType)) parts.Add(objectType);
            if (!string.IsNullOrWhiteSpace(objectId)) parts.Add(objectId);
            if (!string.IsNullOrWhiteSpace(subFolder)) parts.Add(subFolder);
            if (useYearMonth)
            {
                parts.Add(now.Year.ToString());
                parts.Add(now.Month.ToString("D2"));
            }
            var folderPath = string.Join(Path.DirectorySeparatorChar, parts);
            var fullFolder = Path.Combine(_basePath, folderPath);
            if (!Directory.Exists(fullFolder))
                Directory.CreateDirectory(fullFolder);
            var fullPath = Path.Combine(fullFolder, fileName);
            using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fs);
        }
    }
} 