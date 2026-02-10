using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Threading;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper chạy ứng dụng/process bên ngoài với hỗ trợ tham số dài và xử lý lỗi.
    /// </summary>
    public static class ProcessHelper
    {

        public static void RunProcessOutside(string processName, string arguments, int waitSecond = 0, bool displayWindow = false)
        {
            var file = "RunProcessOutside" + System.DateTime.Now.ToString("yyyyMMddHH") + System.Guid.NewGuid().ToString() + ".bat";
            System.IO.File.WriteAllText(file, processName + " " + arguments); //Ghi tham số ra text để tránh bị lỗi quá dài
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            //process.StartInfo.FileName = processName
            process.StartInfo.FileName = file;
            process.EnableRaisingEvents = false;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = displayWindow;
            process.StartInfo.CreateNoWindow = !displayWindow;

            //process.StartInfo.Arguments = arguments;
            int totalSeconds = 0;
            if (process.Start())
            {
                while (!process.HasExited)
                {
                    if (waitSecond > 0)
                    {
                        totalSeconds++;
                        if (totalSeconds > waitSecond)
                            break;
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            }

            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);//Xóa file tạm
        }
        /// <summary>
        /// Kết quả chạy process.
        /// </summary>
        public class ProcessResult
        {
            public int ExitCode { get; set; }
            public string Output { get; set; }
            public string Error { get; set; }
            public bool Success => ExitCode == 0;
            public TimeSpan ExecutionTime { get; set; }
        }

        /// <summary>
        /// Tùy chọn chạy process.
        /// </summary>
        public class ProcessOptions
        {
            public bool RedirectOutput { get; set; } = false;
            public bool RedirectError { get; set; } = false;
            public bool UseShellExecute { get; set; } = true;
            public bool CreateNoWindow { get; set; } = true;
            public string WorkingDirectory { get; set; } = null;
            public int TimeoutSeconds { get; set; } = 300; // 5 phút
            public bool ShowErrorOnFailure { get; set; } = true;
            public Encoding OutputEncoding { get; set; } = Encoding.UTF8;
        }

        /// <summary>
        /// Chạy process ngoài với tham số (không chờ kết thúc).
        /// </summary>
        public static void RunProcess(string fileName, string arguments = "")
        {
            try
            {
                Process.Start(fileName, arguments);
            }
            catch (Exception ex)
            {
                // LogHelper.LogToConsole($"Lỗi chạy process: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Chạy process ngoài với tham số dài (không chờ kết thúc).
        /// </summary>
        public static void RunProcess(string fileName, string[] arguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    UseShellExecute = true,
                    CreateNoWindow = true
                };

                if (arguments != null && arguments.Length > 0)
                {
                    startInfo.Arguments = string.Join(" ", arguments);
                }

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                // LogHelper.LogToConsole($"Lỗi chạy process với tham số dài: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Chạy process ngoài với tham số (chờ kết thúc, trả về exit code).
        /// </summary>
        public static async Task<int> RunProcessAsync(string fileName, string arguments = "")
        {
            var result = await RunProcessWithResultAsync(fileName, arguments);
            return result.ExitCode;
        }

        /// <summary>
        /// Chạy process ngoài với tham số dài (chờ kết thúc, trả về exit code).
        /// </summary>
        public static async Task<int> RunProcessAsync(string fileName, string[] arguments)
        {
            var result = await RunProcessWithResultAsync(fileName, arguments);
            return result.ExitCode;
        }

        /// <summary>
        /// Chạy process với kết quả chi tiết.
        /// </summary>
        public static async Task<ProcessResult> RunProcessWithResultAsync(string fileName, string arguments = "", ProcessOptions options = null)
        {
            options ??= new ProcessOptions();
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = options.RedirectOutput,
                    RedirectStandardError = options.RedirectError,
                    UseShellExecute = options.UseShellExecute,
                    CreateNoWindow = options.CreateNoWindow,
                    StandardOutputEncoding = options.OutputEncoding,
                    StandardErrorEncoding = options.OutputEncoding
                };

                if (!string.IsNullOrEmpty(options.WorkingDirectory))
                {
                    startInfo.WorkingDirectory = options.WorkingDirectory;
                }

                using var process = new Process { StartInfo = startInfo };
                var output = new StringBuilder();
                var error = new StringBuilder();

                if (options.RedirectOutput)
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                            output.AppendLine(e.Data);
                    };
                }

                if (options.RedirectError)
                {
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                            error.AppendLine(e.Data);
                    };
                }

                process.Start();

                if (options.RedirectOutput)
                    process.BeginOutputReadLine();
                if (options.RedirectError)
                    process.BeginErrorReadLine();

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(options.TimeoutSeconds));
                await process.WaitForExitAsync(cts.Token);

                stopwatch.Stop();

                var result = new ProcessResult
                {
                    ExitCode = process.ExitCode,
                    Output = output.ToString().TrimEnd(),
                    Error = error.ToString().TrimEnd(),
                    ExecutionTime = stopwatch.Elapsed
                };

                if (options.ShowErrorOnFailure && !result.Success)
                {
                    var errorMessage = $"Process failed with exit code {result.ExitCode}";
                    if (!string.IsNullOrEmpty(result.Error))
                        errorMessage += $"\nError: {result.Error}";

                    // LogHelper.LogToConsole(errorMessage);
                }

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var errorMessage = $"Lỗi chạy process '{fileName}': {ex.Message}";
                // LogHelper.LogToConsole(errorMessage);

                return new ProcessResult
                {
                    ExitCode = -1,
                    Error = errorMessage,
                    ExecutionTime = stopwatch.Elapsed
                };
            }
        }

        /// <summary>
        /// Chạy process với tham số dài và kết quả chi tiết.
        /// </summary>
        public static async Task<ProcessResult> RunProcessWithResultAsync(string fileName, string[] arguments, ProcessOptions options = null)
        {
            var args = arguments != null ? string.Join(" ", arguments) : "";
            return await RunProcessWithResultAsync(fileName, args, options);
        }

        /// <summary>
        /// Chạy process với tham số từ file (hỗ trợ tham số rất dài).
        /// </summary>
        public static async Task<ProcessResult> RunProcessWithArgumentsFileAsync(string fileName, string[] arguments, ProcessOptions options = null)
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                // Ghi tham số vào file tạm
                await File.WriteAllLinesAsync(tempFile, arguments);

                // Chạy process với file tham số
                var startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = $"@{tempFile}",
                    RedirectStandardOutput = options?.RedirectOutput ?? false,
                    RedirectStandardError = options?.RedirectError ?? false,
                    UseShellExecute = options?.UseShellExecute ?? true,
                    CreateNoWindow = options?.CreateNoWindow ?? true
                };

                if (!string.IsNullOrEmpty(options?.WorkingDirectory))
                {
                    startInfo.WorkingDirectory = options.WorkingDirectory;
                }

                using var process = new Process { StartInfo = startInfo };
                var output = new StringBuilder();
                var error = new StringBuilder();
                var stopwatch = Stopwatch.StartNew();

                if (options?.RedirectOutput == true)
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                            output.AppendLine(e.Data);
                    };
                }

                if (options?.RedirectError == true)
                {
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                            error.AppendLine(e.Data);
                    };
                }

                process.Start();

                if (options?.RedirectOutput == true)
                    process.BeginOutputReadLine();
                if (options?.RedirectError == true)
                    process.BeginErrorReadLine();

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(options.TimeoutSeconds));
                await process.WaitForExitAsync(cts.Token);
                stopwatch.Stop();

                var result = new ProcessResult
                {
                    ExitCode = process.ExitCode,
                    Output = output.ToString().TrimEnd(),
                    Error = error.ToString().TrimEnd(),
                    ExecutionTime = stopwatch.Elapsed
                };

                if (options?.ShowErrorOnFailure == true && !result.Success)
                {
                    var errorMessage = $"Process failed with exit code {result.ExitCode}";
                    if (!string.IsNullOrEmpty(result.Error))
                        errorMessage += $"\nError: {result.Error}";

                    // LogHelper.LogToConsole(errorMessage);
                }

                return result;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Lỗi chạy process với file tham số: {ex.Message}";
                // LogHelper.LogToConsole(errorMessage);
                throw;
            }
            finally
            {
                // Xóa file tạm
                if (File.Exists(tempFile))
                {
                    try
                    {
                        File.Delete(tempFile);
                    }
                    catch
                    {
                        // Bỏ qua lỗi xóa file tạm
                    }
                }
            }
        }

        /// <summary>
        /// Kiểm tra process có đang chạy không.
        /// </summary>
        public static bool IsProcessRunning(string processName)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                return processes.Length > 0;
            }
            catch (Exception ex)
            {
                // LogHelper.LogToConsole($"Lỗi kiểm tra process: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Dừng process theo tên.
        /// </summary>
        public static bool StopProcess(string processName, bool forceKill = false)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    if (forceKill)
                        process.Kill();
                    else
                        process.CloseMainWindow();
                }
                return true;
            }
            catch (Exception ex)
            {
                // LogHelper.LogToConsole($"Lỗi dừng process: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lấy thông tin process.
        /// </summary>
        public static ProcessInfo GetProcessInfo(int processId)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                return new ProcessInfo
                {
                    Id = process.Id,
                    ProcessName = process.ProcessName,
                    StartTime = process.StartTime,
                    TotalProcessorTime = process.TotalProcessorTime,
                    WorkingSet = process.WorkingSet64,
                    PrivateMemorySize = process.PrivateMemorySize64
                };
            }
            catch (Exception ex)
            {
                // LogHelper.LogToConsole($"Lỗi lấy thông tin process: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Tạo file .bat để chạy process với tham số rất dài.
        /// </summary>
        public static string CreateBatchFile(string fileName, string[] arguments, string workingDirectory = null, bool pauseOnError = true)
        {
            var batchContent = new StringBuilder();

            // Thêm header
            batchContent.AppendLine("@echo off");
            batchContent.AppendLine("setlocal enabledelayedexpansion");

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                batchContent.AppendLine($"cd /d \"{workingDirectory}\"");
            }

            batchContent.AppendLine();
            batchContent.AppendLine("echo Starting process...");
            batchContent.AppendLine($"echo Command: {fileName}");

            // Tạo command line với tham số
            var commandLine = $"\"{fileName}\"";
            if (arguments != null && arguments.Length > 0)
            {
                foreach (var arg in arguments)
                {
                    // Escape tham số nếu cần
                    var escapedArg = arg.Replace("\"", "\"\"");
                    if (arg.Contains(" ") || arg.Contains("\"") || arg.Contains("&") || arg.Contains("|") || arg.Contains("<") || arg.Contains(">"))
                    {
                        commandLine += $" \"{escapedArg}\"";
                    }
                    else
                    {
                        commandLine += $" {escapedArg}";
                    }
                }
            }

            batchContent.AppendLine($"echo Arguments: {arguments?.Length ?? 0} parameters");
            batchContent.AppendLine();
            batchContent.AppendLine("echo Executing...");
            batchContent.AppendLine(commandLine);

            // Thêm error handling
            if (pauseOnError)
            {
                batchContent.AppendLine("if errorlevel 1 (");
                batchContent.AppendLine("    echo.");
                batchContent.AppendLine("    echo Process failed with error code %errorlevel%");
                batchContent.AppendLine("    echo.");
                batchContent.AppendLine("    pause");
                batchContent.AppendLine("    exit /b %errorlevel%");
                batchContent.AppendLine(")");
            }

            batchContent.AppendLine();
            batchContent.AppendLine("echo Process completed successfully");
            batchContent.AppendLine("exit /b 0");

            // Tạo file .bat
            var tempDir = Path.GetTempPath();
            var batchFileName = $"process_{Guid.NewGuid():N}.bat";
            var batchFilePath = Path.Combine(tempDir, batchFileName);

            File.WriteAllText(batchFilePath, batchContent.ToString(), Encoding.UTF8);
            return batchFilePath;
        }

        /// <summary>
        /// Tạo file .bat với output redirection.
        /// </summary>
        public static string CreateBatchFileWithOutput(string fileName, string[] arguments, string outputFile = null, string errorFile = null, string workingDirectory = null, bool pauseOnError = true)
        {
            var batchContent = new StringBuilder();

            // Thêm header
            batchContent.AppendLine("@echo off");
            batchContent.AppendLine("setlocal enabledelayedexpansion");

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                batchContent.AppendLine($"cd /d \"{workingDirectory}\"");
            }

            batchContent.AppendLine();
            batchContent.AppendLine("echo Starting process with output redirection...");
            batchContent.AppendLine($"echo Command: {fileName}");

            // Tạo command line với tham số
            var commandLine = $"\"{fileName}\"";
            if (arguments != null && arguments.Length > 0)
            {
                foreach (var arg in arguments)
                {
                    var escapedArg = arg.Replace("\"", "\"\"");
                    if (arg.Contains(" ") || arg.Contains("\"") || arg.Contains("&") || arg.Contains("|") || arg.Contains("<") || arg.Contains(">"))
                    {
                        commandLine += $" \"{escapedArg}\"";
                    }
                    else
                    {
                        commandLine += $" {escapedArg}";
                    }
                }
            }

            // Thêm output redirection
            if (!string.IsNullOrEmpty(outputFile))
            {
                commandLine += $" > \"{outputFile}\"";
            }

            if (!string.IsNullOrEmpty(errorFile))
            {
                commandLine += $" 2> \"{errorFile}\"";
            }

            batchContent.AppendLine($"echo Arguments: {arguments?.Length ?? 0} parameters");
            if (!string.IsNullOrEmpty(outputFile))
                batchContent.AppendLine($"echo Output file: {outputFile}");
            if (!string.IsNullOrEmpty(errorFile))
                batchContent.AppendLine($"echo Error file: {errorFile}");
            batchContent.AppendLine();
            batchContent.AppendLine("echo Executing...");
            batchContent.AppendLine(commandLine);

            // Thêm error handling
            if (pauseOnError)
            {
                batchContent.AppendLine("if errorlevel 1 (");
                batchContent.AppendLine("    echo.");
                batchContent.AppendLine("    echo Process failed with error code %errorlevel%");
                if (!string.IsNullOrEmpty(errorFile))
                {
                    batchContent.AppendLine("    echo Error details:");
                    batchContent.AppendLine($"    type \"{errorFile}\"");
                }
                batchContent.AppendLine("    echo.");
                batchContent.AppendLine("    pause");
                batchContent.AppendLine("    exit /b %errorlevel%");
                batchContent.AppendLine(")");
            }

            batchContent.AppendLine();
            batchContent.AppendLine("echo Process completed successfully");
            if (!string.IsNullOrEmpty(outputFile))
                batchContent.AppendLine($"echo Check output in: {outputFile}");
            batchContent.AppendLine("exit /b 0");

            // Tạo file .bat
            var tempDir = Path.GetTempPath();
            var batchFileName = $"process_{Guid.NewGuid():N}.bat";
            var batchFilePath = Path.Combine(tempDir, batchFileName);

            File.WriteAllText(batchFilePath, batchContent.ToString(), Encoding.UTF8);
            return batchFilePath;
        }

        /// <summary>
        /// Chạy process thông qua file .bat với tham số rất dài.
        /// </summary>
        public static async Task<ProcessResult> RunProcessViaBatchAsync(string fileName, string[] arguments, ProcessOptions options = null)
        {
            options ??= new ProcessOptions();
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Tạo file .bat
                var batchFilePath = CreateBatchFile(fileName, arguments, options.WorkingDirectory, options.ShowErrorOnFailure);

                try
                {
                    // Chạy file .bat
                    var batchResult = await RunProcessWithResultAsync("cmd.exe", $"/c \"{batchFilePath}\"", new ProcessOptions
                    {
                        RedirectOutput = options.RedirectOutput,
                        RedirectError = options.RedirectError,
                        UseShellExecute = false,
                        CreateNoWindow = options.CreateNoWindow,
                        WorkingDirectory = options.WorkingDirectory,
                        TimeoutSeconds = options.TimeoutSeconds,
                        ShowErrorOnFailure = false // Không log lỗi ở đây vì đã có trong batch
                    });

                    stopwatch.Stop();

                    var result = new ProcessResult
                    {
                        ExitCode = batchResult.ExitCode,
                        Output = batchResult.Output,
                        Error = batchResult.Error,
                        ExecutionTime = stopwatch.Elapsed
                    };

                    if (options.ShowErrorOnFailure && !result.Success)
                    {
                        var errorMessage = $"Process failed with exit code {result.ExitCode}";
                        if (!string.IsNullOrEmpty(result.Error))
                            errorMessage += $"\nError: {result.Error}";

                        // LogHelper.LogToConsole(errorMessage);
                    }

                    return result;
                }
                finally
                {
                    // Xóa file .bat tạm
                    try
                    {
                        if (File.Exists(batchFilePath))
                            File.Delete(batchFilePath);
                    }
                    catch
                    {
                        // Bỏ qua lỗi xóa file tạm
                    }
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var errorMessage = $"Lỗi chạy process qua batch file: {ex.Message}";
                // LogHelper.LogToConsole(errorMessage);

                return new ProcessResult
                {
                    ExitCode = -1,
                    Error = errorMessage,
                    ExecutionTime = stopwatch.Elapsed
                };
            }
        }

        /// <summary>
        /// Chạy process thông qua file .bat với output redirection.
        /// </summary>
        public static async Task<ProcessResult> RunProcessViaBatchWithOutputAsync(string fileName, string[] arguments, string outputFile = null, string errorFile = null, ProcessOptions options = null)
        {
            options ??= new ProcessOptions();
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Tạo file .bat với output redirection
                var batchFilePath = CreateBatchFileWithOutput(fileName, arguments, outputFile, errorFile, options.WorkingDirectory, options.ShowErrorOnFailure);

                try
                {
                    // Chạy file .bat
                    var batchResult = await RunProcessWithResultAsync("cmd.exe", $"/c \"{batchFilePath}\"", new ProcessOptions
                    {
                        RedirectOutput = false, // Không redirect vì đã có file output
                        RedirectError = false,
                        UseShellExecute = false,
                        CreateNoWindow = options.CreateNoWindow,
                        WorkingDirectory = options.WorkingDirectory,
                        TimeoutSeconds = options.TimeoutSeconds,
                        ShowErrorOnFailure = false
                    });

                    stopwatch.Stop();

                    // Đọc output và error từ file nếu có
                    var output = "";
                    var error = "";

                    if (!string.IsNullOrEmpty(outputFile) && File.Exists(outputFile))
                    {
                        output = await File.ReadAllTextAsync(outputFile, options.OutputEncoding);
                    }

                    if (!string.IsNullOrEmpty(errorFile) && File.Exists(errorFile))
                    {
                        error = await File.ReadAllTextAsync(errorFile, options.OutputEncoding);
                    }

                    var result = new ProcessResult
                    {
                        ExitCode = batchResult.ExitCode,
                        Output = output,
                        Error = error,
                        ExecutionTime = stopwatch.Elapsed
                    };

                    if (options.ShowErrorOnFailure && !result.Success)
                    {
                        var errorMessage = $"Process failed with exit code {result.ExitCode}";
                        if (!string.IsNullOrEmpty(result.Error))
                            errorMessage += $"\nError: {result.Error}";

                        // LogHelper.LogToConsole(errorMessage);
                    }

                    return result;
                }
                finally
                {
                    // Xóa file .bat tạm
                    try
                    {
                        if (File.Exists(batchFilePath))
                            File.Delete(batchFilePath);
                    }
                    catch
                    {
                        // Bỏ qua lỗi xóa file tạm
                    }
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var errorMessage = $"Lỗi chạy process qua batch file với output: {ex.Message}";
                // LogHelper.LogToConsole(errorMessage);

                return new ProcessResult
                {
                    ExitCode = -1,
                    Error = errorMessage,
                    ExecutionTime = stopwatch.Elapsed
                };
            }
        }

        /// <summary>
        /// Tạo và lưu file .bat để chạy sau (không tự động xóa).
        /// </summary>
        public static string CreatePersistentBatchFile(string fileName, string[] arguments, string outputPath, string workingDirectory = null, bool pauseOnError = true)
        {
            var batchContent = new StringBuilder();

            // Thêm header với thông tin
            batchContent.AppendLine("@echo off");
            batchContent.AppendLine("setlocal enabledelayedexpansion");
            batchContent.AppendLine();
            batchContent.AppendLine("REM ========================================");
            batchContent.AppendLine("REM Auto-generated batch file for process execution");
            batchContent.AppendLine($"REM Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            batchContent.AppendLine($"REM Command: {fileName}");
            batchContent.AppendLine($"REM Arguments: {arguments?.Length ?? 0} parameters");
            batchContent.AppendLine("REM ========================================");
            batchContent.AppendLine();

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                batchContent.AppendLine($"cd /d \"{workingDirectory}\"");
                batchContent.AppendLine();
            }

            batchContent.AppendLine("echo Starting process...");
            batchContent.AppendLine($"echo Command: {fileName}");
            batchContent.AppendLine($"echo Arguments: {arguments?.Length ?? 0} parameters");
            batchContent.AppendLine();
            batchContent.AppendLine("echo Executing...");

            // Tạo command line với tham số
            var commandLine = $"\"{fileName}\"";
            if (arguments != null && arguments.Length > 0)
            {
                foreach (var arg in arguments)
                {
                    var escapedArg = arg.Replace("\"", "\"\"");
                    if (arg.Contains(" ") || arg.Contains("\"") || arg.Contains("&") || arg.Contains("|") || arg.Contains("<") || arg.Contains(">"))
                    {
                        commandLine += $" \"{escapedArg}\"";
                    }
                    else
                    {
                        commandLine += $" {escapedArg}";
                    }
                }
            }

            batchContent.AppendLine(commandLine);

            // Thêm error handling
            if (pauseOnError)
            {
                batchContent.AppendLine("if errorlevel 1 (");
                batchContent.AppendLine("    echo.");
                batchContent.AppendLine("    echo Process failed with error code %errorlevel%");
                batchContent.AppendLine("    echo.");
                batchContent.AppendLine("    pause");
                batchContent.AppendLine("    exit /b %errorlevel%");
                batchContent.AppendLine(")");
            }

            batchContent.AppendLine();
            batchContent.AppendLine("echo Process completed successfully");
            batchContent.AppendLine("echo.");
            batchContent.AppendLine("pause");
            batchContent.AppendLine("exit /b 0");

            // Lưu file .bat
            File.WriteAllText(outputPath, batchContent.ToString(), Encoding.UTF8);
            return outputPath;
        }
    }

    /// <summary>
    /// Thông tin process.
    /// </summary>
    public class ProcessInfo
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan TotalProcessorTime { get; set; }
        public long WorkingSet { get; set; }
        public long PrivateMemorySize { get; set; }
    }
}