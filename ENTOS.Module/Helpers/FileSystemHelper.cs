using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper xử lý file system và file operations với hỗ trợ Dictionary và các tính năng nâng cao.
    /// </summary>
    public static class FileSystemHelper
    {

        public static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            if (!System.IO.Directory.Exists(targetPath))
                System.IO.Directory.CreateDirectory(targetPath);
            foreach (string dirPath in System.IO.Directory.GetDirectories(sourcePath, "*", System.IO.SearchOption.AllDirectories))
            {
                System.IO.Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in System.IO.Directory.GetFiles(sourcePath, "*.*", System.IO.SearchOption.AllDirectories))
            {
                System.IO.File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        #region File Operations

        /// <summary>
        /// Lấy tên file từ đường dẫn.
        /// </summary>
        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Lấy tên file không có extension.
        /// </summary>
        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Lấy phần mở rộng của file.
        /// </summary>
        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }

        /// <summary>
        /// Thay đổi phần mở rộng của file.
        /// </summary>
        public static string ReplaceFileExtension(string fileName, string newExtension)
        {
            return Path.ChangeExtension(fileName, newExtension);
        }

        /// <summary>
        /// Lấy thư mục chứa file.
        /// </summary>
        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Kết hợp đường dẫn.
        /// </summary>
        public static string CombinePath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        /// Lấy đường dẫn tuyệt đối.
        /// </summary>
        public static string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Lấy đường dẫn tương đối.
        /// </summary>
        public static string GetRelativePath(string basePath, string fullPath)
        {
            var baseUri = new Uri(basePath);
            var fullUri = new Uri(fullPath);
            return baseUri.MakeRelativeUri(fullUri).ToString();
        }

        #endregion

        #region File Validation & Checks

        /// <summary>
        /// Kiểm tra file có tồn tại không.
        /// </summary>
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Kiểm tra thư mục có tồn tại không.
        /// </summary>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Kiểm tra file có thể đọc được không.
        /// </summary>
        public static bool IsFileReadable(string path)
        {
            try
            {
                using var stream = File.OpenRead(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra file có thể ghi được không.
        /// </summary>
        public static bool IsFileWritable(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    using var stream = File.OpenWrite(path);
                    return true;
                }
                else
                {
                    using var stream = File.Create(path);
                    File.Delete(path);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Chuẩn hóa tên file bằng cách thay các ký tự không hợp lệ và loại bỏ ký tự Unicode.
        /// </summary>
        /// <param name="fileName">Tên file cần chuẩn hóa.</param>
        /// <returns>Tên file đã được chuẩn hóa: thay ký tự không hợp lệ bằng dấu gạch dưới (_) và chỉ giữ lại các ký tự ASCII.</returns>
        public static string SanitizeFileName(string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            fileName = new string(fileName.Where(c => c <= 127).ToArray()); // Bỏ Unicode, giữ ASCII
            return fileName;
        }

        #endregion

        #region File Information

        /// <summary>
        /// Lấy thông tin file.
        /// </summary>
        public static FileInfo GetFileInfo(string path)
        {
            return new FileInfo(path);
        }

        /// <summary>
        /// Lấy kích thước file (bytes).
        /// </summary>
        public static long GetFileSize(string path)
        {
            return new FileInfo(path).Length;
        }

        /// <summary>
        /// Lấy kích thước file dạng đọc được.
        /// </summary>
        public static string GetFileSizeReadable(string path)
        {
            return FormatFileSize(GetFileSize(path));
        }

        /// <summary>
        /// Format kích thước file.
        /// </summary>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Lấy thời gian tạo file.
        /// </summary>
        public static DateTime GetFileCreationTime(string path)
        {
            return File.GetCreationTime(path);
        }

        /// <summary>
        /// Lấy thời gian sửa đổi cuối.
        /// </summary>
        public static DateTime GetFileLastWriteTime(string path)
        {
            return File.GetLastWriteTime(path);
        }

        /// <summary>
        /// Lấy thời gian truy cập cuối.
        /// </summary>
        public static DateTime GetFileLastAccessTime(string path)
        {
            return File.GetLastAccessTime(path);
        }

        /// <summary>
        /// Lấy MD5 hash của file.
        /// </summary>
        public static string GetFileMd5(string path)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(path);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Lấy SHA256 hash của file.
        /// </summary>
        public static string GetFileSha256(string path)
        {
            using var sha256 = SHA256.Create();
            using var stream = File.OpenRead(path);
            var hash = sha256.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        #endregion

        #region File Content Operations

        /// <summary>
        /// Đọc toàn bộ file text.
        /// </summary>
        public static string ReadAllText(string path, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            return File.ReadAllText(path, encoding);
        }

        /// <summary>
        /// Đọc toàn bộ file text async.
        /// </summary>
        public static async Task<string> ReadAllTextAsync(string path, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            return await File.ReadAllTextAsync(path, encoding);
        }

        /// <summary>
        /// Đọc file text theo dòng.
        /// </summary>
        public static string[] ReadAllLines(string path, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            return File.ReadAllLines(path, encoding);
        }

        /// <summary>
        /// Đọc file text theo dòng async.
        /// </summary>
        public static async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            return await File.ReadAllLinesAsync(path, encoding);
        }

        /// <summary>
        /// Đọc file binary.
        /// </summary>
        public static byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// Đọc file binary async.
        /// </summary>
        public static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            return await File.ReadAllBytesAsync(path);
        }

        /// <summary>
        /// Ghi text vào file. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void WriteAllText(string path, string content, Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            File.WriteAllText(path, content, encoding);
        }

        /// <summary>
        /// Ghi text vào file async. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static async Task WriteAllTextAsync(string path, string content, Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            await File.WriteAllTextAsync(path, content, encoding);
        }

        /// <summary>
        /// Ghi lines vào file. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void WriteAllLines(string path, IEnumerable<string> lines, Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            File.WriteAllLines(path, lines, encoding);
        }

        /// <summary>
        /// Ghi lines vào file async. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static async Task WriteAllLinesAsync(string path, IEnumerable<string> lines, Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            await File.WriteAllLinesAsync(path, lines, encoding);
        }

        /// <summary>
        /// Ghi bytes vào file. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// Ghi bytes vào file async. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static async Task WriteAllBytesAsync(string path, byte[] bytes)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            await File.WriteAllBytesAsync(path, bytes);
        }

        /// <summary>
        /// Append text vào file. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void AppendText(string path, string content, Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            File.AppendAllText(path, content, encoding);
        }

        /// <summary>
        /// Append text vào file async. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static async Task AppendTextAsync(string path, string content, Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            await File.AppendAllTextAsync(path, content, encoding);
        }

        #endregion

        /// <summary>
        /// Thay đổi phần mở rộng của file
        /// </summary>
        /// <param name="inputUrl">Đường dẫn file đầu vào</param>
        /// <param name="newExtension">Phần mở rộng mới</param>
        /// <param name="outputFolder">Thư mục đầu ra</param>
        /// <returns>Đường dẫn file với phần mở rộng mới</returns>
        public static string ReplaceExtension(string inputUrl, string newExtension = ".srt", string outputFolder = null)
        {
            var fileInfo = new System.IO.FileInfo(inputUrl);
            var outputUrl = fileInfo.FullName;
            if (!string.IsNullOrEmpty(outputFolder))
            {
                outputUrl = outputFolder;
                if (!outputUrl.EndsWith("\\"))
                    outputUrl += "\\";
                outputUrl += fileInfo.Name;
            }
            if (!string.IsNullOrEmpty(fileInfo.Extension))
                outputUrl = outputUrl.Replace(fileInfo.Extension, newExtension);
            else
                outputUrl = outputUrl + newExtension;

            return outputUrl;
        }

        /// <summary>
        /// Tạo tên file hợp lệ từ tên file đầu vào
        /// </summary>
        /// <param name="fileName">Tên file đầu vào</param>
        /// <returns>Tên file hợp lệ</returns>
        public static string GetValidFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var invalidChars = System.IO.Path.GetInvalidFileNameChars();
                foreach (var c in invalidChars)
                {
                    fileName = fileName.Replace(c, '_');
                }
            }
            return fileName;
        }

        //Thuật toán chống ghi đè file
        /// <summary>
        /// Tạo tên file duy nhất để tránh ghi đè file hiện có.
        /// Thêm số thứ tự vào tên file nếu file đã tồn tại.
        /// </summary>
        /// <param name="fileName">Tên file gốc</param>
        /// <returns>Tên file duy nhất</returns>
        /// <example>
        /// string uniqueName = Tools.GetUniqueFileName("document.txt");
        /// // Nếu document.txt đã tồn tại, kết quả: "document (1).txt"
        /// // Nếu document (1).txt cũng tồn tại, kết quả: "document (2).txt"
        /// </example>
        public static string GetUniqueFileName(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                return fileName;
            string path = System.IO.Path.GetDirectoryName(fileName);
            string name = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string extension = System.IO.Path.GetExtension(fileName);
            int i = 1;
            while (System.IO.File.Exists(fileName))
            {
                fileName = System.IO.Path.Combine(path, name + " (" + i + ")" + extension);
                i++;
            }
            return fileName;
        }
        #region Dictionary Operations

        /// <summary>
        /// Đọc file CSV thành Dictionary.
        /// </summary>
        public static Dictionary<string, string> ReadCsvToDictionary(string path, char separator = ',', Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var result = new Dictionary<string, string>();

            var lines = File.ReadAllLines(path, encoding);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(separator, 2);
                if (parts.Length >= 2)
                {
                    result[parts[0].Trim()] = parts[1].Trim();
                }
            }

            return result;
        }

        /// <summary>
        /// Ghi Dictionary thành file CSV. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void WriteDictionaryToCsv(Dictionary<string, string> dictionary, string path, char separator = ',', Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            var lines = dictionary.Select(kvp => $"{kvp.Key}{separator}{kvp.Value}");
            File.WriteAllLines(path, lines, encoding);
        }

        /// <summary>
        /// Đọc file JSON thành Dictionary.
        /// </summary>
        public static Dictionary<string, object> ReadJsonToDictionary(string path, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var json = File.ReadAllText(path, encoding);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }

        /// <summary>
        /// Ghi Dictionary thành file JSON. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void WriteDictionaryToJson(Dictionary<string, object> dictionary, string path, Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            var json = JsonSerializer.Serialize(dictionary, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json, encoding);
        }

        /// <summary>
        /// Đọc file INI thành Dictionary.
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> ReadIniToDictionary(string path, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var result = new Dictionary<string, Dictionary<string, string>>();
            var currentSection = "";

            var lines = File.ReadAllLines(path, encoding);
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";")) continue;

                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    if (!result.ContainsKey(currentSection))
                        result[currentSection] = new Dictionary<string, string>();
                }
                else if (!string.IsNullOrEmpty(currentSection) && trimmedLine.Contains("="))
                {
                    var parts = trimmedLine.Split('=', 2);
                    if (parts.Length >= 2)
                    {
                        result[currentSection][parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Ghi Dictionary thành file INI. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void WriteDictionaryToIni(Dictionary<string, Dictionary<string, string>> dictionary, string path, Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            var lines = new List<string>();

            foreach (var section in dictionary)
            {
                lines.Add($"[{section.Key}]");
                foreach (var kvp in section.Value)
                {
                    lines.Add($"{kvp.Key}={kvp.Value}");
                }
                lines.Add("");
            }

            File.WriteAllLines(path, lines, encoding);
        }

        /// <summary>
        /// Đọc file XML thành Dictionary.
        /// </summary>
        public static Dictionary<string, string> ReadXmlToDictionary(string path, string rootElement = "root", Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var result = new Dictionary<string, string>();

            var xml = XDocument.Load(path);
            var root = xml.Element(rootElement);
            if (root != null)
            {
                foreach (var element in root.Elements())
                {
                    result[element.Name.LocalName] = element.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Ghi Dictionary thành file XML. Tự động tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void WriteDictionaryToXml(Dictionary<string, string> dictionary, string path, string rootElement = "root", Encoding encoding = null)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            var xml = new XDocument(
                new XElement(rootElement,
                    dictionary.Select(kvp => new XElement(kvp.Key, kvp.Value))
                )
            );
            xml.Save(path);
        }

        #endregion

        #region File Search & Filter

        /// <summary>
        /// Tìm tất cả file theo pattern.
        /// </summary>
        public static string[] FindFiles(string directory, string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return Directory.GetFiles(directory, searchPattern, searchOption);
        }

        /// <summary>
        /// Tìm tất cả file theo nhiều pattern.
        /// </summary>
        public static string[] FindFiles(string directory, string[] searchPatterns, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var result = new List<string>();
            foreach (var pattern in searchPatterns)
            {
                result.AddRange(Directory.GetFiles(directory, pattern, searchOption));
            }
            return result.Distinct().ToArray();
        }

        /// <summary>
        /// Tìm file theo nội dung.
        /// </summary>
        public static List<string> FindFilesByContent(string directory, string searchText, string[] extensions = null, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var result = new List<string>();

            var files = extensions != null
                ? FindFiles(directory, extensions, SearchOption.AllDirectories)
                : Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    var content = File.ReadAllText(file, encoding);
                    if (content.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        result.Add(file);
                    }
                }
                catch
                {
                    // Bỏ qua file không đọc được
                }
            }

            return result;
        }

        /// <summary>
        /// Tìm file theo regex pattern.
        /// </summary>
        public static List<string> FindFilesByRegex(string directory, string regexPattern, string[] extensions = null, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var result = new List<string>();
            var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);

            var files = extensions != null
                ? FindFiles(directory, extensions, SearchOption.AllDirectories)
                : Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    var content = File.ReadAllText(file, encoding);
                    if (regex.IsMatch(content))
                    {
                        result.Add(file);
                    }
                }
                catch
                {
                    // Bỏ qua file không đọc được
                }
            }

            return result;
        }

        #endregion

        #region File Compression

        /// <summary>
        /// Nén file thành ZIP. Tự động tạo thư mục cho file ZIP nếu chưa tồn tại.
        /// </summary>
        public static void CompressFile(string sourceFile, string zipFile)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(zipFile));
            using var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create);
            archive.CreateEntryFromFile(sourceFile, Path.GetFileName(sourceFile));
        }

        /// <summary>
        /// Nén thư mục thành ZIP. Tự động tạo thư mục cho file ZIP nếu chưa tồn tại.
        /// </summary>
        public static void CompressDirectory(string sourceDirectory, string zipFile)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(zipFile));
            ZipFile.CreateFromDirectory(sourceDirectory, zipFile);
        }

        /// <summary>
        /// Giải nén ZIP file. Tự động tạo thư mục giải nén nếu chưa tồn tại.
        /// </summary>
        public static void ExtractZip(string zipFile, string extractPath)
        {
            DirectoryHelper.EnsureDirectoryExists(extractPath);
            ZipFile.ExtractToDirectory(zipFile, extractPath);
        }

        /// <summary>
        /// Nén nhiều file thành ZIP. Tự động tạo thư mục cho file ZIP nếu chưa tồn tại.
        /// </summary>
        public static void CompressFiles(string[] sourceFiles, string zipFile)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(zipFile));
            using var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create);
            foreach (var file in sourceFiles)
            {
                if (File.Exists(file))
                {
                    archive.CreateEntryFromFile(file, Path.GetFileName(file));
                }
            }
        }

        #endregion

        #region File Backup & Versioning

        /// <summary>
        /// Tạo backup file.
        /// </summary>
        public static string CreateBackup(string filePath, string backupDirectory = null)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("File not found", filePath);

            backupDirectory ??= Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupPath = Path.Combine(backupDirectory, $"{fileName}_{timestamp}{extension}");

            File.Copy(filePath, backupPath);
            return backupPath;
        }

        /// <summary>
        /// Tạo backup với version.
        /// </summary>
        public static string CreateVersionedBackup(string filePath, int maxVersions = 5)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("File not found", filePath);

            var directory = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);

            // Tìm version hiện tại
            var existingBackups = Directory.GetFiles(directory, $"{fileName}_v*{extension}")
                .Select(f => new { Path = f, Version = ExtractVersionNumber(f) })
                .Where(x => x.Version.HasValue)
                .OrderByDescending(x => x.Version)
                .ToList();

            var nextVersion = existingBackups.Any() ? existingBackups.First().Version.Value + 1 : 1;
            var backupPath = Path.Combine(directory, $"{fileName}_v{nextVersion:D3}{extension}");

            File.Copy(filePath, backupPath);

            // Xóa version cũ nếu vượt quá max
            if (existingBackups.Count >= maxVersions)
            {
                var oldestBackup = existingBackups.Last();
                if (File.Exists(oldestBackup.Path))
                    File.Delete(oldestBackup.Path);
            }

            return backupPath;
        }

        private static int? ExtractVersionNumber(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var match = Regex.Match(fileName, @"_v(\d+)$");
            return match.Success ? int.Parse(match.Groups[1].Value) : null;
        }

        #endregion

        #region File Monitoring

        /// <summary>
        /// Theo dõi thay đổi file.
        /// </summary>
        public static FileSystemWatcher WatchFile(string filePath, Action<FileSystemEventArgs> onChange)
        {
            var directory = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileName(filePath);

            var watcher = new FileSystemWatcher(directory, fileName)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime
            };

            watcher.Changed += (sender, e) => onChange(e);
            watcher.Created += (sender, e) => onChange(e);
            watcher.Deleted += (sender, e) => onChange(e);
            watcher.Renamed += (sender, e) => onChange(e);

            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        /// <summary>
        /// Theo dõi thay đổi thư mục.
        /// </summary>
        public static FileSystemWatcher WatchDirectory(string directoryPath, Action<FileSystemEventArgs> onChange, string filter = "*.*")
        {
            var watcher = new FileSystemWatcher(directoryPath, filter)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                IncludeSubdirectories = true
            };

            watcher.Changed += (sender, e) => onChange(e);
            watcher.Created += (sender, e) => onChange(e);
            watcher.Deleted += (sender, e) => onChange(e);
            watcher.Renamed += (sender, e) => onChange(e);

            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        #endregion

        #region File Utilities

        /// <summary>
        /// Tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Xóa file an toàn.
        /// </summary>
        public static bool SafeDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Copy file an toàn.
        /// </summary>
        public static bool SafeCopyFile(string source, string destination, bool overwrite = false)
        {
            try
            {
                var destDir = Path.GetDirectoryName(destination);
                if (!string.IsNullOrEmpty(destDir))
                    EnsureDirectoryExists(destDir);

                File.Copy(source, destination, overwrite);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Move file an toàn.
        /// </summary>
        public static bool SafeMoveFile(string source, string destination)
        {
            try
            {
                var destDir = Path.GetDirectoryName(destination);
                if (!string.IsNullOrEmpty(destDir))
                    EnsureDirectoryExists(destDir);

                File.Move(source, destination);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy đường dẫn tạm thời.
        /// </summary>
        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// Tạo file tạm thời.
        /// </summary>
        public static string CreateTempFile(string extension = ".tmp")
        {
            return Path.GetTempFileName().Replace(".tmp", extension);
        }

        /// <summary>
        /// Làm sạch file tạm thời.
        /// </summary>
        public static void CleanupTempFiles(string pattern = "*", int maxAgeHours = 24)
        {
            var tempPath = Path.GetTempPath();
            var cutoffTime = DateTime.Now.AddHours(-maxAgeHours);

            try
            {
                var files = Directory.GetFiles(tempPath, pattern);
                foreach (var file in files)
                {
                    try
                    {
                        var fileInfo = new FileInfo(file);
                        if (fileInfo.CreationTime < cutoffTime)
                        {
                            File.Delete(file);
                        }
                    }
                    catch
                    {
                        // Bỏ qua file không xóa được
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi cleanup
            }
        }

        #endregion

        #region Large File Support

        /// <summary>
        /// Mở stream đọc file lớn (text).
        /// <para>Ví dụ:
        /// <code>
        /// using (var reader = FileSystemHelper.OpenReadStream("largefile.txt"))
        /// {
        ///     string line;
        ///     while ((line = reader.ReadLine()) != null)
        ///     {
        ///         // Xử lý từng dòng
        ///     }
        /// }
        /// </code>
        /// </para>
        /// </summary>
        public static StreamReader OpenReadStream(string path, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            return new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan), encoding);
        }

        /// <summary>
        /// Mở stream ghi file lớn (text). Tự động tạo thư mục nếu chưa tồn tại.
        /// <para>Ví dụ:
        /// <code>
        /// using (var writer = FileSystemHelper.OpenWriteStream("output.txt"))
        /// {
        ///     foreach (var line in bigDataEnumerable)
        ///         writer.WriteLine(line);
        /// }
        /// </code>
        /// </para>
        /// </summary>
        public static StreamWriter OpenWriteStream(string path, Encoding encoding = null, bool append = false)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            encoding ??= Encoding.UTF8;
            return new StreamWriter(new FileStream(path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096), encoding);
        }

        /// <summary>
        /// Đọc từng dòng từ file lớn (IEnumerable, không load toàn bộ vào RAM).
        /// <para>Ví dụ:
        /// <code>
        /// foreach (var line in FileSystemHelper.ReadLinesStream("bigfile.txt"))
        /// {
        ///     // Xử lý từng dòng
        /// }
        /// </code>
        /// </para>
        /// </summary>
        public static IEnumerable<string> ReadLinesStream(string path, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            using var reader = OpenReadStream(path, encoding);
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        }

        /// <summary>
        /// Ghi từng dòng vào file lớn (không giữ toàn bộ dữ liệu trong RAM).
        /// <para>Ví dụ:
        /// <code>
        /// FileSystemHelper.WriteLinesStream("output.txt", bigDataEnumerable);
        /// </code>
        /// </para>
        /// </summary>
        public static void WriteLinesStream(string path, IEnumerable<string> lines, Encoding encoding = null, bool append = false)
        {
            using var writer = OpenWriteStream(path, encoding, append);
            foreach (var line in lines)
                writer.WriteLine(line);
        }

        /// <summary>
        /// Đọc file lớn theo chunk (async, trả về từng mảng byte nhỏ).
        /// <para>Ví dụ:
        /// <code>
        /// await foreach (var chunk in FileSystemHelper.ReadBytesChunkedAsync("bigfile.bin", 1024 * 1024))
        /// {
        ///     // Xử lý từng chunk (1MB)
        /// }
        /// </code>
        /// </para>
        /// </summary>
        public static async IAsyncEnumerable<byte[]> ReadBytesChunkedAsync(string path, int chunkSize = 1024 * 1024)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, chunkSize, FileOptions.SequentialScan);
            var buffer = new byte[chunkSize];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, chunkSize)) > 0)
            {
                if (bytesRead < chunkSize)
                {
                    var lastChunk = new byte[bytesRead];
                    Array.Copy(buffer, lastChunk, bytesRead);
                    yield return lastChunk;
                }
                else
                {
                    yield return buffer.ToArray();
                }
            }
        }

        /// <summary>
        /// Ghi file lớn theo chunk (async, nhận từng mảng byte nhỏ).
        /// <para>Ví dụ:
        /// <code>
        /// await FileSystemHelper.WriteBytesChunkedAsync("output.bin", chunkedSource);
        /// </code>
        /// </para>
        /// </summary>
        public static async Task WriteBytesChunkedAsync(string path, IAsyncEnumerable<byte[]> chunks)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(path));
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 81920);
            await foreach (var chunk in chunks)
            {
                await stream.WriteAsync(chunk, 0, chunk.Length);
            }
        }

        #endregion

        #region Advanced File Operations

        /// <summary>
        /// So sánh nội dung 2 file lớn (theo từng chunk, không load toàn bộ vào RAM).
        /// </summary>
        public static bool CompareLargeFiles(string file1, string file2, int bufferSize = 1024 * 1024)
        {
            if (new FileInfo(file1).Length != new FileInfo(file2).Length)
                return false;
            using var fs1 = new FileStream(file1, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
            using var fs2 = new FileStream(file2, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];
            int read1, read2;
            do
            {
                read1 = fs1.Read(buffer1, 0, bufferSize);
                read2 = fs2.Read(buffer2, 0, bufferSize);
                if (read1 != read2 || !buffer1.Take(read1).SequenceEqual(buffer2.Take(read2)))
                    return false;
            } while (read1 > 0);
            return true;
        }

        /// <summary>
        /// Chia nhỏ file lớn thành nhiều file nhỏ (theo dung lượng max mỗi file).
        /// </summary>
        public static List<string> SplitFile(string sourceFile, long maxPartSizeBytes)
        {
            var result = new List<string>();
            var buffer = new byte[1024 * 1024]; // 1MB buffer
            int part = 0;
            using var input = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
            while (input.Position < input.Length)
            {
                var partFile = $"{sourceFile}.part{++part:D3}";
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
        /// Ghép nhiều file nhỏ thành một file lớn.
        /// </summary>
        public static void MergeFiles(IEnumerable<string> partFiles, string outputFile)
        {
            DirectoryHelper.EnsureDirectoryExists(Path.GetDirectoryName(outputFile));
            using var output = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
            var buffer = new byte[1024 * 1024];
            foreach (var part in partFiles)
            {
                using var input = new FileStream(part, FileMode.Open, FileAccess.Read);
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, read);
                }
            }
        }

        /// <summary>
        /// Xóa file an toàn: ghi đè dữ liệu ngẫu nhiên trước khi xóa.
        /// </summary>
        public static void SecureDelete(string path, int overwritePasses = 1)
        {
            if (!File.Exists(path)) return;
            var length = new FileInfo(path).Length;
            var buffer = new byte[1024 * 1024];
            var rng = new Random();
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                for (int pass = 0; pass < overwritePasses; pass++)
                {
                    stream.Position = 0;
                    long written = 0;
                    while (written < length)
                    {
                        rng.NextBytes(buffer);
                        int toWrite = (int)Math.Min(buffer.Length, length - written);
                        stream.Write(buffer, 0, toWrite);
                        written += toWrite;
                    }
                }
            }
            File.Delete(path);
        }

        /// <summary>
        /// Tính SHA1 hash cho file lớn.
        /// </summary>
        public static string ComputeSha1(string path)
        {
            using var sha1 = System.Security.Cryptography.SHA1.Create();
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var hash = sha1.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Tính SHA512 hash cho file lớn.
        /// </summary>
        public static string ComputeSha512(string path)
        {
            using var sha512 = System.Security.Cryptography.SHA512.Create();
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var hash = sha512.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Xác định loại file dựa trên magic number (một số định dạng phổ biến).
        /// </summary>
        public static string DetectFileType(string path)
        {
            var signatures = new Dictionary<string, byte[]>
            {
                { "PDF", new byte[] { 0x25, 0x50, 0x44, 0x46 } }, // %PDF
                { "ZIP", new byte[] { 0x50, 0x4B, 0x03, 0x04 } },
                { "PNG", new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
                { "JPG", new byte[] { 0xFF, 0xD8, 0xFF } },
                { "GIF", new byte[] { 0x47, 0x49, 0x46, 0x38 } },
                { "EXE", new byte[] { 0x4D, 0x5A } }, // MZ
                { "RAR", new byte[] { 0x52, 0x61, 0x72, 0x21 } },
                { "7Z",  new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C } },
                { "BMP", new byte[] { 0x42, 0x4D } },
                { "XML", new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C } }, // <?xml
            };
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var maxLen = signatures.Values.Max(s => s.Length);
            var header = new byte[maxLen];
            stream.Read(header, 0, maxLen);
            foreach (var kv in signatures)
            {
                if (header.Take(kv.Value.Length).SequenceEqual(kv.Value))
                    return kv.Key;
            }
            return "Unknown";
        }

        /// <summary>
        /// Ghi log dạng rolling file (tự động tạo file mới khi vượt quá maxSize).
        /// </summary>
        public static void AppendRollingLog(string logDir, string logPrefix, string message, long maxSizeBytes = 10 * 1024 * 1024)
        {
            DirectoryHelper.EnsureDirectoryExists(logDir);
            var date = DateTime.Now.ToString("yyyyMMdd");
            int index = 1;
            string logFile;
            do
            {
                logFile = Path.Combine(logDir, $"{logPrefix}_{date}_{index:D2}.log");
                if (!File.Exists(logFile) || new FileInfo(logFile).Length < maxSizeBytes)
                    break;
                index++;
            } while (true);
            File.AppendAllText(logFile, message + Environment.NewLine);
        }

        /// <summary>
        /// Đọc file lớn theo trang (bắt đầu từ dòng start, lấy tối đa pageSize dòng).
        /// </summary>
        public static List<string> ReadLinesPaged(string path, int startLine, int pageSize, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var result = new List<string>(pageSize);
            using var reader = OpenReadStream(path, encoding);
            int lineNum = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (++lineNum < startLine) continue;
                result.Add(line);
                if (result.Count >= pageSize) break;
            }
            return result;
        }

        /// <summary>
        /// Tạo file tạm với prefix/suffix tùy ý.
        /// </summary>
        public static string CreateTempFileWithPrefixSuffix(string prefix = "temp_", string suffix = ".tmp")
        {
            var tempPath = Path.GetTempPath();
            var fileName = $"{prefix}{Guid.NewGuid():N}{suffix}";
            var fullPath = Path.Combine(tempPath, fileName);
            using (File.Create(fullPath)) { }
            return fullPath;
        }

        #endregion
    }
}