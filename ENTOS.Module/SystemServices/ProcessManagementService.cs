using ENTOS.Module.BusinessObjects;
using System.Diagnostics;

namespace ENTOS.Module.SystemServices
{

    public abstract class ProcessManagementService : IProcessManagementService
    {

        /// <summary>
        /// Mở file bằng ứng dụng mặc định của hệ điều hành
        /// </summary>
        public bool OpenFile(string path)
           => OpenFile(path, null);
        /// <summary>
        /// Mở file bằng ứng dụng mặc định của hệ điều kèm tham số
        /// </summary>
        public bool OpenFile(string path, string arguments)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            if (!File.Exists(path) && !Directory.Exists(path))
                return false;

            try
            {
                if (System.OperatingSystem.IsWindows())
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = arguments ?? string.Empty,
                        UseShellExecute = true
                    });
                }
                else if (System.OperatingSystem.IsLinux())
                {
                    Process.Start("xdg-open", BuildArgs(path, arguments));
                }
                else if (System.OperatingSystem.IsMacOS())
                {
                    Process.Start("open", BuildArgs(path, arguments));
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string BuildArgs(string path, string? args)
        {
            if (string.IsNullOrWhiteSpace(args))
                return $"\"{path}\"";

            return $"\"{path}\" {args}";
        }

        /// <summary>
        /// Mở thư mục
        /// </summary>
        public bool OpenFolder(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            try
            {
                if (System.OperatingSystem.IsWindows())
                {
                    Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                }
                else if (System.OperatingSystem.IsLinux())
                {
                    Process.Start("xdg-open", Path.GetDirectoryName(filePath)!);
                }
                else if (System.OperatingSystem.IsMacOS())
                {
                    Process.Start("open", "-R \"" + filePath + "\"");
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public abstract bool RunCommandFromOtherComputer(string command, string server, string username, string password);
    }
}
