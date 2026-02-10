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
    /// Helper xử lý Directory operations và quản lý thư mục.
    /// </summary>
    public static class DirectoryHelper
    {
        #region Directory Operations

        /// <summary>
        /// Lấy tất cả thư mục con.
        /// </summary>
        public static string[] GetDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return Directory.GetDirectories(path, searchPattern, searchOption);
        }

        /// <summary>
        /// Lấy tất cả file trong thư mục.
        /// </summary>
        public static string[] GetFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }

        /// <summary>
        /// Lấy tất cả file và thư mục.
        /// </summary>
        public static string[] GetFileSystemEntries(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return Directory.GetFileSystemEntries(path, searchPattern, searchOption);
        }

        /// <summary>
        /// Tạo thư mục.
        /// </summary>
        public static DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Xóa thư mục.
        /// </summary>
        public static void DeleteDirectory(string path, bool recursive = false)
        {
            Directory.Delete(path, recursive);
        }

        /// <summary>
        /// Di chuyển thư mục.
        /// </summary>
        public static void MoveDirectory(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        /// <summary>
        /// Lấy thông tin thư mục.
        /// </summary>
        public static DirectoryInfo GetDirectoryInfo(string path)
        {
            return new DirectoryInfo(path);
        }

        /// <summary>
        /// Lấy thời gian tạo thư mục.
        /// </summary>
        public static DateTime GetDirectoryCreationTime(string path)
        {
            return Directory.GetCreationTime(path);
        }

        /// <summary>
        /// Lấy thời gian sửa đổi cuối của thư mục.
        /// </summary>
        public static DateTime GetDirectoryLastWriteTime(string path)
        {
            return Directory.GetLastWriteTime(path);
        }

        /// <summary>
        /// Lấy thời gian truy cập cuối của thư mục.
        /// </summary>
        public static DateTime GetDirectoryLastAccessTime(string path)
        {
            return Directory.GetLastAccessTime(path);
        }

        /// <summary>
        /// Lấy thư mục hiện tại.
        /// </summary>
        public static string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Thay đổi thư mục hiện tại.
        /// </summary>
        public static void SetCurrentDirectory(string path)
        {
            Directory.SetCurrentDirectory(path);
        }

        /// <summary>
        /// Lấy thư mục gốc.
        /// </summary>
        public static string GetDirectoryRoot(string path)
        {
            return Directory.GetDirectoryRoot(path);
        }

        /// <summary>
        /// Lấy thư mục cha.
        /// </summary>
        public static string GetParentDirectory(string path)
        {
            return Directory.GetParent(path)?.FullName;
        }

        /// <summary>
        /// Kiểm tra thư mục có tồn tại không.
        /// </summary>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        #endregion

        #region Directory Analysis

        /// <summary>
        /// Tính tổng kích thước thư mục.
        /// </summary>
        public static long GetDirectorySize(string path, bool includeSubdirectories = true)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return 0;

            var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return directory.GetFiles("*.*", searchOption).Sum(file => file.Length);
        }

        /// <summary>
        /// Tính tổng kích thước thư mục dạng đọc được.
        /// </summary>
        public static string GetDirectorySizeReadable(string path, bool includeSubdirectories = true)
        {
            return FormatFileSize(GetDirectorySize(path, includeSubdirectories));
        }

        /// <summary>
        /// Đếm số file trong thư mục.
        /// </summary>
        public static int GetFileCount(string path, bool includeSubdirectories = true)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return 0;

            var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return directory.GetFiles("*.*", searchOption).Length;
        }

        /// <summary>
        /// Đếm số thư mục con.
        /// </summary>
        public static int GetDirectoryCount(string path, bool includeSubdirectories = true)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return 0;

            var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return directory.GetDirectories("*", searchOption).Length;
        }

        /// <summary>
        /// Lấy thống kê thư mục.
        /// </summary>
        public static DirectoryStatistics GetDirectoryStatistics(string path)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return new DirectoryStatistics();

            var files = directory.GetFiles("*.*", SearchOption.AllDirectories);
            var directories = directory.GetDirectories("*", SearchOption.AllDirectories);

            return new DirectoryStatistics
            {
                TotalFiles = files.Length,
                TotalDirectories = directories.Length,
                TotalSize = files.Sum(f => f.Length),
                OldestFile = files.Any() ? files.Min(f => f.CreationTime) : DateTime.MinValue,
                NewestFile = files.Any() ? files.Max(f => f.CreationTime) : DateTime.MinValue,
                FileExtensions = files.GroupBy(f => f.Extension.ToLowerInvariant())
                                     .ToDictionary(g => g.Key, g => g.Count())
            };
        }

        /// <summary>
        /// Tìm file lớn nhất trong thư mục.
        /// </summary>
        public static FileInfo GetLargestFile(string path, bool includeSubdirectories = true)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return null;

            var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return directory.GetFiles("*.*", searchOption)
                           .OrderByDescending(f => f.Length)
                           .FirstOrDefault();
        }

        /// <summary>
        /// Tìm file cũ nhất trong thư mục.
        /// </summary>
        public static FileInfo GetOldestFile(string path, bool includeSubdirectories = true)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return null;

            var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return directory.GetFiles("*.*", searchOption)
                           .OrderBy(f => f.CreationTime)
                           .FirstOrDefault();
        }

        /// <summary>
        /// Tìm file mới nhất trong thư mục.
        /// </summary>
        public static FileInfo GetNewestFile(string path, bool includeSubdirectories = true)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return null;

            var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return directory.GetFiles("*.*", searchOption)
                           .OrderByDescending(f => f.CreationTime)
                           .FirstOrDefault();
        }

        /// <summary>
        /// Lấy danh sách file theo kích thước.
        /// </summary>
        public static List<FileInfo> GetFilesBySize(string path, bool includeSubdirectories = true, int topCount = 10)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return new List<FileInfo>();

            var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return directory.GetFiles("*.*", searchOption)
                           .OrderByDescending(f => f.Length)
                           .Take(topCount)
                           .ToList();
        }

        /// <summary>
        /// Lấy danh sách file theo thời gian tạo.
        /// </summary>
        public static List<FileInfo> GetFilesByCreationTime(string path, bool includeSubdirectories = true, int topCount = 10)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return new List<FileInfo>();

            var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return directory.GetFiles("*.*", searchOption)
                           .OrderByDescending(f => f.CreationTime)
                           .Take(topCount)
                           .ToList();
        }

        #endregion

        #region Directory Management

        /// <summary>
        /// Copy thư mục.
        /// </summary>
        public static void CopyDirectory(string sourceDir, string destinationDir, bool overwrite = false)
        {
            var source = new DirectoryInfo(sourceDir);
            var destination = new DirectoryInfo(destinationDir);

            if (!source.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

            if (!destination.Exists)
                destination.Create();

            // Copy files
            foreach (var file in source.GetFiles())
            {
                var destFile = Path.Combine(destination.FullName, file.Name);
                file.CopyTo(destFile, overwrite);
            }

            // Copy subdirectories
            foreach (var subDir in source.GetDirectories())
            {
                var destSubDir = Path.Combine(destination.FullName, subDir.Name);
                CopyDirectory(subDir.FullName, destSubDir, overwrite);
            }
        }

        /// <summary>
        /// Copy thư mục an toàn.
        /// </summary>
        public static bool SafeCopyDirectory(string sourceDir, string destinationDir, bool overwrite = false)
        {
            try
            {
                CopyDirectory(sourceDir, destinationDir, overwrite);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa thư mục an toàn.
        /// </summary>
        public static bool SafeDeleteDirectory(string path, bool recursive = false)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, recursive);
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
        /// Tạo thư mục nếu chưa tồn tại.
        /// </summary>
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Tạo cấu trúc thư mục.
        /// </summary>
        public static void CreateDirectoryStructure(params string[] paths)
        {
            foreach (var path in paths)
            {
                EnsureDirectoryExists(path);
            }
        }

        /// <summary>
        /// Làm sạch thư mục (xóa file cũ).
        /// </summary>
        public static void CleanupDirectory(string path, int maxAgeDays = 30, string pattern = "*.*")
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) return;

            var cutoffDate = DateTime.Now.AddDays(-maxAgeDays);
            var files = directory.GetFiles(pattern);

            foreach (var file in files)
            {
                if (file.CreationTime < cutoffDate)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        // Bỏ qua file không xóa được
                    }
                }
            }
        }

        /// <summary>
        /// Làm sạch thư mục tạm thời.
        /// </summary>
        public static void CleanupTempDirectory(int maxAgeHours = 24)
        {
            var tempPath = Path.GetTempPath();
            CleanupDirectory(tempPath, maxAgeHours / 24);
        }

        /// <summary>
        /// Xóa thư mục trống.
        /// </summary>
        public static void RemoveEmptyDirectories(string rootPath)
        {
            var emptyDirs = FindEmptyDirectories(rootPath);
            foreach (var dir in emptyDirs)
            {
                try
                {
                    Directory.Delete(dir);
                }
                catch
                {
                    // Bỏ qua thư mục không xóa được
                }
            }
        }

        #endregion

        #region Directory Search & Filter

        /// <summary>
        /// Tìm thư mục theo tên.
        /// </summary>
        public static List<string> FindDirectoriesByName(string rootPath, string directoryName, bool caseSensitive = false)
        {
            var result = new List<string>();
            var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            try
            {
                var directories = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
                foreach (var dir in directories)
                {
                    var dirName = Path.GetFileName(dir);
                    if (string.Equals(dirName, directoryName, comparison))
                    {
                        result.Add(dir);
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi truy cập
            }

            return result;
        }

        /// <summary>
        /// Tìm thư mục trống.
        /// </summary>
        public static List<string> FindEmptyDirectories(string rootPath)
        {
            var result = new List<string>();

            try
            {
                var directories = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
                foreach (var dir in directories)
                {
                    if (!Directory.GetFiles(dir).Any() && !Directory.GetDirectories(dir).Any())
                    {
                        result.Add(dir);
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi truy cập
            }

            return result;
        }

        /// <summary>
        /// Tìm thư mục lớn nhất.
        /// </summary>
        public static List<DirectoryInfo> FindLargestDirectories(string rootPath, int topCount = 10)
        {
            var directories = new List<DirectoryInfo>();

            try
            {
                var allDirs = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories)
                    .Select(d => new DirectoryInfo(d))
                    .Where(d => d.Exists)
                    .Select(d => new
                    {
                        Directory = d,
                        Size = GetDirectorySize(d.FullName)
                    })
                    .OrderByDescending(x => x.Size)
                    .Take(topCount)
                    .Select(x => x.Directory);

                directories.AddRange(allDirs);
            }
            catch
            {
                // Bỏ qua lỗi truy cập
            }

            return directories;
        }

        /// <summary>
        /// Tìm thư mục theo pattern.
        /// </summary>
        public static List<string> FindDirectoriesByPattern(string rootPath, string pattern, bool caseSensitive = false)
        {
            var result = new List<string>();
            var regex = new Regex(pattern, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);

            try
            {
                var directories = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
                foreach (var dir in directories)
                {
                    var dirName = Path.GetFileName(dir);
                    if (regex.IsMatch(dirName))
                    {
                        result.Add(dir);
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi truy cập
            }

            return result;
        }

        #endregion

        #region Directory Comparison

        /// <summary>
        /// So sánh hai thư mục.
        /// </summary>
        public static DirectoryComparisonResult CompareDirectories(string dir1, string dir2)
        {
            var result = new DirectoryComparisonResult();

            if (!Directory.Exists(dir1) || !Directory.Exists(dir2))
                return result;

            var files1 = GetDirectoryFilesRecursive(dir1);
            var files2 = GetDirectoryFilesRecursive(dir2);

            var relativeFiles1 = files1.Select(f => GetRelativePath(dir1, f)).ToHashSet();
            var relativeFiles2 = files2.Select(f => GetRelativePath(dir2, f)).ToHashSet();

            result.OnlyInFirst = relativeFiles1.Except(relativeFiles2).ToList();
            result.OnlyInSecond = relativeFiles2.Except(relativeFiles1).ToList();
            result.Common = relativeFiles1.Intersect(relativeFiles2).ToList();

            // So sánh nội dung file chung
            foreach (var commonFile in result.Common)
            {
                var file1 = Path.Combine(dir1, commonFile);
                var file2 = Path.Combine(dir2, commonFile);

                if (GetFileMd5(file1) != GetFileMd5(file2))
                {
                    result.Different.Add(commonFile);
                }
                else
                {
                    result.Identical.Add(commonFile);
                }
            }

            return result;
        }

        private static List<string> GetDirectoryFilesRecursive(string path)
        {
            var files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(path, "*", SearchOption.AllDirectories));
            }
            catch
            {
                // Bỏ qua lỗi truy cập
            }
            return files;
        }

        private static string GetRelativePath(string basePath, string fullPath)
        {
            var baseUri = new Uri(basePath);
            var fullUri = new Uri(fullPath);
            return baseUri.MakeRelativeUri(fullUri).ToString();
        }

        private static string GetFileMd5(string path)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(path);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        #endregion

        #region Directory Synchronization

        /// <summary>
        /// Đồng bộ thư mục (copy file mới/changed từ source sang destination).
        /// </summary>
        public static void SynchronizeDirectories(string sourceDir, string destinationDir, bool overwrite = false)
        {
            var source = new DirectoryInfo(sourceDir);
            var destination = new DirectoryInfo(destinationDir);

            if (!source.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

            if (!destination.Exists)
                destination.Create();

            // Đồng bộ files
            foreach (var sourceFile in source.GetFiles("*", SearchOption.AllDirectories))
            {
                var relativePath = GetRelativePath(sourceDir, sourceFile.FullName);
                var destFile = Path.Combine(destinationDir, relativePath);
                var destFileInfo = new FileInfo(destFile);

                if (!destFileInfo.Exists || overwrite || sourceFile.LastWriteTime > destFileInfo.LastWriteTime)
                {
                    var destDir = Path.GetDirectoryName(destFile);
                    if (!string.IsNullOrEmpty(destDir))
                        EnsureDirectoryExists(destDir);

                    sourceFile.CopyTo(destFile, true);
                }
            }
        }

        /// <summary>
        /// Mirror thư mục (đồng bộ hai chiều).
        /// </summary>
        public static void MirrorDirectories(string dir1, string dir2, bool overwrite = false)
        {
            SynchronizeDirectories(dir1, dir2, overwrite);
            SynchronizeDirectories(dir2, dir1, overwrite);
        }

        /// <summary>
        /// Đồng bộ thư mục với progress callback.
        /// </summary>
        public static void SynchronizeDirectoriesWithProgress(string sourceDir, string destinationDir, Action<int, int> progressCallback, bool overwrite = false)
        {
            var source = new DirectoryInfo(sourceDir);
            var destination = new DirectoryInfo(destinationDir);

            if (!source.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

            if (!destination.Exists)
                destination.Create();

            var allFiles = source.GetFiles("*", SearchOption.AllDirectories).ToList();
            var processed = 0;

            foreach (var sourceFile in allFiles)
            {
                var relativePath = GetRelativePath(sourceDir, sourceFile.FullName);
                var destFile = Path.Combine(destinationDir, relativePath);
                var destFileInfo = new FileInfo(destFile);

                if (!destFileInfo.Exists || overwrite || sourceFile.LastWriteTime > destFileInfo.LastWriteTime)
                {
                    var destDir = Path.GetDirectoryName(destFile);
                    if (!string.IsNullOrEmpty(destDir))
                        EnsureDirectoryExists(destDir);

                    sourceFile.CopyTo(destFile, true);
                }

                processed++;
                progressCallback?.Invoke(processed, allFiles.Count);
            }
        }

        #endregion

        #region Directory Backup

        /// <summary>
        /// Tạo backup thư mục.
        /// </summary>
        public static string CreateDirectoryBackup(string sourceDir, string backupDir = null)
        {
            if (!Directory.Exists(sourceDir))
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

            var sourceInfo = new DirectoryInfo(sourceDir);
            backupDir ??= Path.GetDirectoryName(sourceDir);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupPath = Path.Combine(backupDir, $"{sourceInfo.Name}_backup_{timestamp}");

            CopyDirectory(sourceDir, backupPath);
            return backupPath;
        }

        /// <summary>
        /// Tạo backup thư mục với version.
        /// </summary>
        public static string CreateVersionedDirectoryBackup(string sourceDir, int maxVersions = 5)
        {
            if (!Directory.Exists(sourceDir))
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

            var sourceInfo = new DirectoryInfo(sourceDir);
            var backupDir = Path.GetDirectoryName(sourceDir);

            // Tìm version hiện tại
            var existingBackups = Directory.GetDirectories(backupDir, $"{sourceInfo.Name}_v*")
                .Select(d => new { Path = d, Version = ExtractVersionNumber(d) })
                .Where(x => x.Version.HasValue)
                .OrderByDescending(x => x.Version)
                .ToList();

            var nextVersion = existingBackups.Any() ? existingBackups.First().Version.Value + 1 : 1;
            var backupPath = Path.Combine(backupDir, $"{sourceInfo.Name}_v{nextVersion:D3}");

            CopyDirectory(sourceDir, backupPath);

            // Xóa version cũ nếu vượt quá max
            if (existingBackups.Count >= maxVersions)
            {
                var oldestBackup = existingBackups.Last();
                if (Directory.Exists(oldestBackup.Path))
                    SafeDeleteDirectory(oldestBackup.Path, true);
            }

            return backupPath;
        }

        private static int? ExtractVersionNumber(string directoryPath)
        {
            var dirName = Path.GetFileName(directoryPath);
            var match = Regex.Match(dirName, @"_v(\d+)$");
            return match.Success ? int.Parse(match.Groups[1].Value) : null;
        }

        #endregion

        #region Directory Monitoring

        /// <summary>
        /// Theo dõi thay đổi thư mục với filter.
        /// </summary>
        public static FileSystemWatcher WatchDirectoryWithFilter(string directoryPath, Action<FileSystemEventArgs> onChange, string filter = "*.*", bool includeSubdirectories = true)
        {
            var watcher = new FileSystemWatcher(directoryPath, filter)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                IncludeSubdirectories = includeSubdirectories
            };

            watcher.Changed += (sender, e) => onChange(e);
            watcher.Created += (sender, e) => onChange(e);
            watcher.Deleted += (sender, e) => onChange(e);
            watcher.Renamed += (sender, e) => onChange(e);

            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        /// <summary>
        /// Theo dõi thay đổi thư mục với multiple filters.
        /// </summary>
        public static List<FileSystemWatcher> WatchDirectoryWithMultipleFilters(string directoryPath, Action<FileSystemEventArgs> onChange, string[] filters, bool includeSubdirectories = true)
        {
            var watchers = new List<FileSystemWatcher>();

            foreach (var filter in filters)
            {
                var watcher = WatchDirectoryWithFilter(directoryPath, onChange, filter, includeSubdirectories);
                watchers.Add(watcher);
            }

            return watchers;
        }

        #endregion

        #region Directory Utilities

        /// <summary>
        /// Lấy đường dẫn tạm thời.
        /// </summary>
        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// Tạo thư mục tạm thời.
        /// </summary>
        public static string CreateTempDirectory()
        {
            var tempPath = Path.GetTempPath();
            var tempDir = Path.Combine(tempPath, $"temp_{Guid.NewGuid():N}");
            Directory.CreateDirectory(tempDir);
            return tempDir;
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
        /// Lấy đường dẫn tuyệt đối.
        /// </summary>
        public static string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Kết hợp đường dẫn.
        /// </summary>
        public static string CombinePath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        #endregion
    }

    /// <summary>
    /// Thống kê thư mục.
    /// </summary>
    public class DirectoryStatistics
    {
        public int TotalFiles { get; set; }
        public int TotalDirectories { get; set; }
        public long TotalSize { get; set; }
        public DateTime OldestFile { get; set; }
        public DateTime NewestFile { get; set; }
        public Dictionary<string, int> FileExtensions { get; set; } = new Dictionary<string, int>();

        public string TotalSizeReadable => DirectoryHelper.FormatFileSize(TotalSize);
    }

    /// <summary>
    /// Kết quả so sánh thư mục.
    /// </summary>
    public class DirectoryComparisonResult
    {
        public List<string> OnlyInFirst { get; set; } = new List<string>();
        public List<string> OnlyInSecond { get; set; } = new List<string>();
        public List<string> Common { get; set; } = new List<string>();
        public List<string> Identical { get; set; } = new List<string>();
        public List<string> Different { get; set; } = new List<string>();
    }
} 