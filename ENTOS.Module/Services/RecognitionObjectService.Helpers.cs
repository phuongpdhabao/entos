using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Persistent.Base;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Module.Services
{
    public partial class RecognitionObjectService
    {
        #region SourceCode4521ImportCode
                        internal static string GenerateSilence(double durationSeconds, int index)
        {
            string output = Path.Combine(Path.GetTempPath(), $"silence_{index}_{DateTime.Now:yyyyMMddHHmmss}.m4a");
            var args = $"-f lavfi -i anullsrc=r=44100:cl=stereo -t {durationSeconds.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)} -acodec aac -b:a 128k \"{output}\"";
            var (o, e, code) = RunFFmpegCommand(args);
            return code == 0 && File.Exists(output) ? output : null;
        }

        internal static void ConcatenateAudioFiles(string listFilePath, string outputPath)
        {
            var args = $"-avoid_negative_ts make_zero -f concat -safe 0 -i \"{listFilePath}\" -c copy \"{outputPath}\"";

            var result = RunFFmpegCommand(args);

            if (!string.IsNullOrWhiteSpace(result.error))
            {
                Console.WriteLine("❌ FFmpeg Error: " + result.error);
            }

            if (result.exitCode != 0)
            {
                Console.WriteLine("❌ Lỗi khi ghép các file âm thanh.");
            }
            else
            {
                Console.WriteLine("✅ Ghép âm thanh thành công!");
            }
        }
        internal static string ExtractAudioSegment(string videoPath, int beginFrame, int endFrame, int fps, int index)
        {
            double startTime = beginFrame / (double)fps;
            double duration = (endFrame - beginFrame + 1) / (double)fps;

            string outputPath = Path.Combine(Path.GetTempPath(), $"audio_part_{index}_{DateTime.Now:yyyyMMddHHmmss}.m4a");

            var args = $"-y -i \"{videoPath}\" -ss {startTime.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)} -t {duration.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)} -vn -acodec aac -b:a 128k \"{outputPath}\"";
            var (output, error, exitCode) = RunFFmpegCommand(args);

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("❌ FFmpeg Error: " + error);
            }

            return exitCode == 0 && File.Exists(outputPath) ? outputPath : null;
        }
        internal void MergeAudioToVideo(string videoFilePath, string audioFilePath)
        {
            string tempOutput = Path.Combine(Path.GetTempPath(), $"merged_{DateTime.Now:yyyyMMddHHmmss}.mp4");

            var args = $"-i \"{videoFilePath}\" -i \"{audioFilePath}\" -c:v copy -c:a aac -shortest \"{tempOutput}\"";
            var (output, error, exitCode) = RunFFmpegCommand(args);

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("❌ FFmpeg Error: " + error);
            }

            if (exitCode == 0)
            {
                File.Delete(videoFilePath);
                File.Move(tempOutput, videoFilePath);
            }
            else
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "❌ Ghép âm thanh thất bại.", InformationType.Error);
            }
        }
        internal static (string output, string error, int exitCode) RunFFmpegCommand(string arguments)
        {
            var psi = new System.Diagnostics.ProcessStartInfo("ffmpeg", arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new System.Diagnostics.Process { StartInfo = psi };
            process.Start();

            var outputTask = Task.Run(() => process.StandardOutput.ReadToEnd());
            var errorTask = Task.Run(() => process.StandardError.ReadToEnd());

            process.WaitForExit();

            Task.WhenAll(outputTask, errorTask).Wait();

            return (outputTask.Result, errorTask.Result, process.ExitCode);
        }
        internal string CreateTemporaryFile(string path, int index)
        {
            try
            {
                string extension = Path.GetExtension(path);

                string tempDir = Path.Combine(Path.GetTempPath(), "VideoProcessing");
                Directory.CreateDirectory(tempDir);

                string tempPath = Path.Combine(tempDir, $"{index}{extension}");

                File.Copy(path, tempPath, true);

                return tempPath;
            }
            catch (Exception ex)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"⚠️ Lỗi khi tạo tệp tạm thời: {ex.Message}", InformationType.Error);
                return path;
            }
        }
        internal void DeleteTemporaryFile(string tempPath)
        {
            try
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
            catch (Exception ex)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"⚠️ Lỗi khi xóa tệp tạm: {ex.Message}", InformationType.Error);
            }
        }
        private static string RemoveUnicode(string input)
        {
            var formD = input.Normalize(System.Text.NormalizationForm.FormD);

            var normalized = new System.Text.StringBuilder();
            foreach (var c in formD)
            {
                if (c == 'Đ')
                    normalized.Append('D');
                else if (c == 'đ')
                    normalized.Append('d');
                else
                    normalized.Append(c);
            }

            var regEx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            return regEx.Replace(normalized.ToString(), string.Empty).Normalize(System.Text.NormalizationForm.FormC);
        }
        internal static void DrawInfo(OpenCvSharp.Mat frame, string name, RecognitionPosition pos, float timestamp)
        {
            var rect = new OpenCvSharp.Rect(pos.Horizontal.Value, pos.Vertical.Value, pos.Size.Value, pos.Size.Value);
            OpenCvSharp.Cv2.Rectangle(frame, rect, OpenCvSharp.Scalar.Red, 2);

            string cleanedName = RemoveUnicode(name);

            var time = TimeSpan.FromSeconds(timestamp);
            string label = $"{cleanedName} - {time:hh\\:mm\\:ss}";

            OpenCvSharp.Cv2.PutText(frame, label, new OpenCvSharp.Point(rect.X, rect.Y - 10),
                OpenCvSharp.HersheyFonts.HersheySimplex, 0.8, OpenCvSharp.Scalar.Yellow, 2);
        }

        internal static bool IsVideoFile(string path)
        {
            var extensions = new[] { ".mp4", ".avi", ".mov", ".mkv" };
            return extensions.Contains(Path.GetExtension(path).ToLower());
        }
        internal static OpenCvSharp.Mat ResizeWithPadding(OpenCvSharp.Mat image, int targetWidth, int targetHeight)
        {
            var originalWidth = image.Width;
            var originalHeight = image.Height;

            float aspectRatio = (float)originalWidth / originalHeight;

            int newWidth, newHeight;

            if (originalWidth > originalHeight)
            {
                newWidth = targetWidth;
                newHeight = (int)(newWidth / aspectRatio);
            }
            else
            {
                newHeight = targetHeight;
                newWidth = (int)(newHeight * aspectRatio);
            }

            var resized = new OpenCvSharp.Mat();
            OpenCvSharp.Cv2.Resize(image, resized, new OpenCvSharp.Size(newWidth, newHeight));

            var result = new OpenCvSharp.Mat(new OpenCvSharp.Size(targetWidth, targetHeight), OpenCvSharp.MatType.CV_8UC3, new OpenCvSharp.Scalar(255, 255, 255));

            int offsetX = (targetWidth - newWidth) / 2;
            int offsetY = (targetHeight - newHeight) / 2;

            resized.CopyTo(result[new OpenCvSharp.Rect(offsetX, offsetY, newWidth, newHeight)]);

            return result;
        }


        #endregion SourceCode4521ImportCode

        #region SourceCode3450ImportCode
          public static bool IsImageFile(string path) => Path.GetExtension(path).ToLower() is ".jpg" or ".jpeg" or ".png" or ".bmp";
        #endregion SourceCode3450ImportCode

        #region SourceCode3448ImportCode
            public static void AdjustCropSize(int imgWidth, int imgHeight, ref int x, ref int y, ref int width, ref int height)
        {
            if (x + width > imgWidth) width = imgWidth - x;
            if (y + height > imgHeight) height = imgHeight - y;
            if (width <= 0 || height <= 0)
            {
                width = 1;
                height = 1;
            }
        }
        #endregion SourceCode3448ImportCode
    }
}
